// Copyright © 2025-Present The DClare Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to embed content.
/// </summary>
/// <param name="logger">The service used to perform logging</param>
/// <param name="componentDefinitionResolver">The service used to resolve <see cref="ReferenceableComponentDefinition"/>s.</param>
/// <param name="kernelFactory">The service used to create <see cref="Kernel"/>s.</param>
public class ContentEmbedder(ILogger<ContentEmbedder> logger, IComponentDefinitionResolver componentDefinitionResolver, IKernelFactory kernelFactory)
    : IContentEmbedder
{

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the service used to resolve <see cref="ReferenceableComponentDefinition"/>s.
    /// </summary>
    protected IComponentDefinitionResolver ComponentDefinitionResolver { get; } = componentDefinitionResolver;

    /// <summary>
    /// Gets the service used to create <see cref="Kernel"/>s.
    /// </summary>
    protected IKernelFactory KernelFactory { get; } = kernelFactory;

    /// <inheritdoc/>
    public virtual async Task EmbedAsync(IFormFile file, ContentEmbeddingOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(options);
        var vectorStoreDefinition = await ComponentDefinitionResolver.ResolveAsync<VectorStoreDefinition>(options.VectorStore.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var embedderDefinition = await ComponentDefinitionResolver.ResolveAsync<EmbeddingModelDefinition>(options.Embedding.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var llmDefinition = options.Llm == null ? null : await ComponentDefinitionResolver.ResolveAsync<LlmDefinition>(options.Llm.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var kernelDefinition = new KernelDefinition()
        {
            Llm = llmDefinition,
            Knowledge = new()
            {
                Embedding = embedderDefinition,
                Store = vectorStoreDefinition,
            }
        };
        var kernel = await KernelFactory.CreateAsync(kernelDefinition, null, cancellationToken).ConfigureAwait(false);
        var recordCollection = kernel.GetRequiredService<IVectorStoreRecordCollection>();
        try
        {
            if (!await recordCollection.CollectionExistsAsync(cancellationToken).ConfigureAwait(false)) await recordCollection.CreateCollectionAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await recordCollection.CreateCollectionAsync(cancellationToken).ConfigureAwait(false);
        }
        switch (file.ContentType)
        {
            case MediaTypeNames.Text.Html:

                break;
            case MediaTypeNames.Application.Json:

                break;
            case MediaTypeNames.Application.Pdf:
                await EmbedPdfFileAsync(file, kernel, cancellationToken).ConfigureAwait(false);
                break;
            case MediaTypeNames.Text.Plain:

                break;
            case MediaTypeNames.Text.RichText:

                break;
            case MediaTypeNames.Text.Xml or MediaTypeNames.Application.Xml:

                break;
            default:
                throw new ProblemDetailsException(Problems.UnsupportedFileContentType(file.ContentType));
        }
    }

    /// <summary>
    /// Embeds the specified PDF file.
    /// </summary>
    /// <param name="file">The PDF file to embed.</param>
    /// <param name="kernel">The <see cref="Kernel"/> to use.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task EmbedPdfFileAsync(IFormFile file, Kernel kernel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(kernel);
        var textEmbeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var recordCollection = kernel.GetRequiredService<IVectorStoreRecordCollection>();
        var chatCompletionService = kernel.Services.GetService<IChatCompletionService>();
        using var stream = file.OpenReadStream();
        var extractImages = chatCompletionService != null;
        var fileContents = ExtractPdfFileContents(stream, extractImages, cancellationToken);
        if (!extractImages) fileContents = fileContents.Where(c => c.Image == null);
        var batches = fileContents.Chunk(10);
        foreach (var batch in batches)
        {

            var textContentTasks = batch.Select(async content =>
            {
                if (content.Text != null) return content;
                if (chatCompletionService == null) return null;
                try
                {
                    var textFromImage = await ConvertImageToTextAsync(chatCompletionService, content.Image!.Value, cancellationToken).ConfigureAwait(false);
                    return new RawContent { Text = textFromImage, PageNumber = content.PageNumber };
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Failed to convert the image into text {ex}. Skipping.", ex);
                    return null;
                }
            });
            var textContent = (await Task.WhenAll(textContentTasks).ConfigureAwait(false)).Where(c => c != null);
            var recordTasks = textContent.Select(async content => new TextEmbeddingRecord
            {
                Text = content!.Text,
                Reference = new()
                {
                    Id = file.Name,
                    Type = file.ContentType,
                    PageNumber = content.PageNumber,
                    Link = $"{file.Name}#page={content.PageNumber}"
                },
                TextEmbedding = await GenerateTextEmbeddingAsync(textEmbeddingService, content.Text!, cancellationToken).ConfigureAwait(false)
            });
            var records = await Task.WhenAll(recordTasks).ConfigureAwait(false);
            var upsertedKeys = await recordCollection.UpsertAsync(records, cancellationToken: cancellationToken).ConfigureAwait(false);
            foreach (var key in upsertedKeys) Logger.LogDebug("Upserted record '{key}' into vector store", key);
            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Extracts contents from the specified PDF file stream.
    /// </summary>
    /// <param name="stream">The PDF file stream to extract content from.</param>
    /// <param name="extractImages">A boolean indicating whether or not to extract images.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing extracted contents.</returns>
    protected virtual IEnumerable<RawContent> ExtractPdfFileContents(Stream stream, bool extractImages, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var pdf = PdfDocument.Open(stream);
        foreach (var page in pdf.GetPages())
        {
            if (cancellationToken.IsCancellationRequested) break;
            if(extractImages)
            {
                foreach (var image in page.GetImages())
                {
                    if (image.TryGetPng(out var png)) yield return new RawContent
                    {
                        Image = png,
                        PageNumber = page.Number
                    };
                    else Logger.LogWarning("Unsupported image format on page {pageNumber}", page.Number);
                }
            }
            var blocks = DefaultPageSegmenter.Instance.GetBlocks(page.GetWords());
            foreach (var block in blocks)
            {
                if (cancellationToken.IsCancellationRequested) break;
                yield return new RawContent { Text = block.Text, PageNumber = page.Number };
            }
        }
    }

    /// <summary>
    /// Converts the specified image to text.
    /// </summary>
    /// <param name="chatCompletionService">The <see cref="IChatCompletionService"/> used to convert the image to text.</param>
    /// <param name="imageBytes">The bytes of the image to convert.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A description of the specified image.</returns>
    protected virtual async Task<string> ConvertImageToTextAsync(IChatCompletionService chatCompletionService, ReadOnlyMemory<byte> imageBytes, CancellationToken cancellationToken)
    {
        var tries = 0;
        while (true)
        {
            try
            {
                var chatHistory = new ChatHistory();
                chatHistory.AddUserMessage([new TextContent("What's in this image?"), new ImageContent(imageBytes, "image/png"),]);
                var result = await chatCompletionService.GetChatMessageContentsAsync(chatHistory, cancellationToken: cancellationToken).ConfigureAwait(false);
                return string.Join("\n", result.Select(x => x.Content));
            }
            catch (HttpOperationException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
            {
                tries++;
                if (tries < 3)
                {
                    Logger.LogDebug("Failed to generate text from image. Error: {ex}", ex);
                    Logger.LogDebug("Retrying text to image conversion...");
                    await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
                }
                else throw;
            }
        }
    }

    /// <summary>
    /// Generates embeddings for the specified text.
    /// </summary>
    /// <param name="textEmbeddingGenerationService">The service used to generate embeddings.</param>
    /// <param name="text">The text to embed.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The generated embedding.</returns>
    protected virtual async Task<ReadOnlyMemory<float>> GenerateTextEmbeddingAsync(ITextEmbeddingGenerationService textEmbeddingGenerationService, string text, CancellationToken cancellationToken)
    {
        var tries = 0;
        while (true)
        {
            try
            {
                return await textEmbeddingGenerationService.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken).ConfigureAwait(false);
            }
            catch (HttpOperationException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
            {
                tries++;
                if (tries < 3)
                {
                    Logger.LogDebug("Failed to generate embedding. Error: {ex}", ex);
                    Logger.LogDebug("Retrying embedding generation...");
                    await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
                }
                else throw;
            }
        }
    }

}

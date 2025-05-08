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
using Microsoft.SemanticKernel.Embeddings;
using System.IO;
using System.Threading;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to embed content.
/// </summary>
/// <param name="componentDefinitionResolver">The service used to resolve <see cref="ReferenceableComponentDefinition"/>s.</param>
/// <param name="kernelFactory">The service used to create <see cref="Kernel"/>s.</param>
public class ContentEmbedder(IComponentDefinitionResolver componentDefinitionResolver, IKernelFactory kernelFactory)
    : IContentEmbedder
{

    /// <inheritdoc/>
    public virtual async Task EmbedAsync(IFormFile file, ContentEmbeddingOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(options);
        var vectorStoreDefinition = await componentDefinitionResolver.ResolveAsync<VectorStoreDefinition>(options.VectorStore.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var embedderDefinition = await componentDefinitionResolver.ResolveAsync<EmbeddingModelDefinition>(options.Embedding.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var kernelDefinition = new KernelDefinition()
        {
            Knowledge = new()
            {
                Embedding = embedderDefinition,
                Store = vectorStoreDefinition
            }
        };
        var kernel = await kernelFactory.CreateAsync(kernelDefinition, null, cancellationToken).ConfigureAwait(false);
        var embedder = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
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
        using var stream = file.OpenReadStream();
        var fileContents = ExtractPdfFileContents(stream, cancellationToken);
        var batches = fileContents.Chunk(10);
        foreach (var batch in batches)
        {
            var textContentTasks = batch.Select(async content =>
            {
                if (content.Text != null) return content;
                var textFromImage = await ConvertImageToTextWithRetryAsync(chatCompletionService, content.Image!.Value, cancellationToken).ConfigureAwait(false);
                return new RawContent { Text = textFromImage, PageNumber = content.PageNumber };
            });
            var textContent = await Task.WhenAll(textContentTasks).ConfigureAwait(false);
            var recordTasks = textContent.Select(async content => new TextEmbeddingRecord<string>
            {
                Key = Guid.NewGuid().ToString("N"),
                Text = content.Text,
                Reference = new()
                {
                    Id = $"{new FileInfo(filePath).Name}#page={content.PageNumber}",
                    Link = $"{new Uri(new FileInfo(filePath).FullName).AbsoluteUri}#page={content.PageNumber}"
                },
                TextEmbedding = await GenerateEmbeddingsWithRetryAsync(textEmbeddingGenerationService, content.Text!, cancellationToken: cancellationToken).ConfigureAwait(false)
            });
            var records = await Task.WhenAll(recordTasks).ConfigureAwait(false);
            var upsertedKeys = await vectorStoreRecordCollection.UpsertAsync(records, cancellationToken: cancellationToken).ConfigureAwait(false);
            foreach (var key in upsertedKeys)
            {
                Console.WriteLine($"Upserted record '{key}' into VectorDB");
            }
            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
        }
    }

    protected virtual IEnumerable<RawContent> ExtractPdfFileContents(Stream stream, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var pdf = PdfDocument.Open(stream);
        foreach (var page in pdf.GetPages())
        {
            if (cancellationToken.IsCancellationRequested) break;
            foreach (var image in page.GetImages())
            {
                if (image.TryGetPng(out var png)) yield return new RawContent
                {
                    Image = png,
                    PageNumber = page.Number
                };
                else Console.WriteLine($"Unsupported image format on page {page.Number}");
            }
            var blocks = DefaultPageSegmenter.Instance.GetBlocks(page.GetWords());
            foreach (var block in blocks)
            {
                if (cancellationToken.IsCancellationRequested) break;
                yield return new RawContent { Text = block.Text, PageNumber = page.Number };
            }
        }
    } 

}

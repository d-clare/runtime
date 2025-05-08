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

using DClare.Runtime.Integration.Commands.VectorStores;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;

namespace DClare.Runtime.Application.Commands.VectorStores;

/// <summary>
/// Represents the service used to handle <see cref="SearchTextCommand"/>s.
/// </summary>
/// <param name="componentDefinitionResolver">The service used to resolve <see cref="ReferenceableComponentDefinition"/>s.</param>
/// <param name="kernelFactory">The service used to create <see cref="Kernel"/>s.</param>
public sealed class SearchTextCommandHandler(IComponentDefinitionResolver componentDefinitionResolver, IKernelFactory kernelFactory)
    : ICommandHandler<SearchTextCommand, IAsyncEnumerable<SemanticSearchResult>>
{

    /// <inheritdoc/>
    public async Task<IOperationResult<IAsyncEnumerable<SemanticSearchResult>>> HandleAsync(SearchTextCommand command, CancellationToken cancellationToken = default)
    {
        var vectorStoreDefinition = await componentDefinitionResolver.ResolveAsync<VectorStoreDefinition>(command.VectorStore.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var embedderDefinition = await componentDefinitionResolver.ResolveAsync<EmbeddingModelDefinition>(command.Parameters.Embedding.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
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
        var vector = await embedder.GenerateEmbeddingAsync(command.Parameters.Text, kernel, cancellationToken).ConfigureAwait(false);
        IAsyncEnumerable<SemanticSearchResult> results;
        if (vectorStoreDefinition.Provider.Name == VectorStoreProvider.Redis)
        {
            var vectorStore = kernel.GetRequiredService<IVectorStoreRecordCollection<string, TextEmbeddingRecord<string>>>();
            results = vectorStore.SearchEmbeddingAsync(vector, command.Parameters.Top, new()
            {
                Skip = command.Parameters.Skip
            }, cancellationToken).Select(r => r.Record.AsSearchResult());
        }
        else
        {
            var vectorStore = kernel.GetRequiredService<IVectorStoreRecordCollection<Guid, TextEmbeddingRecord<Guid>>>();
            results = vectorStore.SearchEmbeddingAsync(vector, command.Parameters.Top, new()
            {
                Skip = command.Parameters.Skip
            }, cancellationToken).Select(r => r.Record.AsSearchResult());
        }
        return this.Ok(results);
    }

}

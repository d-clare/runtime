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

namespace DClare.Runtime.Application.Commands.VectorStores;

/// <summary>
/// Represents the service used to handle <see cref="UploadFileCommand"/>s.
/// </summary>
/// <param name="contentEmbedder">The service used to embed content.</param>
public sealed class UploadFileCommandHandler(IContentEmbedder contentEmbedder)
    : ICommandHandler<UploadFileCommand>
{

    /// <inheritdoc/>
    public async Task<IOperationResult> HandleAsync(UploadFileCommand command, CancellationToken cancellationToken = default)
    {
        await contentEmbedder.EmbedAsync(command.Parameters.File, new()
        {
            VectorStore = command.VectorStore,
            Embedding = command.Parameters.Embedding,
            Llm = command.Parameters.Llm
        }, cancellationToken).ConfigureAwait(false);
        return this.Ok();
    }

}

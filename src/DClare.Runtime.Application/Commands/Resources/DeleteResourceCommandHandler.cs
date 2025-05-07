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

using DClare.Runtime.Integration.Commands.Resources;

namespace DClare.Runtime.Application.Commands.Resources;

/// <summary>
/// Represents the service used to handle <see cref="DeleteResourceCommand{TResource}"/>s
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/> to create</typeparam>
/// <param name="repository">The service used to manage <see cref="IResource"/>s</param>
public sealed class DeleteResourceCommandHandler<TResource>(IResourceRepository repository)
    : ICommandHandler<DeleteResourceCommand<TResource>, TResource>
    where TResource : class, IResource, new()
{

    /// <inheritdoc/>
    public async Task<IOperationResult<TResource>> HandleAsync(DeleteResourceCommand<TResource> command, CancellationToken cancellationToken)
    {
        var resource = await repository.RemoveAsync<TResource>(command.Name, command.Namespace, false, cancellationToken).ConfigureAwait(false);
        return this.Ok(resource);
    }

}

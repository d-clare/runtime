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
/// Represents the service used to handle <see cref="PatchResourceCommand{TResource}"/>s
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/> to patch</typeparam>
/// <param name="repository">The service used to manage <see cref="IResource"/>s</param>
public sealed class PatchResourceCommandHandler<TResource>(IResourceRepository repository)
    : ICommandHandler<PatchResourceCommand<TResource>, TResource>
    where TResource : class, IResource, new()
{

    /// <inheritdoc/>
    public async Task<IOperationResult<TResource>> HandleAsync(PatchResourceCommand<TResource> command, CancellationToken cancellationToken)
    {
        var resource = await repository.PatchAsync<TResource>(command.Patch, command.Name, command.Namespace, command.ResourceVersion, false, cancellationToken).ConfigureAwait(false);
        return this.Ok(resource);
    }

}

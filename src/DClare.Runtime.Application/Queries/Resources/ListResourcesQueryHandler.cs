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

using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Application.Queries.Resources;

/// <summary>
/// Represents the service used to handle <see cref="ListResourcesQuery{TResource}"/> instances
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/>s to list</typeparam>
/// <param name="repository">The service used to manage <see cref="IResource"/>s</param>
public sealed class ListResourcesQueryHandler<TResource>(IResourceRepository repository)
    : IQueryHandler<ListResourcesQuery<TResource>, Neuroglia.Data.Infrastructure.ResourceOriented.ICollection<TResource>>
    where TResource : class, IResource, new()
{

    /// <inheritdoc/>
    public async Task<IOperationResult<Neuroglia.Data.Infrastructure.ResourceOriented.ICollection<TResource>>> HandleAsync(ListResourcesQuery<TResource> query, CancellationToken cancellationToken)
    {
        return this.Ok(await repository.ListAsync<TResource>(query.Namespace, query.LabelSelectors, query.MaxResults, query.ContinuationToken, cancellationToken).ConfigureAwait(false));
    }

}

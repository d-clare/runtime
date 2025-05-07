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

namespace DClare.Runtime.Integration.Queries.Resources;

/// <summary>
/// Represents the <see cref="IQuery{TResult}"/> used to list all <see cref="IResource"/>s.
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/>s to list.</typeparam>
/// <param name="namespace">The namespace the <see cref="IResource"/>s to get belong to, if any.</param>
/// <param name="labelSelectors">A <see cref="List{T}"/> containing the <see cref="LabelSelector"/>s used to filter <see cref="IResource"/>s by.</param>
/// <param name="maxResults">The maximum amount of results to list.</param>
/// <param name="continuationToken">The token, if any, used to continue enumerating the results.</param>
[Description("Represents the query used to list resources.")]
public class ListResourcesQuery<TResource>(string? @namespace, IEnumerable<LabelSelector>? labelSelectors, ulong? maxResults, string? continuationToken)
    : Query<Neuroglia.Data.Infrastructure.ResourceOriented.ICollection<TResource>>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the namespace the <see cref="IResource"/>s to get belong to, if any.
    /// </summary>
    [Description("The namespace, if any, the resources to list belong to.")]
    public string? Namespace { get; } = @namespace;

    /// <summary>
    /// Gets a <see cref="List{T}"/> containing the <see cref="LabelSelector"/>s used to filter <see cref="IResource"/>s by.
    /// </summary>
    [Description("Label selectors, if any, used to filter the resources to list.")]
    public IEnumerable<LabelSelector>? LabelSelectors { get; } = labelSelectors;

    /// <summary>
    /// Gets the maximum amount of results to list.
    /// </summary>
    [Description("The maximum amount of results to list.")]
    public ulong? MaxResults { get; } = maxResults;

    /// <summary>
    /// Gets the token, if any, used to continue enumerating the results.
    /// </summary>
    [Description("The token, if any, used to continue enumerating the results.")]
    public string? ContinuationToken { get; } = continuationToken;

}

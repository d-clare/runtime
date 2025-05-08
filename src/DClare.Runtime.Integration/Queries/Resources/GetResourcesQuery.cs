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
/// Represents the <see cref="IQuery{TResult}"/> used to retrieves <see cref="IResource"/>s of the specified type.
/// </summary>
/// <param name="namespace">The namespace the <see cref="IResource"/>s to get belong to, if any.</param>
/// <param name="labelSelectors">A <see cref="List{T}"/> containing the <see cref="LabelSelector"/>s used to filter <see cref="IResource"/>s by.</param>
[Description("Represents the query used to get resources.")]
public class GetResourcesQuery<TResource>(string? @namespace = null, IEnumerable<LabelSelector>? labelSelectors = null)
    : Query<IAsyncEnumerable<TResource>>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the namespace the <see cref="IResource"/>s to get belong to, if any.
    /// </summary>
    [Description("The namespace, if any, the resources to get belong to.")]
    public string? Namespace { get; } = @namespace;

    /// <summary>
    /// Gets a <see cref="List{T}"/> containing the <see cref="LabelSelector"/>s used to filter <see cref="IResource"/>s by.
    /// </summary>
    [Description("Label selectors, if any, used to filter the get to enumerate.")]
    public IEnumerable<LabelSelector>? LabelSelectors { get; } = labelSelectors;

}

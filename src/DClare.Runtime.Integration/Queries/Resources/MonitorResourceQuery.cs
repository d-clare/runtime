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
/// Represents the <see cref="IQuery{TResult}"/> used to monitor a specific <see cref="IResource"/>.
/// </summary>
/// <param name="name">The name of the <see cref="IResource"/> to monitor.</param>
/// <param name="namespace">The namespace the <see cref="IResource"/> to monitor belongs to, if any.</param>
[Description("Represents the query used to monitor a resource.")]
public class MonitorResourceQuery<TResource>(string name, string? @namespace)
    : Query<IAsyncEnumerable<IResourceWatchEvent<TResource>>>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the name of the <see cref="IResource"/>s to monitor.
    /// </summary>
    [Description("The name of the resource to monitor.")]
    public string Name { get; } = name;

    /// <summary>
    /// Gets the namespace the <see cref="IResource"/>s to get belongs to, if any.
    /// </summary>
    [Description("The namespace, if any, the resource to monitor belong to.")]
    public string? Namespace { get; } = @namespace;

}

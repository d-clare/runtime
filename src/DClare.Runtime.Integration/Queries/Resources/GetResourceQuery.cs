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
/// Represents the <see cref="IQuery{TResult}"/> used to get an existing <see cref="IResource"/>.
/// </summary>
/// <typeparam name="TResource">The type of the <see cref="IResource"/> to get.</typeparam>
/// <param name="name">The name of the <see cref="IResource"/> to get.</param>
/// <param name="namespace">The namespace the <see cref="IResource"/> to get belongs to.</param>
[Description("Represents the query used to get an existing resource.")]
public class GetResourceQuery<TResource>(string name, string? @namespace)
    : Query<TResource>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the name of the <see cref="IResource"/> to get.
    /// </summary>
    [Description("The name of the resource to get.")]
    public string Name { get; } = name;

    /// <summary>
    /// Gets the namespace the <see cref="IResource"/> to get belongs to.
    /// </summary>
    [Description("The namespace the resource to get belongs to.")]
    public string? Namespace { get; } = @namespace;

}

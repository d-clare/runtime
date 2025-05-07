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

namespace DClare.Runtime.Integration.Commands.Resources;

/// <summary>
/// Represents the <see cref="ICommand"/> used to delete an existing <see cref="IResource"/>.
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/> to create.</typeparam>
/// <param name="name">The name of the <see cref="IResource"/> to delete.</param>
/// <param name="namespace">The namespace the <see cref="IResource"/> to delete belongs to.</param>
[Description("Represents the command used to delete a resource.")]
public class DeleteResourceCommand<TResource>(string name, string? @namespace)
    : Command<TResource>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the name of the <see cref="IResource"/> to delete.
    /// </summary>
    [Description("The name of the resource to delete.")]
    public string Name { get; } = name;

    /// <summary>
    /// Gets the namespace the <see cref="IResource"/> to delete belongs to.
    /// </summary>
    [Description("The namespace, if any, the resource to delete belongs to.")]
    public string? Namespace { get; } = @namespace;

}

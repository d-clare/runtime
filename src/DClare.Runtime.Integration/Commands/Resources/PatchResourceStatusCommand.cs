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
/// Represents the <see cref="ICommand"/> used to patch the status of an <see cref="IResource"/>.
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/> to patch.</typeparam>
/// <param name="name">The name of the <see cref="IResource"/> to patch.</param>
/// <param name="namespace">The namespace the <see cref="IResource"/> to patch belongs to.</param>
/// <param name="patch">The patch to apply.</param>
/// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
[Description("Represents the command used to patch the status of a resource.")]
public class PatchResourceStatusCommand<TResource>(string name, string? @namespace, Patch patch, string? resourceVersion)
    : Command<TResource>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the name of the <see cref="IResource"/> to patch
    /// </summary>
    [Description("The name of the resource to patch.")]
    public string Name { get; } = name;

    /// <summary>
    /// Gets the name of the <see cref="IResource"/> to patch
    /// </summary>
    [Description("The namespace, if any, the resource to patch belongs to.")]
    public string? Namespace { get; } = @namespace;

    /// <summary>
    /// Gets the patch to apply
    /// </summary>
    [Description("The patch to apply.")]
    public Patch Patch { get; } = patch ?? throw new ArgumentNullException(nameof(patch));

    /// <summary>
    /// Gets the expected resource version, if any, used for optimistic concurrency
    /// </summary>
    [Description("The expected resource version, if any, used for optimistic concurrency.")]
    public string? ResourceVersion { get; } = resourceVersion;

}

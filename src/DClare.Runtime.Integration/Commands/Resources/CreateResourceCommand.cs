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
/// Represents the command used to create a new <see cref="IResource"/>.
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/> to create.</typeparam>
/// <param name="resource">The resource to create.</param>
[Description("Represents the command used to create a new resource.")]
public class CreateResourceCommand<TResource>(TResource resource)
    : Command<TResource>
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the resource to create.
    /// </summary>
    [Description("The resource to create.")]
    public TResource Resource { get; } = resource ?? throw new ArgumentNullException(nameof(resource));

}

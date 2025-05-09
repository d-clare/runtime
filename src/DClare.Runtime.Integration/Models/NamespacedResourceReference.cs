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

namespace DClare.Runtime.Integration.Models;

/// <summary>
/// Represents a reference to a namespaced resource.
/// </summary>
[Description("Represents a reference to a namespaced resource.")]
[DataContract]
public record NamespacedResourceReference
{

    /// <summary>
    /// Gets or sets the namespace the reference resource belongs to.
    /// </summary>
    [Required, MinLength(1)]
    [Description("The namespace the reference resource belongs to.")]
    public virtual required string Namespace { get; set; }

    /// <summary>
    /// Gets or sets the name of the reference resource.
    /// </summary>
    [Required, MinLength(1)]
    [Description("The name of the reference resource.")]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets the qualified name of the reference resource.
    /// </summary>
    /// <returns>The qualified name of the reference resource.</returns>
    public virtual string GetQualifiedName() => $"{Name}.{Namespace}";

    /// <inheritdoc/>
    public override string ToString() => GetQualifiedName();

}

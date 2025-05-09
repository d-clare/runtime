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

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Defines a service used to resolve <see cref="ReferenceableComponentDefinition"/>s.
/// </summary>
public interface IComponentDefinitionResolver
{

    /// <summary>
    /// Resolves the specified <see cref="ReferenceableComponentDefinition"/>.
    /// </summary>
    /// <typeparam name="TComponent">The type of <see cref="ReferenceableComponentDefinition"/> to resolve.</typeparam>
    /// <param name="reference">A reference to the <see cref="ReferenceableComponentDefinition"/> to resolve.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The resolved <see cref="ReferenceableComponentDefinition"/>.</returns>
    Task<TComponent> ResolveAsync<TComponent>(string reference, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
        where TComponent : ReferenceableComponentDefinition, new();

}

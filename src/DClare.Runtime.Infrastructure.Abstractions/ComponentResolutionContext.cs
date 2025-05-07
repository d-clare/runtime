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

namespace DClare.Runtime;

/// <summary>
/// Represents the context used to resolve components, including their resolution scope and any optional locally defined component collection.
/// </summary>
public class ComponentResolutionContext
{

    /// <summary>
    /// Gets or sets the scope in which components are resolved.
    /// </summary>
    public virtual required string Scope { get; init; }

    /// <summary>
    /// Gets or sets the optional collection of components available in the current context.
    /// </summary>
    public virtual ComponentDefinitionCollection? Components { get; init; }

}

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
/// Defines the components required to create and configure a <see cref="Kernel"/>.
/// </summary>
public class KernelDefinition
{

    /// <summary>
    /// Gets the definition of the large language model (LLM) to be used by the <see cref="Kernel"/>.
    /// </summary>
    public virtual LlmDefinition? Llm { get; init; }

    /// <summary>
    /// Gets the definition of the knowledge base to be integrated with the <see cref="Kernel"/>, if any.
    /// </summary>
    public virtual KnowledgeDefinition? Knowledge { get; init; }

    /// <summary>
    /// Gets a key/definition mapping of the <see cref="Kernel"/>'s toolsets.
    /// </summary>
    public virtual EquatableDictionary<string, ToolsetDefinition>? Toolsets { get; init; }

}
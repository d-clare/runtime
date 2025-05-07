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

using DClare.Sdk.Models;

namespace DClare.Runtime.Integration.Models;

/// <summary>
/// Represents the specification of an AI agent.
/// </summary>
[Description("Represents the specification of an AI agent.")]
[DataContract]
public record AgentSpec
{

    /// <summary>
    /// Gets or sets the agent's definition.
    /// </summary>
    [Description("The agent's definition.")]
    [Required]
    [DataMember(Name = "definition", Order = 1), JsonPropertyName("definition"), JsonPropertyOrder(1), YamlMember(Alias = "definition", Order = 1)]
    public virtual AgentDefinition Definition { get; set; } = null!;

}

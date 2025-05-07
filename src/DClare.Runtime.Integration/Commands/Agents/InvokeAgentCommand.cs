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

namespace DClare.Runtime.Integration.Commands.Agents;

/// <summary>
/// Represents the command to invoke an AI Agent with a message, optionally within a conversational session.
/// </summary>
[Description("Represents the command to invoke an AI Agent with a message, optionally within a conversational session.")]
[DataContract]
public class InvokeAgentCommand
    : Command<ChatResponseStream>
{

    /// <summary>
    /// Gets/sets the name of the agent to invoke.
    /// </summary>
    [Description("The name of the agent to invoke.")]
    [Required]
    [DataMember(Name = "agent", Order = 1), JsonPropertyName("agent"), JsonPropertyOrder(1), YamlMember(Alias = "agent", Order = 1)]
    public virtual required NamespacedResourceReference Agent { get; set; }

    /// <summary>
    /// Gets/sets the invocation parameters.
    /// </summary>
    [Description("The invocation parameters.")]
    [Required,]
    [DataMember(Name = "parameters", Order = 2), JsonPropertyName("parameters"), JsonPropertyOrder(2), YamlMember(Alias = "parameters", Order = 2)]
    public virtual required InvokeAgentCommandParameters Parameters { get; set; }

}

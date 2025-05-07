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
/// Represents the parameters of the command used to invoke an agent.
/// </summary>
[Description("Represents the parameters of the command used to invoke an agent.")]
[DataContract]
public record InvokeAgentCommandParameters
{

    /// <summary>
    /// Gets/sets the input message to process.
    /// </summary>
    [Description("The input message to process")]
    [Required, MinLength(1)]
    [DataMember(Name = "message", Order = 1), JsonPropertyName("message"), JsonPropertyOrder(1), YamlMember(Alias = "message", Order = 1)]
    public virtual string Message { get; set; } = null!;

    /// <summary>
    /// Gets/sets the options used to configure the agent's invocation.
    /// </summary>
    [Description("The options used to configure the agent's invocation")]
    [DataMember(Name = "options", Order = 2), JsonPropertyName("options"), JsonPropertyOrder(2), YamlMember(Alias = "options", Order = 2)]
    public virtual AgentInvocationOptions? Options { get; set; }

}

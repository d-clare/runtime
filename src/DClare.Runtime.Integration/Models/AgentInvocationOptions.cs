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
/// Represents the options used to configure an agent's invocation.
/// </summary>
[Description("Represents the options used to configure an agent's invocation.")]
[DataContract]
public record AgentInvocationOptions
{

    /// <summary>
    /// Gets/sets the unique identifier of the chat thread used to retrieve or continue a conversation context.
    /// </summary>
    [Description("The unique identifier of the chat thread used to retrieve or continue a conversation context.")]
    [DataMember(Name = "chatId", Order = 1), JsonPropertyName("chatId"), JsonPropertyOrder(1), YamlMember(Alias = "chatId", Order = 1)]
    public virtual string? ChatId { get; set; }

    /// <summary>
    /// Gets/sets the session identifier to scope the invocation to a broader user interaction context.
    /// </summary>
    [Description("The session identifier to scope the invocation to a broader user interaction context.")]
    [DataMember(Name = "sessionId", Order = 2), JsonPropertyName("sessionId"), JsonPropertyOrder(2), YamlMember(Alias = "sessionId", Order = 2)]
    public virtual string? SessionId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the chat session.<para></para>
    /// If the caller is authenticated, this property must be left unset as it will be automatically populated from the authenticated user's subject claim.<para></para>
    /// If the caller is not authenticated, this property is required and must be explicitly set.
    /// </summary>
    [Description("The user identifier. Ignored if the caller is authenticated, in which case it is derived from the current user's subject. Required if the caller is anonymous.")]
    [DataMember(Name = "userId", Order = 3), JsonPropertyName("userId"), JsonPropertyOrder(3), YamlMember(Alias = "userId", Order = 3)]
    public virtual string? UserId { get; set; }

    /// <summary>
    /// Gets/sets a key/value mapping of parameters that influence the agent's behavior or response.
    /// </summary>
    [Description("A key/value mapping of parameters that influence the agent's behavior or response.")]
    [DataMember(Name = "parameters", Order = 4), JsonPropertyName("parameters"), JsonPropertyOrder(4), YamlMember(Alias = "parameters", Order = 4)]
    public virtual IDictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Gets/sets a value indicating whether to include message metadata in the agent's response.
    /// </summary>
    [Description("Indicates whether to include message metadata in the agent's response.")]
    [DataMember(Name = "includeMetadata", Order = 5), JsonPropertyName("includeMetadata"), JsonPropertyOrder(5), YamlMember(Alias = "includeMetadata", Order = 5)]
    public virtual bool IncludeMetadata { get; set; }

}

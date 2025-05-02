// Copyright � 2025-Present The DClare Authors
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
/// Represents a chat consisting of a unique identifier, user, agent, an optional display name, and a collection of exchanged messages.
/// </summary>
[DataContract, Description("A chat session consisting of an identifier, user, agent, optional display name, and a collection of messages exchanged with an agent")]
public record Chat
{

    /// <summary>
    /// Initializes a new <see cref="Chat"/>
    /// </summary>
    public Chat() { }

    /// <summary>
    /// Initializes a new <see cref="Chat"/>
    /// </summary>
    /// <param name="id">The <see cref="Chat"/>'s user-defined identifier.</param>
    /// <param name="userId">The user associated with the <see cref="Chat"/> session.</param>
    /// <param name="agentName">The name of the agent involved in the <see cref="Chat"/> session.</param>
    /// <param name="name">An optional display name for the <see cref="Chat"/>.</param>
    /// <param name="messages">The collection of messages exchanged during the <see cref="Chat"/>.</param>
    public Chat(string id, string userId, string agentName, string? name, IEnumerable<ChatMessage> messages)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(agentName);
        Key = BuildKey(id, userId, agentName);
        Id = id;
        UserId = userId;
        AgentName = agentName;
        Name = name;
        Messages = messages;
    }

    /// <summary>
    /// Gets or sets the key that uniquely identifies the chat.
    /// </summary>
    [Description("The key that uniquely identifies the chat")]
    [Required, MinLength(1)]
    [DataMember(Name = "key", Order = 1), JsonPropertyName("key"), JsonPropertyOrder(1), YamlMember(Alias = "key", Order = 1)]
    public virtual string Key { get; set; } = null!;

    /// <summary>
    /// Gets or sets the chat's user-defined identifier.
    /// </summary>
    [Description("The key that uniquely identifies the chat")]
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 2), JsonPropertyName("id"), JsonPropertyOrder(2), YamlMember(Alias = "id", Order = 2)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the user associated with the chat session.
    /// </summary>
    [Description("The user associated with the chat session.")]
    [Required, MinLength(1)]
    [DataMember(Name = "userId", Order = 3), JsonPropertyName("userId"), JsonPropertyOrder(3), YamlMember(Alias = "userId", Order = 3)]
    public virtual string UserId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the agent involved in the chat session.
    /// </summary>
    [Description("The name of the agent involved in the chat session.")]
    [Required, MinLength(1)]
    [DataMember(Name = "agentName", Order = 4), JsonPropertyName("agentName"), JsonPropertyOrder(4), YamlMember(Alias = "agentName", Order = 4)]
    public virtual string AgentName { get; set; } = null!;

    /// <summary>
    /// Gets or sets an optional display name for the chat session.
    /// </summary>
    [Description("An optional display name for the chat session.")]
    [DataMember(Name = "name", Order = 5), JsonPropertyName("name"), JsonPropertyOrder(5), YamlMember(Alias = "name", Order = 5)]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the collection of messages exchanged during the chat session.
    /// </summary>
    [Description("The collection of messages exchanged during the chat session.")]
    [DataMember(Name = "messages", Order = 6), JsonPropertyName("messages"), JsonPropertyOrder(6), YamlMember(Alias = "messages", Order = 6)]
    public virtual IEnumerable<ChatMessage> Messages { get; set; } = [];

    /// <summary>
    /// Builds a new <see cref="Chat"/> key
    /// </summary>
    /// <param name="id">The <see cref="Chat"/>'s user-defined identifier.</param>
    /// <param name="userId">The id of the user the <see cref="Chat"/> belongs to</param>
    /// <param name="agentName">The name of the agent the <see cref="Chat"/> concerns</param>
    /// <returns>A new <see cref="Chat"/> key</returns>
    public static string BuildKey(string id, string userId, string agentName) => $"{userId}-{agentName}-{id}";

}


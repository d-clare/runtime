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
/// Represents a response sent by an AI Agent to a prompt request.
/// </summary>
[Description("Represents a response sent by an AI Agent to a prompt request.")]
[DataContract]
public record ChatResponse
{

    /// <summary>
    /// Initializes a new <see cref="ChatResponse"/>.
    /// </summary>
    public ChatResponse() { }

    /// <summary>
    /// Initializes a new <see cref="ChatResponse"/>.
    /// </summary>
    /// <param name="id">The response's unique identifier.</param>
    /// <param name="messages">The messages produced by the chat.</param>
    public ChatResponse(string id, IEnumerable<ChatMessage> messages)
    {
        Id = id;
        Messages = messages;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the chat response.
    /// </summary>
    [Description("The response's unique identifier")]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of messages returned by the agent.
    /// </summary>
    [Description("A stream of messages produced by the chat")]
    [DataMember(Name = "messages", Order = 2), JsonPropertyName("messages"), JsonPropertyOrder(2), YamlMember(Alias = "messages", Order = 2)]
    public virtual IEnumerable<ChatMessage> Messages { get; set; } = [];

}

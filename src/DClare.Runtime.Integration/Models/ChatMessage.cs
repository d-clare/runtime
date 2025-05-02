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
/// Represents a chat message
/// </summary>
[DataContract, Description("A chat message")]
public record ChatMessage
{

    /// <summary>
    /// Initializes a new <see cref="ChatMessage"/>
    /// </summary>
    public ChatMessage() { }

    /// <summary>
    /// Initializes a new <see cref="ChatMessage"/>
    /// </summary>
    /// <param name="role">The message's role</param>
    /// <param name="content">The message's content</param>
    /// <param name="metadata">The message's metadata, if any</param>
    public ChatMessage(string role, string? content, IReadOnlyDictionary<string, object?>? metadata = null)
    {
        Role = role;
        Content = content;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets or sets the role associated with the message (e.g., user, assistant, system).
    /// </summary>
    [Description("The message's role (e.g., user, assistant, system)")]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual string Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets the textual content of the message.
    /// </summary>
    [Description("The message's content")]
    [DataMember(Name = "content", Order = 2), JsonPropertyName("content"), JsonPropertyOrder(2), YamlMember(Alias = "content", Order = 2)]
    public virtual string? Content { get; set; }

    /// <summary>
    /// Gets or sets additional metadata associated with the message, if any.
    /// </summary>
    [Description("The message's metadata, if any")]
    [DataMember(Name = "metadata", Order = 3), JsonPropertyName("metadata"), JsonPropertyOrder(3), YamlMember(Alias = "metadata", Order = 3)]
    public virtual IReadOnlyDictionary<string, object?>? Metadata { get; set; }

}

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
/// Represents the result of invoking a chat with a user message, including a unique response identifier and a stream of output messages.
/// </summary>
[Description("Represents the result of invoking a chat with a user message, including a unique response identifier and a stream of output messages.")]
[DataContract]
public record ChatResponseStream
{

    /// <summary>
    /// Initializes a new <see cref="ChatResponseStream"/>.
    /// </summary>
    public ChatResponseStream() { }

    /// <summary>
    /// Initializes a new <see cref="ChatResponseStream"/>.
    /// </summary>
    /// <param name="id">The response's unique identifier.</param>
    /// <param name="stream">The stream of chat messages produced by the chat.</param>
    public ChatResponseStream(string id, IAsyncEnumerable<StreamingChatMessageContent> stream)
    {
        Id = id;
        Stream = stream;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the chat response.
    /// </summary>
    [Description("The response's unique identifier")]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the stream of chat messages produced by the agent.
    /// </summary>
    [Description("A stream of chat messages produced by the chat")]
    [DataMember(Name = "stream", Order = 2), JsonPropertyName("stream"), JsonPropertyOrder(2), YamlMember(Alias = "stream", Order = 2)]
    public virtual IAsyncEnumerable<StreamingChatMessageContent> Stream { get; set; } = default!;

}

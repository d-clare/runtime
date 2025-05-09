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

using DClare.Runtime.Integration.Serialization.Json;

namespace DClare.Runtime.Integration.Models;

/// <summary>
/// Represents a structured message consisting of one or more typed content parts, such as text or binary data.
/// </summary>
[Description("Represents a structured message consisting of one or more typed content parts, such as text or binary data.")]
[DataContract]
[JsonConverter(typeof(JsonMessageConverter))]
public record Message
{

    /// <summary>
    /// Gets or sets the role associated with the message (e.g., user, assistant, system).
    /// </summary>
    [Description("The message's role (e.g., user, assistant, system).")]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual required string Role { get; init; }

    /// <summary>
    /// Gets or sets the name, if any, of the message's author.
    /// </summary>
    [Description("The name, if any, of the message's author.")]
    [DataMember(Name = "author", Order = 2), JsonPropertyName("author"), JsonPropertyOrder(2), YamlMember(Alias = "author", Order = 2)]
    public virtual string? Author { get; init; }

    /// <summary>
    /// Gets or sets the parts that compose the message.
    /// </summary>
    [Description("The parts that compose the message.")]
    [DataMember(Name = "parts", Order = 3), JsonPropertyName("parts"), JsonPropertyOrder(3), YamlMember(Alias = "parts", Order = 3)]
    public virtual IReadOnlyList<MessagePart> Parts { get; init; } = null!;

    /// <summary>
    /// Gets or sets additional metadata associated with the message, if any.
    /// </summary>
    [Description("The message's metadata, if any.")]
    [DataMember(Name = "metadata", Order = 4), JsonPropertyName("metadata"), JsonPropertyOrder(4), YamlMember(Alias = "metadata", Order = 4)]
    public virtual IReadOnlyDictionary<string, object?>? Metadata { get; init; }

    /// <summary>
    /// Gets a key/value mapping of the message's extension properties, if any.
    /// </summary>
    [JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; init; }

    /// <summary>
    /// Gets or sets the textual content of the message.
    /// </summary>
    [Description("The message's content.")]
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual string? Content
    {
        get => Parts?.OfType<TextPart>().FirstOrDefault()?.Text;
        init => Parts = [new TextPart()
            {
                MimeType = MediaTypeNames.Text.Plain,
                Encoding = Encoding.UTF8,
                Text = value!
            }];
    }

    /// <summary>
    /// The encoding of the text content.
    /// </summary>
    [Description("The message's content encoding, if any.")]
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public Encoding? Encoding => Parts?.OfType<TextPart>().FirstOrDefault()?.Encoding;

}

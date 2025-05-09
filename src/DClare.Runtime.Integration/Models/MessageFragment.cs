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
/// Represents a fragment of a message produced during a streamed agent response, containing partial content, role, and optional metadata.
/// </summary>
[Description("Represents a fragment of a message produced during a streamed agent response, containing partial content, role, and optional metadata.")]
[DataContract]
[JsonConverter(typeof(JsonMessageFragmentConverter))]
public record MessageFragment
{

    /// <summary>
    /// Gets or sets the role associated with the message fragment, if any.
    /// </summary>
    [Description("The message's role")]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual string? Role { get; init; }

    /// <summary>
    /// Gets or sets the parts that compose the streamed message fragment.
    /// </summary>
    [Description("The content parts of the streamed message fragment.")]
    [DataMember(Name = "parts", Order = 2), JsonPropertyName("parts"), JsonPropertyOrder(2), YamlMember(Alias = "parts", Order = 2)]
    public virtual IReadOnlyList<MessageFragmentPart>? Parts { get; set; }

    /// <summary>
    /// Gets or sets any metadata associated with the message fragment.
    /// </summary>
    [Description("The message's metadata, if any")]
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual IReadOnlyDictionary<string, object?>? Metadata { get; init; }

    /// <summary>
    /// Gets a key/value mapping of the message's extension properties, if any.
    /// </summary>
    [JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; init; }

    /// <summary>
    /// Gets or sets the textual content of the message fragment, if any.
    /// </summary>
    [Description("The message fragment's text content.")]
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual string? Content
    {
        get => Parts?.OfType<TextFragmentPart>().FirstOrDefault()?.Text;
        set
        {
            Parts = [new TextFragmentPart
            {
                Text = value
            }];
        }
    }

    /// <summary>
    /// The encoding of the text content fragment.
    /// </summary>
    [Description("The message fragment's content encoding, if any.")]
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public Encoding? Encoding => Parts?.OfType<TextFragmentPart>().FirstOrDefault()?.Encoding;

}

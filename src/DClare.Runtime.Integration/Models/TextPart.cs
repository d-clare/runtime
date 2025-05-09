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
/// Represents a chat message content that contains text.
/// </summary>
[Description("Represents a chat message content that contains text.")]
[DataContract]
public record TextPart
    : MessagePart
{

    /// <summary>
    /// Gets the text content.
    /// </summary>
    [Description("The text content.")]
    [DataMember(Name = "text", Order = 3), JsonPropertyName("text"), JsonPropertyOrder(3), YamlMember(Alias = "text", Order = 3)]
    public virtual required string Text { get; init; }

    /// <summary>
    /// Gets the text content.
    /// </summary>
    [Description("The name of the text's encoding, if specified.")]
    [DataMember(Name = "encoding", Order =4), JsonPropertyName("encoding"), JsonPropertyOrder(4), YamlMember(Alias = "encoding", Order = 4)]
    public virtual string? EncodingName { get; init; }

    /// <summary>
    /// Gets the text's encoding.
    /// </summary>
    [Description("The text's encoding, if specified.")]
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual Encoding? Encoding
    {
        get => string.IsNullOrWhiteSpace(EncodingName) ? null : Encoding.GetEncoding(EncodingName);
        init => EncodingName = value?.WebName;
    }

}

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
/// Represents a chat message.
/// </summary>
[Description("Represents a chat message.")]
[DataContract]
[JsonConverter(typeof(JsonMessagePartConverter))]
public abstract record MessagePart
{

    /// <summary>
    /// Gets the content's MIME type.
    /// </summary>
    [Description("The content's MIME type.")]
    [DataMember(Name = "mimeType", Order = 1), JsonPropertyName("mimeType"), JsonPropertyOrder(1), YamlMember(Alias = "mimeType", Order = 1)]
    public virtual string? MimeType { get; init; }

    /// <summary>
    /// Gets the id of the model, if any, used to generate the content.
    /// </summary>
    [Description("The id of the model, if any, used to generate the content.")]
    [DataMember(Name = "modelId", Order = 2), JsonPropertyName("modelId"), JsonPropertyOrder(2), YamlMember(Alias = "modelId", Order = 2)]
    public virtual string? ModelId { get; init; }

    /// <summary>
    /// Gets a key/value mapping of the metadata properties, if any, associated to the message content
    /// </summary>
    [Description("A key/value mapping of the metadata properties, if any, associated to the message content.")]
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual IReadOnlyDictionary<string, object?>? Metadata { get; init; }

    /// <summary>
    /// Gets a key/value mapping of the content's extension properties, if any.
    /// </summary>
    [JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; init; }

}

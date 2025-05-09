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
/// Represents a single part of a streamed message fragment, such as partial text or binary data.
/// </summary>
[Description("Represents a single part of a streamed message fragment, such as partial text or binary data.")]
[DataContract]
[JsonConverter(typeof(JsonMessageFragmentPartConverter))]
public abstract record MessageFragmentPart
{

    /// <summary>
    /// Gets the id of the model, if any, used to generate the content.
    /// </summary>
    [Description("The id of the model, if any, used to generate the content.")]
    [DataMember(Name = "modelId", Order = 1), JsonPropertyName("modelId"), JsonPropertyOrder(1), YamlMember(Alias = "modelId", Order = 1)]
    public virtual string? ModelId { get; init; }

    /// <summary>
    /// Gets a key/value mapping of the metadata associated with the fragment part, if any.
    /// </summary>
    [Description("The fragment part's metadata, if any.")]
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual IReadOnlyDictionary<string, object?>? Metadata { get; init; }

    /// <summary>
    /// Gets a key/value mapping of the extension properties, if any.
    /// </summary>
    [JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; init; }

}

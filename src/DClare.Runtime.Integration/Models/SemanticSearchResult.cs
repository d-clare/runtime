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

using Microsoft.Extensions.VectorData;

namespace DClare.Runtime.Integration.Models;

/// <summary>
/// Represents a text embedding search result.
/// </summary>
[Description("Represents a text embedding search result.")]
[DataContract]
public record SemanticSearchResult
{

    /// <summary>
    /// Gets or sets the raw text content to be indexed and embedded.
    /// </summary>
    [Description("The raw text content to be indexed and embedded.")]
    [VectorStoreRecordData]
    [DataMember(Name = "text", Order = 1), JsonPropertyName("text"), JsonPropertyOrder(1), YamlMember(Alias = "text", Order = 1)]
    public virtual string? Text { get; set; }

    /// <summary>
    /// Gets or sets metadata about the source from which the text snippet was extracted, including its identifier, type, position (page or section), and an optional link.
    /// </summary>
    [Description("Metadata about the source from which the text snippet was extracted, including its identifier, type, position (page or section), and an optional link.")]
    [VectorStoreRecordData]
    [DataMember(Name = "reference", Order = 2), JsonPropertyName("reference"), JsonPropertyOrder(2), YamlMember(Alias = "reference", Order = 2)]
    public virtual TextEmbeddingRecordReference? Reference { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when this record was created or indexed.
    /// </summary>
    [Description("The timestamp indicating when this record was created or indexed.")]
    [VectorStoreRecordData]
    [DataMember(Name = "createdAt", Order = 3), JsonPropertyName("createdAt"), JsonPropertyOrder(3), YamlMember(Alias = "createdAt", Order = 3)]
    public virtual DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets a list of tags that categorize or annotate the text snippet.
    /// </summary>
    [Description("A list of tags that categorize or annotate the text snippet.")]
    [VectorStoreRecordData]
    [DataMember(Name = "tags", Order = 4), JsonPropertyName("tags"), JsonPropertyOrder(4), YamlMember(Alias = "tags", Order = 4)]
    public virtual IReadOnlyList<string>? Tags { get; set; }

    /// <summary>
    /// Gets or sets additional custom metadata as key-value pairs.
    /// </summary>
    [Description("Additional custom metadata as key-value pairs.")]
    [VectorStoreRecordData]
    [DataMember(Name = "metadata", Order = 5), JsonPropertyName("metadata"), JsonPropertyOrder(5), YamlMember(Alias = "metadata", Order = 5)]
    public virtual Dictionary<string, string>? Metadata { get; set; }

}

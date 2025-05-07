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

namespace DClare.Runtime.Application;

/// <summary>
/// Represents a text snippet enriched with a vector embedding and metadata, including reference context
/// for semantic search and traceability. Typically used in vector stores to enable retrieval-augmented reasoning.
/// </summary>
/// <typeparam name="TKey">The type of the unique key identifying the record.</typeparam>
public record TextEmbeddingRecord<TKey>
{

    /// <summary>
    /// Gets or sets the unique identifier for this record.
    /// </summary>
    [VectorStoreRecordKey]
    public virtual required TKey Key { get; set; }

    /// <summary>
    /// Gets or sets the raw text content to be indexed and embedded.
    /// </summary>
    [VectorStoreRecordData]
    public virtual string? Text { get; set; }

    /// <summary>
    /// Gets or sets metadata about the source from which the text snippet was extracted, including its identifier, type, position (page or section), and an optional link.
    /// </summary>
    [VectorStoreRecordData]
    public virtual TextEmbeddingRecordReference? Reference { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when this record was created or indexed.
    /// </summary>
    [VectorStoreRecordData]
    public virtual DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets a list of tags that categorize or annotate the text snippet.
    /// </summary>
    [VectorStoreRecordData]
    public virtual IReadOnlyList<string>? Tags { get; set; }

    /// <summary>
    /// Gets or sets additional custom metadata as key-value pairs.
    /// </summary>
    [VectorStoreRecordData]
    public virtual Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the semantic vector embedding representing the content of this text snippet.
    /// </summary>
    [VectorStoreRecordVector(1536)]
    public virtual ReadOnlyMemory<float> TextEmbedding { get; set; }

}

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
public record TextEmbeddingRecord
    : SemanticSearchResult
{

    /// <summary>
    /// Gets or sets the semantic vector embedding representing the content of this text snippet.
    /// </summary>
    [VectorStoreRecordVector(1536)]
    public virtual ReadOnlyMemory<float> TextEmbedding { get; set; }

}


/// <summary>
/// Represents a text snippet enriched with a vector embedding and metadata, including reference context
/// for semantic search and traceability. Typically used in vector stores to enable retrieval-augmented reasoning.
/// </summary>
/// <typeparam name="TKey">The type of the unique key identifying the record.</typeparam>
public record TextEmbeddingRecord<TKey>
    : TextEmbeddingRecord
{

    /// <summary>
    /// Gets or sets the unique identifier for this record.
    /// </summary>
    [VectorStoreRecordKey]
    public virtual required TKey Key { get; set; }

}

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
/// Represents the options used to configure content embedding.
/// </summary>
[Description("Represents the options used to configure content embedding.")]
[DataContract]
public record ContentEmbeddingOptions
{

    /// <summary>
    /// Gets/sets a reference to the vector store to store embeddings into.
    /// </summary>
    [Description("A reference to the vector store to store embeddings into.")]
    [Required]
    [DataMember(Name = "vectorStore", Order = 1), JsonPropertyName("vectorStore"), JsonPropertyOrder(1), YamlMember(Alias = "vectorStore", Order = 1)]
    public virtual required NamespacedResourceReference VectorStore { get; set; }

    /// <summary>
    /// Gets/sets a reference to the embedding model to use.
    /// </summary>
    [Description("A reference to the embedding model to use.")]
    [Required]
    [DataMember(Name = "embedding", Order = 2), JsonPropertyName("embedding"), JsonPropertyOrder(2), YamlMember(Alias = "embedding", Order = 2)]
    public virtual required NamespacedResourceReference Embedding { get; set; }

    /// <summary>
    /// Gets/sets a reference to the Large Language Model (LLM), if any, to use for converting images to text.
    /// </summary>
    [Description("A reference to the Large Language Model (LLM), if any, to use for converting images to text..")]
    [Required]
    [DataMember(Name = "llm", Order = 3), JsonPropertyName("llm"), JsonPropertyOrder(3), YamlMember(Alias = "llm", Order = 3)]
    public virtual NamespacedResourceReference? Llm { get; set; }

}

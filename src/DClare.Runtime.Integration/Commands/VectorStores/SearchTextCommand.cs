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

namespace DClare.Runtime.Integration.Commands.VectorStores;

/// <summary>
/// Represents the command used to search a vector store.
/// </summary>
[Description("Represents the command used to search a vector store.")]
[DataContract]
public class SearchTextCommand
    : Command<IAsyncEnumerable<SemanticSearchResult>>
{

    /// <summary>
    /// Gets/sets a reference to the vector store to invoke.
    /// </summary>
    [Description("A reference to the vector store to invoke.")]
    [Required]
    [DataMember(Name = "vectorStore", Order = 1), JsonPropertyName("vectorStore"), JsonPropertyOrder(1), YamlMember(Alias = "vectorStore", Order = 1)]
    public virtual required NamespacedResourceReference VectorStore { get; set; }

    /// <summary>
    /// Gets/sets the parameters used to configure the search to perform.
    /// </summary>
    [Description("The parameters used to configure the search to perform.")]
    [Required]
    [DataMember(Name = "parameters", Order = 2), JsonPropertyName("parameters"), JsonPropertyOrder(2), YamlMember(Alias = "parameters", Order = 2)]
    public virtual required SearchTextCommandParameters Parameters { get; set; }

}

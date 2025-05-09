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

using Microsoft.AspNetCore.Http;

namespace DClare.Runtime.Integration.Models;

/// <summary>
/// Represents the parameters of the command used to search a vector store.
/// </summary>
[Description("Represents the parameters of the command used to search a vector store.")]
[DataContract]
public record SearchTextCommandParameters
{

    /// <summary>
    /// Gets the default maximum number of results to return.
    /// </summary>
    public const int DefaultTop = 5;

    /// <summary>
    /// Gets or sets the input text to search for similar items in the vector store.
    /// </summary>
    [Description("The input text to search for similar items in the vector store.")]
    [Required, MinLength(1)]
    [DataMember(Name = "text", Order = 1), JsonPropertyName("text"), JsonPropertyOrder(1), YamlMember(Alias = "text", Order = 1)]
    public virtual string Text { get; set; } = null!;

    /// <summary>
    /// Gets/sets a reference to the embedding model to use.
    /// </summary>
    [Description("A reference to the embedding model to use.")]
    [Required]
    [DataMember(Name = "embedding", Order = 2), JsonPropertyName("embedding"), JsonPropertyOrder(2), YamlMember(Alias = "embedding", Order = 2)]
    public virtual required NamespacedResourceReference Embedding { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of results to return. Defaults to '5'.
    /// </summary>
    [Description("The maximum number of results to return. Defaults to '5'.")]
    [DefaultValue(DefaultTop)]
    [DataMember(Name = "top", Order = 3), JsonPropertyName("top"), JsonPropertyOrder(3), YamlMember(Alias = "top", Order = 3)]
    public virtual int Top { get; set; } = DefaultTop;

    /// <summary>
    /// Gets or sets the number of results to skip before returning results, that is, the index of the first result to return.
    /// </summary>
    [Description("The number of results to skip before returning results, that is, the index of the first result to return.")]
    [DefaultValue(0)]
    [DataMember(Name = "skip", Order = 4), JsonPropertyName("skip"), JsonPropertyOrder(4), YamlMember(Alias = "skip", Order = 4)]
    public virtual int Skip { get; set; }

}

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
/// Represents the parameters of the command used to upload a file to a vector store.
/// </summary>
[Description("Represents the parameters of the command used to upload a file to a vector store.")]
[DataContract]
public record UploadFileCommandParameters
{

    /// <summary>
    /// Gets/sets the file to upload.
    /// </summary>
    [Description("The file to upload.")]
    [Required]
    [DataMember(Name = "file", Order = 1), JsonPropertyName("file"), JsonPropertyOrder(1), YamlMember(Alias = "file", Order = 1)]
    public virtual required IFormFile File { get; set; }

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
    [DataMember(Name = "llm", Order = 3), JsonPropertyName("llm"), JsonPropertyOrder(3), YamlMember(Alias = "llm", Order = 3)]
    public virtual NamespacedResourceReference? Llm { get; set; }

}

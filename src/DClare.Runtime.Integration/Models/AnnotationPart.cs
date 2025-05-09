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
/// Represents a chat message content that contains an annotation.
/// </summary>
[Description("Represents a chat message content that contains an annotation.")]
[DataContract]
public record AnnotationPart
    : MessagePart
{

    /// <summary>
    /// Gets the identifier, if any, of the annotated file.
    /// </summary>
    [Description("The text content.")]
    [DataMember(Name = "fileId", Order = 3), JsonPropertyName("fileId"), JsonPropertyOrder(3), YamlMember(Alias = "fileId", Order = 3)]
    public virtual string? FileId { get; init; }

    /// <summary>
    /// Gets the exact quoted text from the original content.
    /// </summary>
    [DataMember(Name = "quote", Order = 4), JsonPropertyName("quote"), JsonPropertyOrder(4), YamlMember(Alias = "quote", Order = 4)]
    public virtual required string Quote { get; init; }

    /// <summary>
    /// Gets the zero-based start index of the quoted text in the original content.
    /// </summary>
    [DataMember(Name = "start", Order = 5), JsonPropertyName("start"), JsonPropertyOrder(5), YamlMember(Alias = "start", Order = 5)]
    public virtual int Start { get; init; }

    /// <summary>
    /// Gets the zero-based end index (exclusive) of the quoted text in the original content.
    /// </summary>
    [DataMember(Name = "end", Order = 6), JsonPropertyName("end"), JsonPropertyOrder(6), YamlMember(Alias = "end", Order = 6)]
    public virtual int End { get; init; }

}

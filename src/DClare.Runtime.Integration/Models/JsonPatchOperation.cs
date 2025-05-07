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
/// Represents a single JSON Patch operation, as defined by RFC 6902.
/// </summary>
[Description("Represents a single JSON Patch operation, as defined by RFC 6902.")]
[DataContract]
public record JsonPatchOperation
{

    /// <summary>
    /// Gets or sets the operation to be performed. Valid values are 'add', 'remove', 'replace', 'move', 'copy', and 'test'.
    /// </summary>
    [Description("The operation to be performed. Valid values are 'add', 'remove', 'replace', 'move', 'copy', and 'test'.")]
    [Required, AllowedValues(JsonPatchOperationType.Add, JsonPatchOperationType.Remove, JsonPatchOperationType.Replace, JsonPatchOperationType.Move, JsonPatchOperationType.Copy, JsonPatchOperationType.Test)]
    [DataMember(Name ="op", Order = 1), JsonPropertyName("op"), JsonPropertyOrder(1), YamlMember(Alias = "op", Order = 1)]
    public virtual required string Op { get; init; }

    /// <summary>
    /// Gets or sets a JSON Pointer path that indicates the target location for the operation.
    /// </summary>
    [Description("A JSON Pointer path that indicates the target location for the operation.")]
    [Required, MinLength(1), RegularExpression("^(/(?:[^/~\r\n]|~[01])*)+$")]
    [DataMember(Name = "path", Order = 2), JsonPropertyName("path"), JsonPropertyOrder(2), YamlMember(Alias = "path", Order = 2)]
    public virtual required string Path { get; init; }

    /// <summary>
    /// Gets or sets the value to apply in the operation. Required for 'add', 'replace', and 'test' operations.
    /// </summary>
    [Description("The value to apply in the operation. Required for 'add', 'replace', and 'test' operations.")]
    [DataMember(Name = "value", Order = 3), JsonPropertyName("value"), JsonPropertyOrder(3), YamlMember(Alias = "value", Order = 3)]
    public virtual object? Value { get; init; }

    /// <summary>
    /// Gets or sets the source path for 'move' and 'copy' operations.
    /// </summary>
    [Description("The source path for 'move' and 'copy' operations.")]
    [DataMember(Name = "from", Order = 4), JsonPropertyName("from"), JsonPropertyOrder(4), YamlMember(Alias = "from", Order = 4)]
    public virtual string? From { get; init; }

}

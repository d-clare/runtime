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
/// Represents a binary part in a streamed message fragment.
/// </summary>
[Description("Represents a binary part in a streamed message fragment.")]
[DataContract]
public record BinaryFragmentPart
    : MessageFragmentPart
{

    /// <summary>
    /// Gets the URI that references the binary content.
    /// </summary>
    [Description("The URI that references the binary content.")]
    [DataMember(Name = "uri", Order = 3), JsonPropertyName("uri"), JsonPropertyOrder(3), YamlMember(Alias = "uri", Order = 3)]
    public virtual Uri? Uri { get; init; }

    /// <summary>
    /// Gets the raw binary data.
    /// </summary>
    [Description("The raw binary data.")]
    [DataMember(Name = "data", Order = 4), JsonPropertyName("data"), JsonPropertyOrder(4), YamlMember(Alias = "data", Order = 4)]
    public virtual byte[]? Data { get; init; }

}

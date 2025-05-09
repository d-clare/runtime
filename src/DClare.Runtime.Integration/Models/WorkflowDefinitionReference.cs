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
/// Represents a reference to a specific version of a workflow definition, identified by its namespace, name, and version.
/// </summary>
[Description("Represents a reference to a specific version of a workflow definition, identified by its namespace, name, and version.")]
[DataContract]
public record WorkflowDefinitionReference
{

    /// <summary>
    /// Gets or sets the namespace under which the workflow definition is registered.
    /// </summary>
    [Description("The namespace under which the workflow definition is registered.")]
    [Required, StringLength(DnsLabel.MaxLength, MinimumLength = DnsLabel.MinLength), RegularExpression(DnsLabel.Regex)]
    [DataMember(Name = "namespace", Order = 1), JsonPropertyName("namespace"), JsonPropertyOrder(1), YamlMember(Alias = "namespace", Order = 1)]
    public virtual required string Namespace { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow definition.
    /// </summary>
    [Description("The name of the workflow definition.")]
    [Required, StringLength(DnsLabel.MaxLength, MinimumLength = DnsLabel.MinLength), RegularExpression(DnsLabel.Regex)]
    [DataMember(Name = "name", Order = 2), JsonPropertyName("name"), JsonPropertyOrder(2), YamlMember(Alias = "name", Order = 2)]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the version of the workflow definition.
    /// </summary>
    [Description("The version of the workflow definition.")]
    [Required, RegularExpression("^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$")]
    [DataMember(Name = "version", Order = 3), JsonPropertyName("version"), JsonPropertyOrder(3), YamlMember(Alias = "version", Order = 3, ScalarStyle = ScalarStyle.SingleQuoted)]
    public virtual required string Version { get; set; }

}


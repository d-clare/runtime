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
/// Represents the specification for a workflow execution, including its definition and optional input data.
/// </summary>
[Description("Represents the specification for a workflow execution, including its definition and optional input data.")]
[DataContract]
public record WorkflowInstanceSpec
{

    /// <summary>
    /// Gets or sets the reference to the workflow definition to be executed.
    /// </summary>
    [Description("The reference to the workflow definition to be executed.")]
    [Required]
    [DataMember(Name = "definition", Order = 1), JsonPropertyName("definition"), JsonPropertyOrder(1), YamlMember(Alias = "definition", Order = 1)]
    public virtual WorkflowDefinitionReference Definition { get; set; } = null!;

    /// <summary>
    /// Gets or sets the input parameters to be passed to the workflow at execution time.
    /// </summary>
    [Description("The input parameters to be passed to the workflow at execution time.")]
    [DataMember(Name = "input", Order = 2), JsonPropertyName("input"), JsonPropertyOrder(2), YamlMember(Alias = "input", Order = 2)]
    public virtual EquatableDictionary<string, object>? Input { get; set; }

}

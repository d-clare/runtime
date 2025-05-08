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

namespace DClare.Runtime.Integration.Resources;

/// <summary>
/// Represents the current execution status of a workflow, including status information by version.
/// </summary>
[Description("Represents the current execution status of a workflow, including status information by version.")]
[DataContract]
public record WorkflowStatus
{

    /// <summary>
    /// Gets or sets the version-specific execution status for this workflow.
    /// </summary>
    [Description("The version-specific execution status for this workflow.")]
    [DataMember(Order = 1, Name = "versions"), JsonPropertyOrder(1), JsonPropertyName("versions"), YamlMember(Order = 1, Alias = "versions")]
    public virtual EquatableDictionary<string, WorkflowVersionStatus> Versions { get; set; } = [];

}

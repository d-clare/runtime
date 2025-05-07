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
/// Represents execution statistics for a specific version of a workflow.
/// </summary>
[Description("Represents execution statistics for a specific version of a workflow.")]
[DataContract]
public record WorkflowVersionStatus
{

    /// <summary>
    /// Gets or sets the total number of times this version has been executed.
    /// </summary>
    [Description("The total number of times this version has been executed.")]
    [DataMember(Name = "totalInstances", Order = 1), JsonPropertyName("totalInstances"), JsonPropertyOrder(1), YamlMember(Alias = "totalInstances", Order = 1)]
    public virtual int TotalInstances { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the most recent execution start for this version.
    /// </summary>
    [Description("The timestamp of the most recent execution start for this version.")]
    [DataMember(Name = "lastStartedAt", Order = 2), JsonPropertyName("lastStartedAt"), JsonPropertyOrder(2), YamlMember(Alias = "lastStartedAt", Order = 2)]
    public virtual DateTimeOffset? LastStartedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the most recent execution end for this version.
    /// </summary>
    [Description("The timestamp of the most recent execution end for this version.")]
    [DataMember(Name = "lastEndedAt", Order = 3), JsonPropertyName("lastEndedAt"), JsonPropertyOrder(3), YamlMember(Alias = "lastEndedAt", Order = 3)]
    public virtual DateTimeOffset? LastEndedAt { get; set; }

}

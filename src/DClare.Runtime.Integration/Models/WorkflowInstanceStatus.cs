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
/// Represents the current status of a workflow execution, including timestamps, history, and output data.
/// </summary>
[Description("Represents the current status of a workflow execution, including timestamps, history, and output data.")]
[DataContract]
public record WorkflowInstanceStatus
{

    /// <summary>
    /// Gets or sets the current phase of the workflow.
    /// </summary>
    [Description("The current phase of the workflow.")]
    [AllowedValues(WorkflowInstanceStatusPhase.Pending, WorkflowInstanceStatusPhase.Running, WorkflowInstanceStatusPhase.Suspended, WorkflowInstanceStatusPhase.Waiting, WorkflowInstanceStatusPhase.Completed, WorkflowInstanceStatusPhase.Cancelled, WorkflowInstanceStatusPhase.Faulted)]
    [DataMember(Name = "phase", Order = 1), JsonPropertyName("phase"), JsonPropertyOrder(1), YamlMember(Alias = "phase", Order = 1)]
    public virtual string Phase { get; set; } = WorkflowInstanceStatusPhase.Pending;

    /// <summary>
    /// Gets or sets the timestamp when the workflow was created.
    /// </summary>
    [Description("The timestamp when the workflow was created.")]
    [Required]
    [DataMember(Name = "createdAt", Order = 2), JsonPropertyName("createdAt"), JsonPropertyOrder(2), YamlMember(Alias = "createdAt", Order = 2)]
    public virtual DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the workflow was last updated.
    /// </summary>
    [Description("The timestamp when the workflow was last updated.")]
    [DataMember(Name = "updatedAt", Order = 3), JsonPropertyName("updatedAt"), JsonPropertyOrder(3), YamlMember(Alias = "updatedAt", Order = 3)]
    public virtual DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the workflow started executing.
    /// </summary>
    [Description("The timestamp when the workflow started executing.")]
    [DataMember(Name = "startedAt", Order = 4), JsonPropertyName("startedAt"), JsonPropertyOrder(4), YamlMember(Alias = "startedAt", Order = 4)]
    public virtual DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the workflow ended, regardless of success, failure, or cancellation.
    /// </summary>
    [Description("The timestamp when the workflow ended, regardless of success, failure, or cancellation.")]
    [DataMember(Name = "endedAt", Order = 5), JsonPropertyName("endedAt"), JsonPropertyOrder(5), YamlMember(Alias = "endedAt", Order = 5)]
    public virtual DateTimeOffset? EndedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of task instances that were executed as part of the workflow.Gets or sets the list of task instances that were executed as part of the workflow.
    /// </summary>
    [Description("The list of task instances that were executed as part of the workflow.Gets or sets the list of task instances that were executed as part of the workflow.")]
    [DataMember(Name = "tasks", Order = 6), JsonPropertyName("tasks"), JsonPropertyOrder(6), YamlMember(Alias = "tasks", Order = 6)]
    public virtual EquatableList<TaskInstance>? Tasks { get; set; }

    /// <summary>
    /// Gets or sets the current state of the workflow's data.
    /// </summary>
    [Description("The current state of the workflow's data.")]
    [DataMember(Name = "state", Order = 7), JsonPropertyName("state"), JsonPropertyOrder(7), YamlMember(Alias = "state", Order = 7)]
    public virtual object? State { get; set; }

    /// <summary>
    /// Gets or sets the final output of the workflow after execution completes.
    /// </summary>
    [Description("The final output of the workflow after execution completes.")]
    [DataMember(Name = "output", Order = 8), JsonPropertyName("output"), JsonPropertyOrder(8), YamlMember(Alias = "output", Order = 8)]
    public virtual object? Output { get; set; }

}


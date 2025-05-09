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
/// Represents the resource used to describe the instance of a task.
/// </summary>
[Description("Represents the resource used to describe the instance of a task.")]
[DataContract]
public record TaskInstance
{

    /// <summary>
    /// Gets or sets the task's identifier.
    /// </summary>
    [Description("The task's identifier.")]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = Guid.NewGuid().ToString("N")[..12];

    /// <summary>
    /// Gets or sets the name of the task, if any.
    /// </summary>
    [Description("The name of the task, if any.")]
    [DataMember(Name = "name", Order = 2), JsonPropertyName("name"), JsonPropertyOrder(2), YamlMember(Alias = "name", Order = 2)]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets a relative URI that references the task's definition.
    /// </summary>
    [Description("A relative URI that references the task's definition.")]
    [DataMember(Name = "reference", Order = 3), JsonPropertyName("reference"), JsonPropertyOrder(3), YamlMember(Alias = "reference", Order = 3)]
    public required virtual Uri Reference { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task is part of an extension.
    /// </summary>
    [Description("Indicates whether the task is part of an extension.")]
    [DataMember(Name = "isExtension", Order = 4), JsonPropertyName("isExtension"), JsonPropertyOrder(4), YamlMember(Alias = "isExtension", Order = 4)]
    public virtual bool IsExtension { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the task's parent, if any.
    /// </summary>
    [Description("The identifier of the task's parent, if any.")]
    [DataMember(Name = "parentId", Order = 5), JsonPropertyName("parentId"), JsonPropertyOrder(5), YamlMember(Alias = "parentId", Order = 5)]
    public virtual string? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the task was created.
    /// </summary>
    [Description("The date and time when the task was created.")]
    [DataMember(Name = "createdAt", Order = 6), JsonPropertyName("createdAt"), JsonPropertyOrder(6), YamlMember(Alias = "createdAt", Order = 6)]
    public virtual DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Gets or sets the date and time when the task started execution, if applicable.
    /// </summary>
    [Description("The date and time when the task started execution, if applicable.")]
    [DataMember(Name = "startedAt", Order = 7), JsonPropertyName("startedAt"), JsonPropertyOrder(7), YamlMember(Alias = "startedAt", Order = 7)]
    public virtual DateTimeOffset? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the task ended, if applicable.
    /// </summary>
    [Description("The date and time when the task ended, if applicable.")]
    [DataMember(Name = "endedAt", Order = 8), JsonPropertyName("endedAt"), JsonPropertyOrder(8), YamlMember(Alias = "endedAt", Order = 8)]
    public virtual DateTimeOffset? EndedAt { get; set; }

    /// <summary>
    /// Gets or sets the current status of the task.
    /// </summary>
    [Description("The current status of the task.")]
    [AllowedValues(TaskInstanceStatus.Pending, TaskInstanceStatus.Running, TaskInstanceStatus.Faulted, TaskInstanceStatus.Skipped, TaskInstanceStatus.Suspended, TaskInstanceStatus.Cancelled, TaskInstanceStatus.Completed)]
    [DataMember(Name = "status", Order = 9), JsonPropertyName("status"), JsonPropertyOrder(9), YamlMember(Alias = "status", Order = 9)]
    public virtual string Status { get; set; } = TaskInstanceStatus.Pending;

    /// <summary>
    /// Gets or sets the reason for the task's current status, if any.
    /// </summary>
    [Description("The reason for the task's current status, if any.")]
    [DataMember(Name = "statusReason", Order = 10), JsonPropertyName("statusReason"), JsonPropertyOrder(10), YamlMember(Alias = "statusReason", Order = 10)]
    public virtual string? StatusReason { get; set; }

    /// <summary>
    /// Gets or sets the error that occurred during the task's execution, if any.
    /// </summary>
    [Description("The error that occurred during the task's execution, if any.")]
    [DataMember(Name = "error", Order = 11), JsonPropertyName("error"), JsonPropertyOrder(11), YamlMember(Alias = "error", Order = 11)]
    public virtual Error? Error { get; set; }

    /// <summary>
    /// Gets or sets the input patch operations applied to the task at execution time.
    /// </summary>
    [Description("The input patch operations applied to the task at execution time.")]
    [DataMember(Name = "input", Order = 12), JsonPropertyName("input"), JsonPropertyOrder(12), YamlMember(Alias = "input", Order = 12)]
    public virtual EquatableList<JsonPatchOperation>? Input { get; set; }

    /// <summary>
    /// Gets or sets the context patch operations applied during the task's execution.
    /// </summary>
    [Description("The context patch operations applied during the task's execution.")]
    [DataMember(Name = "context", Order = 13), JsonPropertyName("context"), JsonPropertyOrder(13), YamlMember(Alias = "context", Order = 13)]
    public virtual EquatableList<JsonPatchOperation>? Context { get; set; }

    /// <summary>
    /// Gets or sets the output patch operations produced by the task.
    /// </summary>
    [Description("The output patch operations produced by the task.")]
    [DataMember(Name = "output", Order = 14), JsonPropertyName("output"), JsonPropertyOrder(14), YamlMember(Alias = "output", Order = 14)]
    public virtual EquatableList<JsonPatchOperation>? Output { get; set; }

    /// <summary>
    /// Gets a value indicating whether the task is in an operative state, meaning it is pending, running, or suspended.
    /// </summary>
    [Description("Indicates whether the task is in an operative state, meaning it is pending, running, or suspended.")]
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual bool IsOperative => Status == TaskInstanceStatus.Pending || Status == TaskInstanceStatus.Running || Status == TaskInstanceStatus.Suspended;
}

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
/// Represents a resource used to describe and configure a workflow.
/// </summary>
[Description("Represents a resource used to describe and configure a workflow.")]
[DataContract]
public record Workflow
    : Resource<WorkflowSpec, WorkflowStatus>
{

    /// <summary>
    /// Gets the <see cref="Workflow"/> resource definition.
    /// </summary>
    public static readonly ResourceDefinitionInfo ResourceDefinition = new WorkflowResourceDefinition()!;

    /// <inheritdoc/>
    public Workflow() : base(ResourceDefinition) { Status = new(); }

    /// <inheritdoc/>
    public Workflow(ResourceMetadata metadata, WorkflowSpec spec) : base(ResourceDefinition, metadata, spec, new()) { }

}

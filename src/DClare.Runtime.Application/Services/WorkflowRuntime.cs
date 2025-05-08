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

using DClare.Runtime.Integration.Resources;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IWorkflowRuntime"/> interface.
/// </summary>
/// <param name="logger">The service used to perform logging.</param>
/// <param name="resources">The repository used to manage the application's resources.</param>
public class WorkflowRuntime(ILogger<WorkflowRuntime> logger, IResourceRepository resources)
    : IWorkflowRuntime
{

    /// <summary>
    /// Gets the service used to perform logging.
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the repository used to manage the application's resources.
    /// </summary>
    protected IResourceRepository Resources { get; } = resources;

    /// <inheritdoc/>
    public virtual async Task<WorkflowInstance> RunAsync(string name, string @namespace, string version, IDictionary<string, object>? input = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(@namespace);
        ArgumentException.ThrowIfNullOrWhiteSpace(version);
        var resource = await Resources.GetAsync<Workflow>(name, @namespace, cancellationToken).ConfigureAwait(false) ?? throw new ProblemDetailsException(Problems.ResourceNotFound<Workflow>(name, @namespace));
        var definition = resource.Spec.Versions.FirstOrDefault(v => v.Document.Version == version) ?? throw new ProblemDetailsException(Problems.WorkflowDefinitionNotFound(name, @namespace, version));
        var instance = new WorkflowInstance()
        {
            Metadata = new()
            {
                Namespace = resource.Metadata.Namespace,
                Name = $"{resource.Metadata.Name}-"
            },
            Spec = new()
            {
                Definition = new()
                {
                    Namespace = resource.Metadata.Namespace!,
                    Name = resource.Metadata.Name!,
                    Version = definition.Document.Version
                },
                Input = input == null ? null : new(input),
            },
            Status = new()
            {
                Phase = WorkflowInstanceStatusPhase.Pending,
                CreatedAt = DateTimeOffset.Now
            }
        };
        instance = await Resources.AddAsync(instance, false, cancellationToken).ConfigureAwait(false);

        return instance;
    }

}

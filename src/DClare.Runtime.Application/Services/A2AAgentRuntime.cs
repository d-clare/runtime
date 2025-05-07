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

using A2A.Models;
using A2A.Server.Infrastructure;
using A2A.Server.Infrastructure.Services;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default DClare implementation of the <see cref="IAgentRuntime"/> interface
/// </summary>
/// <param name="name">The name of the <see cref="IAgent"/> to invoke</param>
/// <param name="definition">The definition of the <see cref="IAgent"/> to invoke</param>
/// <param name="components">A collection, if any, containing the reusable components available to the <see cref="IProcess"/></param>
/// <param name="agentFactory">The service used to create <see cref="IAgent"/>s</param>
public class A2AAgentRuntime(string name, AgentDefinition definition, IAgentFactory agentFactory)
    : IAgentRuntime
{

    /// <summary>
    /// Gets the name of the <see cref="IAgent"/> to invoke
    /// </summary>
    protected string Name { get; } = name;

    /// <summary>
    /// Gets the definition of the <see cref="IAgent"/> to invoke
    /// </summary>
    protected AgentDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets the service used to create <see cref="IAgent"/>s
    /// </summary>
    protected IAgentFactory AgentFactory { get; } = agentFactory;

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<AgentResponseContent> ExecuteAsync(TaskRecord task, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var agent = await AgentFactory.CreateAsync(Name, Definition, null, cancellationToken).ConfigureAwait(false);
        var invocationOptions = new AgentInvocationOptions()
        {
            ChatId = task.SessionId
        };
        var response = await agent.InvokeAsync(task.Message.ToText()!, invocationOptions, cancellationToken).ConfigureAwait(false);
        var messages = response.Messages.ToList();
        for (int i = 0; i < messages.Count; i++)
        {
            var message = messages[i];
            var last = i == messages.Count - 1;
            yield return new(new Artifact()
            {
                Index = (uint)i,
                Parts =
                [
                    new TextPart(message.Content!)
                ],
                Append = !last,
                LastChunk = last,
                Metadata = message.Metadata == null ? null : new(message.Metadata!)
            });
        }
    }

    /// <inheritdoc/>
    public virtual System.Threading.Tasks.Task CancelAsync(string taskId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        //todo: implement
        throw new NotImplementedException();
    }

}

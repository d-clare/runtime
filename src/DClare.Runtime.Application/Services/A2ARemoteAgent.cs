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

using A2A;
using A2A.Client.Services;
using A2A.Events;
using A2A.Models;
using A2A.Requests;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents a remote <see cref="IAgent"/> implementation that is invoked through an external communication channel
/// </summary>
/// <param name="name">The agent's name</param>
/// <param name="definition">The agent's definition</param>
/// <param name="manifest">The agent's manifest</param>
/// <param name="client">The service used to interact with the remote agent using the A2A protocol</param>
public class A2ARemoteAgent(string name, RemoteAgentDefinition definition, AgentCard manifest, IA2AProtocolClient client)
    : IAgent
{

    /// <inheritdoc/>
    public string Name { get; } = name;

    /// <inheritdoc/>
    public string? Description => Manifest.Description;

    /// <inheritdoc/>
    public IReadOnlyCollection<AgentSkillDefinition> Skills { get; } = [.. manifest.Skills.Select(s => new AgentSkillDefinition() 
    { 
        Name = s.Name,
        Description = s.Description,
    })];

    /// <summary>
    /// Gets the agent's manifest
    /// </summary>
    protected AgentCard Manifest { get; } = manifest;

    /// <summary>
    /// Gets the agent's definition
    /// </summary>
    protected RemoteAgentDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets the service used to interact with the remote agent using the A2A protocol
    /// </summary>
    protected IA2AProtocolClient Client { get; } = client;

    /// <inheritdoc/>
    public virtual async Task<ChatResponse> InvokeAsync(string message, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        var stream = await InvokeStreamingAsync(message, sessionId, cancellationToken).ConfigureAwait(false);
        return await stream.ToResponseAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<ChatResponseStream> InvokeStreamingAsync(string message, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        var requestParameters = new TaskSendParameters()
        {
            SessionId = sessionId,
            Message = new()
            {
                Role = MessageRole.User,
                Parts =
                [
                    new TextPart(message)
                ]
            }
        };
        var responseId = Guid.NewGuid().ToString("N");
        IAsyncEnumerable<Integration.Models.StreamingChatMessageContent> stream;
        if (Manifest.Capabilities.Streaming)
        {
            var request = new SendTaskStreamingRequest()
            {
                Id = responseId,
                Params = requestParameters
            };
            stream = Client.SendTaskStreamingAsync(request, cancellationToken)
                .TakeWhile(e => e.Result is not TaskStatusUpdateEvent status || !status.Final)
                .SelectMany(e =>
                {
                    if (e.Result is TaskArtifactUpdateEvent artifactUpdate) return artifactUpdate.Artifact.ToStreamingChatMessageContents().ToAsyncEnumerable();
                    else return Enumerable.Empty<Integration.Models.StreamingChatMessageContent>().ToAsyncEnumerable();
                });
        }
        else
        {
            var request = new SendTaskRequest()
            {
                Id = responseId,
                Params = requestParameters
            };
            var response = await Client.SendTaskAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.Error != null) throw new ProblemDetailsException(Problems.AgentCommunicationError(Name, $"[{response.Error.Code}] {response.Error.Message}"));
            if (response.Result == null) throw new ProblemDetailsException(Problems.AgentCommunicationError(Name, $"No result was returned by the remote agent"));
            var contents = response.Result.Artifacts?.SelectMany(a => a.ToStreamingChatMessageContents()) ?? [];
            stream = contents.ToAsyncEnumerable();
        }
        return new(responseId, stream);
    }

}

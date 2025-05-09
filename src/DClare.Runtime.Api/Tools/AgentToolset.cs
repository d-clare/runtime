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

using DClare.Runtime.Integration.Commands.Agents;
using DClare.Runtime.Integration.Queries.Resources;
using ModelContextProtocol.Server;

namespace DClare.Runtime.Api.Tools;

/// <summary>
/// Represents the MCP toolset used to manage <see cref="Agent"/>s.
/// </summary>
/// <param name="mediator">The service used to mediate calls.</param>
[Description("Represents the MCP toolset used to manage agents.")]
[McpServerToolType]
public sealed class AgentToolset(IMediator mediator)
{

    /// <summary>
    /// Lists agents.
    /// </summary>
    /// <param name="namespace">The optional namespace the agents to list belong to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new list containing the resulting agents.</returns>
    [Description("Lists agents.")]
    [McpServerTool(Name = "ListAgents", Title = "List Agents")]
    public async Task<List<Agent>> ListAgentsAsync(
        [Description("The optional namespace the agents to list belong to.")]string? @namespace = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.ExecuteAsync(new GetResourcesQuery<Agent>(@namespace), cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) throw new Exception(); //todo: improve
        else if(result.Data == null) throw new Exception(); //todo: improve
        return await result.Data.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the specified agent.
    /// </summary>
    /// <param name="namespace">The namespace the agent to get belongs to.</param>
    /// <param name="name">The name of the agent to get.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The specified agent.</returns>
    [Description("Gets the specified agent.")]
    [McpServerTool(Name = "GetAgent", Title = "Get Agent")]
    public async Task<Agent> GetAgentAsync(
        [Description("The namespace the agent to get belongs to.")] string @namespace,
        [Description("The name of the agent to get.")] string name,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.ExecuteAsync(new GetResourceQuery<Agent>(name, @namespace), cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) throw new Exception(); //todo: improve
        else if (result.Data == null) throw new Exception(); //todo: improve
        return result.Data;
    }

    /// <summary>
    /// Invokes the specified agent.
    /// </summary>
    /// <param name="namespace">The namespace the agent to invoke belongs to.</param>
    /// <param name="name">The name of the agent to invoke.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    [Description("Invokes the specified agent.")]
    [McpServerTool(Name = "InvokeAgent", Title = "Invoke Agent")]
    public async Task<ChatResponse> InvokeAgentAsync(
        [Description("The namespace the agent to invoke belongs to.")] string @namespace,
        [Description("The name of the agent to invoke.")] string name,
        [Description("The input message to process.")]string message,
        [Description("The session identifier to scope the invocation to a broader user interaction context.")] string? sessionId = null,
        [Description("The unique identifier of the chat thread used to retrieve or continue a conversation context.")] string? chatId = null,
        [Description("The user identifier associated with the chat session.")] string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.ExecuteAsync(new InvokeAgentCommand()
        {
            Agent = new()
            {
                Namespace = @namespace,
                Name = name
            },
            Parameters = new()
            {
                Message = message,
                Options = new()
                {
                    SessionId = sessionId,
                    ChatId = chatId,
                    UserId = userId
                }
            }
        }, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) throw new Exception(); //todo: improve
        else if (result.Data == null) throw new Exception(); //todo: improve
        return await result.Data.ToResponseAsync(false, cancellationToken).ConfigureAwait(false);
    }

}

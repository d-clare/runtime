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

namespace DClare.Runtime.Application.Commands.Agents;

/// <summary>
/// Represents the service used to handle <see cref="InvokeAgentCommand"/>s.
/// </summary>
/// <param name="logger">The service used to perform logging.</param>
/// <param name="componentDefinitionResolver">The service used to resolve <see cref="ReferenceableComponentDefinition"/>s.</param>
/// <param name="agentFactory">The service used to create <see cref="IAgent"/>s.</param>
/// <param name="userAccessor">The service used to access the current user.</param>
public class InvokeAgentCommandHandler(ILogger<InvokeAgentCommandHandler> logger, IComponentDefinitionResolver componentDefinitionResolver, IAgentFactory agentFactory, IUserAccessor userAccessor)
    : ICommandHandler<InvokeAgentCommand, ChatResponseStream>
{

    /// <inheritdoc/>
    public async Task<IOperationResult<ChatResponseStream>> HandleAsync(InvokeAgentCommand command, CancellationToken cancellationToken = default)
    {
        var agentDefinition = await componentDefinitionResolver.ResolveAsync<AgentDefinition>(command.Agent.GetQualifiedName(), null, cancellationToken).ConfigureAwait(false);
        var agent = await agentFactory.CreateAsync(command.Agent.Name, agentDefinition, null, cancellationToken).ConfigureAwait(false);
        var userId = userAccessor.User?.FindFirst(JwtClaimTypes.Subject)?.Value;
        var invocationOptions = command.Parameters.Options ?? new();
        invocationOptions = invocationOptions with
        {
            UserId = userId
        };
        if (string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(invocationOptions.ChatId))
        {
            logger.LogWarning("Chat ID '{ChatId}' was ignored because the user ID could not be resolved — messages cannot be correlated to a specific chat without a user context.", invocationOptions.ChatId);
            invocationOptions = invocationOptions with
            {
                ChatId = null
            };
        }
        var response = await agent.InvokeStreamingAsync(command.Parameters.Message, invocationOptions, cancellationToken).ConfigureAwait(false);
        return this.Ok(response);
    }

}

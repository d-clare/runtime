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
/// Represents the service used to handle <see cref="InvokeAgentCommand"/>s
/// </summary>
/// <param name="manifestHandler">The service used to handle the application's manifest</param>
/// <param name="agentFactory">The service used to create <see cref="IAgent"/>s</param>
public class InvokeAgentCommandHandler(IManifestHandler manifestHandler, IAgentFactory agentFactory)
    : ICommandHandler<InvokeAgentCommand, ChatResponseStream>
{

    /// <inheritdoc/>
    public async Task<IOperationResult<ChatResponseStream>> HandleAsync(InvokeAgentCommand command, CancellationToken cancellationToken = default)
    {
        var manifest = await manifestHandler.GetManifestAsync(cancellationToken).ConfigureAwait(false);
        if (manifest.Interfaces == null || manifest.Interfaces.Agents == null || !manifest.Interfaces.Agents!.TryGetValue(command.Agent, out var agentDefinition) || agentDefinition == null) throw new ProblemDetailsException(Problems.AgentNotFound(command.Agent));
        var agent = await agentFactory.CreateAsync(command.Agent, agentDefinition, manifest.Components, cancellationToken).ConfigureAwait(false);
        var response = await agent.InvokeStreamingAsync(command.Message, command.SessionId, command.Parameters, cancellationToken).ConfigureAwait(false);
        return this.Ok(response);
    }

}

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

using DClare.Sdk.Models.Processes;
using Json.Schema;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents a convergence implementation of the <see cref="IProcess"/> interface
/// </summary>
/// <param name="definition">The <see cref="IProcess"/>'s definition</param>
/// <param name="components">A a collection, if any, containing the reusable components available to the <see cref="IProcess"/></param>
/// <param name="logger">The service used to perform logging</param>
/// <param name="agentFactory">The service used to create <see cref="IAgent"/>s</param>
/// <param name="kernelFunctionStrategyFactory">The service used to create <see cref="IKernelFunctionStrategy"/> instances</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
public class ConvergenceProcess(ConvergenceAgenticProcessDefinition definition, ComponentCollectionDefinition? components, ILogger<ConvergenceProcess> logger, IAgentFactory agentFactory, IKernelFunctionStrategyFactory kernelFunctionStrategyFactory, IJsonSerializer jsonSerializer)
    : IProcess
{

    /// <summary>
    /// Gets the <see cref="IProcess"/>'s definition
    /// </summary>
    protected ConvergenceAgenticProcessDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets a collection, if any, containing the reusable components available to the <see cref="IProcess"/>
    /// </summary>
    protected ComponentCollectionDefinition? Components { get; } = components;

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the service used to create <see cref="IAgent"/>s
    /// </summary>
    protected IAgentFactory AgentFactory { get; } = agentFactory;

    /// <summary>
    /// Gets the service used to create <see cref="IKernelFunctionStrategy"/> instances
    /// </summary>
    protected IKernelFunctionStrategyFactory KernelFunctionStrategyFactory { get; } = kernelFunctionStrategyFactory;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <inheritdoc/>
    public virtual async Task<ChatResponse> InvokeAsync(string prompt, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);
        var stream = await InvokeStreamingAsync(prompt, sessionId, cancellationToken).ConfigureAwait(false);
        return await stream.ToResponseAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<ChatResponseStream> InvokeStreamingAsync(string prompt, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);
        var agents = await Definition.Agents.ToAsyncEnumerable().SelectAwait(async a => await AgentFactory.CreateAsync(a.Key, a.Value, Components, cancellationToken).ConfigureAwait(false)).ToListAsync(cancellationToken).ConfigureAwait(false);
        var decompositionStrategy = Definition.Strategy.Decomposition == null ? null : await KernelFunctionStrategyFactory.CreateAsync(Definition.Strategy.Decomposition, Components, cancellationToken).ConfigureAwait(false);
        IDictionary<string, object?> strategyArguments;
        IDictionary<string, string> agentSubPrompts;
        if (Definition.Strategy.Decomposition != null && decompositionStrategy != null)
        {
            var agentsVariable = string.Join(Environment.NewLine, agents.Select(a => $"- {a.Name}: {a.Description ?? "General-purpose agent available for generic tasks"}"));
            strategyArguments = new Dictionary<string, object?>()
            {
                { Definition.Strategy.Decomposition.PromptVariableName, prompt },
                { Definition.Strategy.Decomposition.AgentsVariableName, agentsVariable }
            };
            var messages = await decompositionStrategy.InvokeAsync(strategyArguments, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
            var json = string.Concat(messages.Select(m => m.Content).Where(c => !string.IsNullOrWhiteSpace(c)));
            try
            {
                agentSubPrompts = JsonSerializer.Deserialize<IDictionary<string, string>>(json)!;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("The decomposition function must return a valid, unformatted JSON object where each key is an agent name and each value is a tailored sub-prompt");
            }
        }
        else
        {
            agentSubPrompts = agents.ToDictionary(a => a.Name, a => prompt);
        }
        var agentSubPromptTasks = new List<Task<AgentResponse>>(agentSubPrompts.Count);
        foreach (var agentSubPrompt in agentSubPrompts)
        {
            var agent = agents.FirstOrDefault(a => a.Name == agentSubPrompt.Key);
            if (agent == null) continue;
            agentSubPromptTasks.Add(InvokeAgentAsync(agent, agentSubPrompt.Value, sessionId, cancellationToken));
        }
        var agentSubPromptResponses = await Task.WhenAll(agentSubPromptTasks).ConfigureAwait(false);
        IAsyncEnumerable<Integration.Models.StreamingChatMessageContent> stream;
        if (Definition.Strategy.Synthesis == null)
        {
            stream = agentSubPromptResponses
                .SelectMany(response =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var messages = new List<Integration.Models.StreamingChatMessageContent>();
                    if (!response.IsSuccessStatusCode)
                    {
                        messages.Add(new Integration.Models.StreamingChatMessageContent($"⚠️ Agent '{response.AgentName}' failed: {response.Exception ?? "Unknown error"}",AuthorRole.System.Label, new Dictionary<string, object?>
                        {
                            ["agent"] = response.AgentName,
                            ["statusCode"] = response.StatusCode
                        }));
                        return messages;
                    }
                    if (response.Response == null || response.Response.Messages == null || !response.Response.Messages.Any()) return messages;
                    messages.Add(new Integration.Models.StreamingChatMessageContent($"🤖 Response from agent '{response.AgentName}':",AuthorRole.System.Label, new Dictionary<string, object?> { ["agent"] = response.AgentName }));
                    messages.AddRange(response.Response.Messages.Select(message =>
                    {
                        var metadata = message.Metadata ?? new Dictionary<string, object?>();
                        metadata["agent"] = response.AgentName;
                        return new Integration.Models.StreamingChatMessageContent(message.Content, message.Role, metadata);
                    }));
                    return messages;
                })
                .ToAsyncEnumerable();
        }
        else
        {
            var synthesisStrategy = await KernelFunctionStrategyFactory.CreateAsync(Definition.Strategy.Synthesis, Components, cancellationToken).ConfigureAwait(false);
            var agentSubPromptsVariable = string.Join(Environment.NewLine, agentSubPromptResponses.Where(r => r.IsSuccessStatusCode).Select(r => $"- {r.AgentName}: {(r.Response == null ? null : string.Concat(r.Response.Messages.Select(m => m.Content).Where(c => !string.IsNullOrWhiteSpace(c))))}"));
            strategyArguments = new Dictionary<string, object?>()
            {
                { Definition.Strategy.Synthesis.InputsVariableName, agentSubPromptsVariable }
            };
            stream = synthesisStrategy.InvokeStreamingAsync(strategyArguments, cancellationToken);
        }
        return new(Guid.NewGuid().ToString("N"), stream);
    }

    /// <summary>
    /// Invokes the specified <see cref="IAgent"/>
    /// </summary>
    /// <param name="agent">The <see cref="IAgent"/> to invoke</param>
    /// <param name="prompt">The prompt to invoke the <see cref="IAgent"/> with</param>
    /// <param name="sessionId">An optional session identifier to preserve state across invocations</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="AgentResponse"/></returns>
    protected virtual async Task<AgentResponse> InvokeAgentAsync(IAgent agent, string prompt, string? sessionId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(agent);
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);
        try
        {
            var response = await agent.InvokeAsync(prompt, sessionId, cancellationToken).ConfigureAwait(false);
            return new(agent.Name, (int)HttpStatusCode.OK, response);
        }
        catch (Exception ex)
        {
            Logger.LogError("An error occurred while prompting the AI Agent '{name}': {ex}", agent.Name, ex);
            return new(agent.Name, (int)HttpStatusCode.InternalServerError, Exception: ex.ToString());
        }
    }

}

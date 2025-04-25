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

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents a fully defined, locally hosted <see cref="IAgent"/> implementation with in-process execution
/// </summary>
/// <param name="name">The agent's name</param>
/// <param name="definition">The agent's definition</param>
/// <param name="kernel">The agent's kernel</param>
/// <param name="chatHistoryManager">The service used to manage <see cref="ChatHistory"/> instances</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
public class HostedAgent(string name, HostedAgentDefinition definition, Kernel kernel, IChatHistoryManager chatHistoryManager, IJsonSerializer jsonSerializer)
    : IAgent
{

    /// <inheritdoc/>
    public string Name { get; } = name;

    /// <inheritdoc/>
    public string? Description => Definition.Description;

    /// <inheritdoc/>
    public IReadOnlyCollection<AgentSkillDefinition> Skills { get; } = definition.Skills?.ToList() ?? [];

    /// <summary>
    /// Gets the agent's definition
    /// </summary>
    protected HostedAgentDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets the agent's kernel
    /// </summary>
    protected Kernel Kernel { get; } = kernel;

    /// <summary>
    /// Gets the service used for chat completion, if any
    /// </summary>
    protected IChatCompletionService? ChatCompletionService { get; } = kernel.GetAllServices<IChatCompletionService>().LastOrDefault();

    /// <summary>
    /// Gets the service used to manage <see cref="ChatHistory"/> instances
    /// </summary>
    protected IChatHistoryManager ChatHistoryManager { get; } = chatHistoryManager;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

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
        if (ChatCompletionService == null) throw new NotSupportedException($"The agent '{Name}' does not define reasoning capability");
        var chatHistory = string.IsNullOrWhiteSpace(sessionId) ? null : await ChatHistoryManager.GetChatHistoryAsync(Name, sessionId, cancellationToken).ConfigureAwait(false);
        chatHistory ??= string.IsNullOrWhiteSpace(Definition.Instructions) ? new() : new(Definition.Instructions);
        var responseId = Guid.NewGuid().ToString("N");
        var stream = StreamResponseAsync(message, chatHistory, sessionId, cancellationToken);
        return new ChatResponseStream(responseId, stream);
    }

    /// <summary>
    /// Streams the response of the agent by sending the user's message and existing chat history to the configured chat completion service
    /// </summary>
    /// <param name="userMessage">The user's input message to send to the agent</param>
    /// <param name="chatHistory">The chat history to include in the prompt and update with the final assistant message</param>
    /// <param name="sessionId">The id of the session, if any, in the context of which to invoke the agent</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>An asynchronous stream of <see cref="Integration.Models.StreamingChatMessageContent"/> values representing the streamed response</returns>
    protected virtual async IAsyncEnumerable<Integration.Models.StreamingChatMessageContent> StreamResponseAsync(string userMessage, ChatHistory chatHistory, string? sessionId = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userMessage);
        ArgumentNullException.ThrowIfNull(chatHistory);
        if (ChatCompletionService == null) throw new NotSupportedException($"The agent '{Name}' does not define reasoning capability");
        // todo: await AddMemoryContextAsync(userMessage, chatHistory, cancellationToken).ConfigureAwait(false);
        chatHistory.AddUserMessage(userMessage);
        var answerBuilder = new StringBuilder();
        var promptSettings = Kernel.Services.GetRequiredService<PromptExecutionSettings>();
        await foreach (var message in ChatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, promptSettings, Kernel, cancellationToken).ConfigureAwait(false))
        {
            answerBuilder.Append(message.Content);
            var chatMessage = new Integration.Models.StreamingChatMessageContent(message.Content, message.Role?.Label, message.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            yield return chatMessage;
        }
        var answer = answerBuilder.ToString();
        chatHistory.AddAssistantMessage(answer);
        if (!string.IsNullOrWhiteSpace(sessionId)) await ChatHistoryManager.SetChatHistoryAsync(Name, sessionId, chatHistory, cancellationToken).ConfigureAwait(false);
    }

    protected virtual Task AddMemoryContextAsync(string userMessage, ChatHistory chatHistory, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userMessage);
        ArgumentNullException.ThrowIfNull(chatHistory);
        throw new NotImplementedException(); //todo
    }

}

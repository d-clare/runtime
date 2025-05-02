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

using System.Linq;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents a fully defined, locally hosted <see cref="IAgent"/> implementation with in-process execution
/// </summary>
/// <param name="name">The agent's name</param>
/// <param name="definition">The agent's definition</param>
/// <param name="kernel">The agent's kernel</param>
/// <param name="chatManager">The service used to manage <see cref="Chat"/>s</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
public class Agent(string name, HostedAgentDefinition definition, Kernel kernel, IChatManager chatManager, IJsonSerializer jsonSerializer)
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
    /// Gets the service used to manage <see cref="Chat"/>s
    /// </summary>
    protected IChatManager ChatManager { get; } = chatManager;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <inheritdoc/>
    public virtual async Task<ChatResponse> InvokeAsync(string message, AgentInvocationOptions? options = null, CancellationToken cancellationToken = default)
    {
        var stream = await InvokeStreamingAsync(message, options, cancellationToken).ConfigureAwait(false);
        return await stream.ToResponseAsync(true, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<ChatResponseStream> InvokeStreamingAsync(string message, AgentInvocationOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        if (ChatCompletionService == null) throw new NotSupportedException($"The agent '{Name}' does not define reasoning capability");
        var chat = string.IsNullOrWhiteSpace(options?.ChatId) ? null : await ChatManager.GetAsync(options.ChatId, options.UserId, Name, cancellationToken).ConfigureAwait(false);
        var responseId = Guid.NewGuid().ToString("N");
        var stream = StreamResponseAsync(message, chat, options, cancellationToken);
        return new ChatResponseStream(responseId, stream);
    }

    /// <summary>
    /// Streams the response of the agent by sending the user's message and existing chat history to the configured chat completion service
    /// </summary>
    /// <param name="userMessage">The user's input message to send to the agent</param>
    /// <param name="chat">The current chat, if any</param>
    /// <param name="options">The options used to configure the agent's invocation</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>An asynchronous stream of <see cref="Integration.Models.StreamingChatMessageContent"/> values representing the streamed response</returns>
    protected virtual async IAsyncEnumerable<Integration.Models.StreamingChatMessageContent> StreamResponseAsync(string userMessage, Chat? chat, AgentInvocationOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userMessage);
        if (ChatCompletionService == null) throw new NotSupportedException($"The agent '{Name}' does not define reasoning capability");
        var chatHistory = chat == null ? null : new ChatHistory(chat.Messages.Select(m => new ChatMessageContent(new(m.Role), m.Content, metadata: m.Metadata)));
        chatHistory ??= string.IsNullOrWhiteSpace(Definition.Instructions) ? new() : new(Definition.Instructions);
        // todo: await AddMemoryContextAsync(userMessage, chatHistory, cancellationToken).ConfigureAwait(false);
        chatHistory.AddUserMessage(userMessage);
        var answerBuilder = new StringBuilder();
        var promptSettings = Kernel.Services.GetRequiredService<PromptExecutionSettings>();
        if (options?.Parameters != null)
        {
            promptSettings.ExtensionData ??= new Dictionary<string, object>();
            foreach (var parameter in options.Parameters) promptSettings.ExtensionData[parameter.Key] = parameter.Value;
        }
        await foreach (var message in ChatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, promptSettings, Kernel, cancellationToken).ConfigureAwait(false))
        {
            answerBuilder.Append(message.Content);
            var chatMessage = new Integration.Models.StreamingChatMessageContent(message.Content, message.Role?.Label, message.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            yield return chatMessage;
        }
        var answer = answerBuilder.ToString();
        chatHistory.AddAssistantMessage(answer);
        if (!string.IsNullOrWhiteSpace(options?.ChatId))
        {
            var messages = chatHistory.Select(m => new ChatMessage(m.Role == AuthorRole.Tool ? AuthorRole.Assistant.Label : m.Role.Label, m.Content, m.Metadata));
            chat ??= new(options.ChatId, options.UserId!, Name, null, messages);
            chat.Messages = messages;
            await ChatManager.AddOrUpdateAsync(chat, cancellationToken).ConfigureAwait(false);
        }
    }

    protected virtual Task AddMemoryContextAsync(string userMessage, ChatHistory chatHistory, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userMessage);
        ArgumentNullException.ThrowIfNull(chatHistory);
        throw new NotImplementedException(); //todo
    }

}

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

using Microsoft.Extensions.Caching.Distributed;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IChatHistoryManager"/> interface
/// </summary>
/// <param name="cache">The service used to cache agent <see cref="ChatHistory"/> instances</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
public class ChatHistoryManager(IDistributedCache cache, IJsonSerializer jsonSerializer)
    : IChatHistoryManager
{

    /// <summary>
    /// Gets the service used to cache agent <see cref="ChatHistory"/> instances
    /// </summary>
    protected IDistributedCache Cache { get; } = cache;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <inheritdoc/>
    public virtual Task SetChatHistoryAsync(string agentName, string sessionId, ChatHistory chatHistory, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentName);
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
        ArgumentNullException.ThrowIfNull(chatHistory);
        var key = BuildAgentSessionKey(agentName, sessionId);
        var messages = chatHistory.Select(m => new ChatMessage(m.Role.Label, m.Content));
        var json = JsonSerializer.SerializeToText(messages);
        return Cache.SetStringAsync(key, json, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<ChatHistory?> GetChatHistoryAsync(string agentName, string sessionId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentName);
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
        var key = BuildAgentSessionKey(agentName, sessionId);
        var json = await Cache.GetStringAsync(key, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(json)) return null;
        var messages = (JsonSerializer.Deserialize<List<ChatMessage>>(json))!;
        return [.. messages.Select(m => new ChatMessageContent(new(m.Role), m.Content))];
    }


    /// <summary>
    /// Builds a new cache key for the specified agent name and session id
    /// </summary>
    /// <param name="agentName">The name of the agent to build a new session cache key for</param>
    /// <param name="sessionId">The id of the session to build a new cache key for</param>
    /// <returns>A new agent session cache key</returns>
    protected virtual string BuildAgentSessionKey(string agentName, string sessionId) => $"session:{agentName}:{sessionId}";

}

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

namespace DClare.Runtime.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IChatSessionStore"/> interface.
/// </summary>
/// <param name="cache">The service used to cache <see cref="ChatSession"/>s.</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON.</param>
public class DistributedCacheChatSessionStore(IDistributedCache cache, IJsonSerializer jsonSerializer)
    : IChatSessionStore
{

    /// <summary>
    /// Gets the service used to cache <see cref="ChatSession"/>s.
    /// </summary>
    protected IDistributedCache Cache { get; } = cache;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON.
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <inheritdoc/>
    public virtual async Task AddOrUpdateAsync(ChatSession chat, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chat);
        var chatCacheKey = BuildChatCacheKey(chat.Key);
        var json = JsonSerializer.SerializeToText(chat);
        await Cache.SetStringAsync(chatCacheKey, json, cancellationToken).ConfigureAwait(false);
        var indexKey = BuildUserChatIndexCacheKey(chat.UserId);
        json = await Cache.GetStringAsync(indexKey, cancellationToken).ConfigureAwait(false);
        var index = string.IsNullOrWhiteSpace(json) ? [] : JsonSerializer.Deserialize<List<string>>(json)!;
        index.Add(chat.Key);
        json = JsonSerializer.SerializeToText(index);
        await Cache.SetStringAsync(indexKey, json, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<ChatSession?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var cacheKey = BuildChatCacheKey(key);
        var json = await Cache.GetStringAsync(cacheKey, cancellationToken).ConfigureAwait(false);
        return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<ChatSession>(json);
    }

    /// <inheritdoc/>
    public virtual Task<ChatSession?> GetAsync(string id, string userId, string agentName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(agentName);
        return GetAsync(ChatSession.BuildKey(id, userId, agentName), cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<ChatSession> ListAsync(string userId, string? agentName = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        var indexKey = BuildUserChatIndexCacheKey(userId);
        var json = await Cache.GetStringAsync(indexKey, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(json)) yield break;
        var keys = JsonSerializer.Deserialize<List<string>>(json)!;
        foreach (var key in keys)
        {
            var chat = await GetAsync(key, cancellationToken).ConfigureAwait(false);
            if (chat is not null && (string.IsNullOrWhiteSpace(agentName) || chat.AgentName == agentName)) yield return chat;
        }
    }

    /// <inheritdoc/>
    public virtual async Task RenameAsync(string key, string? name, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var cacheKey = BuildChatCacheKey(key);
        var json = await Cache.GetStringAsync(cacheKey, cancellationToken).ConfigureAwait(false);
        var chat = string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<ChatSession>(json);
        if (chat == null) return;
        chat.Name = name;
        json = JsonSerializer.SerializeToText(chat);
        await Cache.SetStringAsync(cacheKey, json, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var chat = await GetAsync(key, cancellationToken).ConfigureAwait(false);
        if (chat is null) return;
        var cacheKey = BuildChatCacheKey(key);
        await Cache.RemoveAsync(cacheKey, cancellationToken).ConfigureAwait(false);
        var indexKey = BuildUserChatIndexCacheKey(chat.UserId);
        var json = await Cache.GetStringAsync(indexKey, cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(json))
        {
            var ids = JsonSerializer.Deserialize<List<string>>(json)!;
            ids.Remove(key);
            json = JsonSerializer.SerializeToText(ids);
            await Cache.SetStringAsync(indexKey, json, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Builds the cache key used to store a chat by ID.
    /// </summary>
    protected virtual string BuildChatCacheKey(string key) => $"chat:{key}";

    /// <summary>
    /// Builds the key used to index chats for a given user
    /// </summary>
    /// <param name="userId">The id of the user to build the chat index key belongs to</param>
    protected virtual string BuildUserChatIndexCacheKey(string userId) => $"user:{userId}:chats";

}

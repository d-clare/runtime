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
/// Defines the fundamentals of a service used to manage <see cref="ChatSession"/>s
/// </summary>
public interface IChatSessionStore
{

    /// <summary>
    /// Adds or updated the specified chat
    /// </summary>
    /// <param name="chat">The chat to add or update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task AddOrUpdateAsync(ChatSession chat, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the specified chat
    /// </summary>
    /// <param name="key">The key of the chat to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The matching <see cref="ChatHistory"/>, if any</returns>
    Task<ChatSession?> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the specified chat
    /// </summary>
    /// <param name="id">The chat's user-defined identifier</param>
    /// <param name="userId">The id of the user the chat to get belongs to</param>
    /// <param name="agentName">The name of the agent the chat to get concerns</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The matching <see cref="ChatHistory"/>, if any</returns>
    Task<ChatSession?> GetAsync(string id, string userId, string agentName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all chats for the specified user, and optionally agent
    /// </summary>
    /// <param name="userId">The id of the user to list the chats for</param>
    /// <param name="agentName">The name of the agent, if any, to retrieve the specified user's chats</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> used to asynchronously enumerate the user's chats</returns>
    IAsyncEnumerable<ChatSession> ListAsync(string userId, string? agentName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Renames the specified chat
    /// </summary>
    /// <param name="key">The key of the chat to rename</param>
    /// <param name="name">The chat's name</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task RenameAsync(string key, string? name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified chat
    /// </summary>
    /// <param name="key">The key of the chat to delete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);

}
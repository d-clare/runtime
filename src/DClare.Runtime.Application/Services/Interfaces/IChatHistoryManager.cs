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
/// Defines the fundamentals of a service used to manage <see cref="ChatHistory"/> instances
/// </summary>
public interface IChatHistoryManager
{

    /// <summary>
    /// Sets the <see cref="ChatHistory"/> for the specified session
    /// </summary>
    /// <param name="agentName">The name of the agent to set the <see cref="ChatHistory"/> for</param>
    /// <param name="sessionId">The id of the session to set the <see cref="ChatHistory"/> for</param>
    /// <param name="chatHistory">The <see cref="ChatHistory"/> to set</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task SetChatHistoryAsync(string agentName, string sessionId, ChatHistory chatHistory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the <see cref="ChatHistory"/> for the specified session
    /// </summary>
    /// <param name="agentName">The name of the agent to get the <see cref="ChatHistory"/> for</param>
    /// <param name="sessionId">The id of the session to get the <see cref="ChatHistory"/> for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The <see cref="ChatHistory"/> for the specified session, if any</returns>
    Task<ChatHistory?> GetChatHistoryAsync(string agentName, string sessionId, CancellationToken cancellationToken = default);

}

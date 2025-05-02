// Copyright � 2025-Present The DClare Authors
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

using DClare.Runtime.Integration.Queries.Chats;

namespace DClare.Runtime.Application.Queries.Chats;

/// <summary>
/// Represents the service used to handle <see cref="ListChatsQuery"/> instances
/// </summary>
/// <param name="httpContextAccessor">The service used to access the current <see cref="HttpContext"/></param>
/// <param name="chatManager">The service used to manage <see cref="Chat"/>s</param>
public class ListChatsQueryHandler(IHttpContextAccessor httpContextAccessor, IChatManager chatManager)
    : IQueryHandler<ListChatsQuery, IAsyncEnumerable<Chat>>
{

    /// <inheritdoc/>
    public virtual Task<IOperationResult<IAsyncEnumerable<Chat>>> HandleAsync(ListChatsQuery query, CancellationToken cancellationToken = default)
    {
        if (httpContextAccessor.HttpContext.User.Identity == null || !httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) throw new ProblemDetailsException(Problems.Unauthorized());
        var userId = httpContextAccessor.HttpContext.User.FindFirst(JwtClaimTypes.Subject)?.Value;
        if (string.IsNullOrWhiteSpace(userId)) throw new ProblemDetailsException(Problems.Forbidden());
        return Task.FromResult(this.Ok(chatManager.ListAsync(userId, query.Agent, cancellationToken)));
    }

}

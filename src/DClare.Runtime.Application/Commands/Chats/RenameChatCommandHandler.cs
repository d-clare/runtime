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

using DClare.Runtime.Integration.Commands.Chats;

namespace DClare.Runtime.Application.Commands.Chats;

/// <summary>
/// Represents the service used to handle <see cref="RenameChatCommand"/>s
/// </summary>
/// <param name="chatManager">The service used to manage <see cref="Chat"/>s</param>
public class RenameChatCommandHandler(IChatManager chatManager)
    : ICommandHandler<RenameChatCommand>
{

    /// <inheritdoc/>
    public async Task<IOperationResult> HandleAsync(RenameChatCommand command, CancellationToken cancellationToken = default)
    {
        await chatManager.RenameAsync(command.Key, command.Name, cancellationToken).ConfigureAwait(false);
        return this.Ok();
    }

}


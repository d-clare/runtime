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

using Microsoft.SemanticKernel.ChatCompletion;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="ChatSession"/>s
/// </summary>
public static class ChatSessionExtensions
{

    /// <summary>
    /// Converts the <see cref="ChatSession"/> into a new <see cref="ChatHistory"/>.
    /// </summary>
    /// <param name="chat">The <see cref="ChatHistory"/> to convert.</param>
    /// <returns>A new <see cref="ChatHistory"/>.</returns>
    public static ChatHistory ToChatHistory(this ChatSession chat)
    {
        var chatHistory = new ChatHistory();
        foreach (var message in chat.Messages) chatHistory.AddMessage(new AuthorRole(message.Role), message.Parts.ToChatMessageContentItemCollection(), message.Encoding, message.Metadata);
        return chatHistory;
    }

}

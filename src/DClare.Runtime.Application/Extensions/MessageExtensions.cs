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

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="Message"/>s.
/// </summary>
public static class MessageExtensions
{

    /// <summary>
    /// Converts the <see cref="Message"/> into a new <see cref="ChatMessageContent"/>.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to convert.</param>
    /// <returns>A new <see cref="ChatMessageContent"/>.</returns>
    public static ChatMessageContent ToChatMessageContent(this Message message) => new()
    {
        Role = new (message.Role),
        AuthorName = message.Author,
        Items = [.. message.Parts.Select(p => p.ToKernelContent())],
        Metadata = message.Metadata
    };

}

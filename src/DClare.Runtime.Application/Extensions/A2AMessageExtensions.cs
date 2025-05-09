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

using A2A;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="A2A.Models.Message"/>s
/// </summary>
public static class A2AMessageExtensions
{

    /// <summary>
    /// Converts the <see cref="A2A.Models.Message"/> into a new <see cref="Message"/>.
    /// </summary>
    /// <param name="message">The <see cref="A2A.Models.Message"/> to convert.</param>
    /// <returns>A new <see cref="Message"/>.</returns>
    public static Message ToMessage(this A2A.Models.Message message) => new()
    {
        Role = message.Role == MessageRole.Agent ? AuthorRole.Assistant.Label : message.Role,
        Parts = [.. message.Parts == null ? [] : message.Parts.Select(p => p.ToMessagePart())],
        Metadata = message.Metadata?.AsReadOnly()!
    };

}

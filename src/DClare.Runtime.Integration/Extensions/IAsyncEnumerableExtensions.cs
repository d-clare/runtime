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

namespace DClare.Runtime;

/// <summary>
/// Defines extensions for <see cref="IAsyncEnumerable{T}"/> instances
/// </summary>
public static class IAsyncEnumerableExtensions
{

    /// <summary>
    /// Converts the specified stream of <see cref="StreamingChatMessageContent"/>s into a stream of <see cref="ChatMessage"/>s
    /// </summary>
    /// <param name="stream">The stream to convert</param>
    /// <param name="includeMetadata">A boolean indicating whether or not to include metadata</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new stream of <see cref="ChatMessage"/>s</returns>
    public static async IAsyncEnumerable<ChatMessage> AsMessageStreamAsync(this IAsyncEnumerable<StreamingChatMessageContent> stream, bool includeMetadata, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string? currentRole = null;
        var currentContent = new StringBuilder();
        IDictionary<string, object?>? metadata = null;
        await foreach (var part in stream.WithCancellation(cancellationToken))
        {
            var role = part.Role?.ToString();
            if (role != null)
            {
                if (currentContent.Length > 0 && currentRole != null) yield return new ChatMessage(currentRole, currentContent.ToString(), metadata);
                currentRole = role;
                currentContent.Clear();
                currentContent.Append(part.Content ?? string.Empty);
                if (includeMetadata && part.Metadata != null)
                {
                    metadata ??= new Dictionary<string, object?>();
                    foreach (var kvp in part.Metadata) metadata[kvp.Key] = kvp.Value;
                }
            }
            else
            {
                if (includeMetadata && part.Metadata != null)
                {
                    metadata ??= new Dictionary<string, object?>();
                    foreach (var kvp in part.Metadata) metadata[kvp.Key] = kvp.Value;
                }
                currentContent.Append(part.Content ?? string.Empty);
            }
        }
        if (currentContent.Length > 0) yield return new ChatMessage(currentRole ?? "assistant", currentContent.ToString(), metadata);
    }

}

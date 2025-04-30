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
/// Defines extensions for <see cref="ChatResponseStream"/>s
/// </summary>
public static class AgentResponseStreamExtensions
{

    /// <summary>
    /// Converts the <see cref="ChatResponseStream"/> into a new <see cref="ChatResponse"/>
    /// </summary>
    /// <param name="response">The <see cref="ChatResponseStream"/> to convert</param>
    /// <param name="includeMetadata">A boolean indicating whether or not to include metadata</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="ChatResponse"/></returns>
    public static async Task<ChatResponse> ToResponseAsync(this ChatResponseStream response, bool includeMetadata, CancellationToken cancellationToken = default) => new(response.Id, await response.Stream.AsMessageStreamAsync(includeMetadata, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false));

}

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

using Microsoft.AspNetCore.Http;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to embed content.
/// </summary>
public interface IContentEmbedder
{

    /// <summary>
    /// Embeds the specified <see cref="IFormFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="IFormFile"/> to embed.</param>
    /// <param name="options">The options to use.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    Task EmbedAsync(IFormFile file, ContentEmbeddingOptions options, CancellationToken cancellationToken = default);

}

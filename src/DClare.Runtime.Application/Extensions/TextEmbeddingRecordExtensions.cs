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
/// Defines extensions for <see cref="TextEmbeddingRecord{TKey}"/> instances
/// </summary>
public static class TextEmbeddingRecordExtensions
{

    /// <summary>
    /// Converts the <see cref="TextEmbeddingRecord{TKey}"/> into a new <see cref="SemanticSearchResult"/>.
    /// </summary>
    /// <typeparam name="TKey">The key used to uniquely identify the <see cref="TextEmbeddingRecord{TKey}"/> to convert.</typeparam>
    /// <param name="result">The <see cref="TextEmbeddingRecord{TKey}"/> to convert.</param>
    /// <returns>A new <see cref="SemanticSearchResult"/>.</returns>
    public static SemanticSearchResult AsSearchResult<TKey>(this TextEmbeddingRecord<TKey> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return new()
        {
            CreatedAt = result.CreatedAt,
            Text = result.Text,
            Reference = result.Reference,
            Metadata = result.Metadata,
            Tags = result.Tags
        };
    }

}

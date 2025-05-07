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
/// Represents metadata about the original source of a text snippet, including its identifier, type, location within the source (such as page or section), and an optional reference link for navigation or citation purposes.
/// </summary>
public record TextEmbeddingRecordReference
{

    /// <summary>
    /// Gets or sets the identifier for the referenced source (e.g., document ID, slug, or name).
    /// </summary>
    public virtual string? Id { get; init; }

    /// <summary>
    /// Gets or sets the type of the source (e.g., "pdf", "web", "chat", "markdown").
    /// </summary>
    public virtual string? Type { get; init; }

    /// <summary>
    /// Gets or sets the section index of the text within the referenced source, if applicable.
    /// </summary>
    public virtual int? SectionIndex { get; init; }

    /// <summary>
    /// Gets or sets the page number in the referenced source, if applicable (e.g., for PDFs).
    /// </summary>
    public virtual int? PageNumber { get; init; }

    /// <summary>
    /// Gets or sets a direct link to the referenced source, such as a URL or file path.
    /// </summary>
    public virtual string? Link { get; init; }

}

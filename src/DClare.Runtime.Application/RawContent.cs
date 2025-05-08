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
/// Represents a unit of raw content, such as a block of text or an image segment, optionally associated with a section index or a page number.
/// </summary>
public class RawContent
{

    /// <summary>
    /// Gets or sets the raw text content, if any.
    /// </summary>
    public virtual string? Text { get; init; }

    /// <summary>
    /// Gets or sets the image content, if any.
    /// </summary>
    public virtual ReadOnlyMemory<byte>? Image { get; init; }

    /// <summary>
    /// Gets or sets the zero-based index of the section from which the content was derived, if applicable.
    /// </summary>
    public virtual int? SectionIndex { get; init; }

    /// <summary>
    /// Gets or sets the page number associated with the content, if applicable.
    /// </summary>
    public virtual int? PageNumber { get; init; }

}


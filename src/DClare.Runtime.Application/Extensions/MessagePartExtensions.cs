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

using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using static System.Net.Mime.MediaTypeNames;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="MessagePart"/>s.
/// </summary>
public static class MessagePartExtensions
{

    /// <summary>
    /// Converts the <see cref="MessagePart"/> into a new <see cref="KernelContent"/>.
    /// </summary>
    /// <param name="messagePart">The <see cref="MessagePart"/> to convert.</param>
    /// <returns>A new <see cref="KernelContent"/>.</returns>
    public static KernelContent ToKernelContent(this MessagePart messagePart)
    {
        ArgumentNullException.ThrowIfNull(messagePart);
        return messagePart switch
        {
            AnnotationPart annotation => new AnnotationContent
            {
                MimeType = annotation.MimeType,
                ModelId = annotation.ModelId,
                Quote = annotation.Quote,
                StartIndex = annotation.Start,
                EndIndex = annotation.End,
                Metadata = annotation.Metadata
            },
            BinaryPart binary => new BinaryContent
            {
                MimeType = binary.MimeType,
                ModelId = binary.ModelId,
                Uri = binary.Uri,
                Data = binary.Data,
                Metadata = binary.Metadata
            },
            TextPart text => new TextContent
            {
                MimeType = text.MimeType,
                ModelId = text.ModelId,
                Text = text.Text,
                Encoding = text.Encoding ?? Encoding.UTF8,
                Metadata = text.Metadata
            },
            _ => throw new NotSupportedException($"The specified {nameof(MessagePart)} type '{messagePart.GetType().Name}' is not supported")
        };
    }

    /// <summary>
    /// Converts the <see cref="MessagePart"/>s into a new <see cref="ChatMessageContentItemCollection"/>.
    /// </summary>
    /// <param name="messageParts">The <see cref="MessagePart"/>s to convert.</param>
    /// <returns>A new <see cref="ChatMessageContentItemCollection"/>.</returns>
    public static ChatMessageContentItemCollection ToChatMessageContentItemCollection(this IEnumerable<MessagePart> messageParts) => [.. messageParts.Select(p => p.ToKernelContent())];

    /// <summary>
    /// Converts the <see cref="MessagePart"/> into a new <see cref="A2A.Models.Part"/>.
    /// </summary>
    /// <param name="messagePart">The <see cref="MessagePart"/> to convert.</param>
    /// <returns>A new <see cref="A2A.Models.Part"/></returns>
    public static A2A.Models.Part ToA2APart(this MessagePart messagePart)
    {
        ArgumentNullException.ThrowIfNull(messagePart);
        return messagePart switch
        {
            AnnotationPart annotation => new A2A.Models.TextPart
            {
                Text = annotation.Quote,
                Metadata = annotation.Metadata == null ? null : new(annotation.Metadata!)
            },
            BinaryPart binary => new A2A.Models.FilePart
            {
                File = new()
                {
                    MimeType = binary.MimeType,
                    Uri = binary.Uri,
                    Bytes = binary.Data.HasValue ? Convert.ToBase64String(binary.Data.Value.ToArray()) : null,
                },
                Metadata = binary.Metadata == null ? null : new(binary.Metadata!)
            },
            TextPart text => new A2A.Models.TextPart
            {
                Text = text.Text,
                Metadata = text.Metadata == null ? null : new(text.Metadata!)
            },
            _ => throw new NotSupportedException($"The specified {nameof(MessagePart)} type '{messagePart.GetType().Name}' is not supported")
        };
    }

}

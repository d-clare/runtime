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

using Neuroglia.Serialization.Json;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="A2A.Models.Part"/>s
/// </summary>
public static class A2APartExtensions
{

    /// <summary>
    /// Converts the <see cref=" A2A.Models.Part"/> into a new <see cref="MessagePart"/>.
    /// </summary>
    /// <param name="part">The <see cref="A2A.Models.Part"/> to convert.</param>
    /// <returns>A new <see cref="MessagePart"/>.</returns>
    public static MessagePart ToMessagePart(this A2A.Models.Part part)
    {
        ArgumentNullException.ThrowIfNull(part);
        return part switch
        {
            A2A.Models.TextPart text => new TextPart
            {
                MimeType = MediaTypeNames.Text.Plain,
                Encoding = Encoding.UTF8,
                Text = text.Text,
                Metadata = text.Metadata?.AsReadOnly()!
            },
            A2A.Models.FilePart file => new BinaryPart
            {
                MimeType = file.File.MimeType,
                Uri = file.File.Uri,
                Data = string.IsNullOrWhiteSpace(file.File.Bytes) ? null : Convert.FromBase64String(file.File.Bytes),
                Metadata = new EquatableDictionary<string, object?>((file.Metadata ?? [])!)
                {
                    ["fileName"] = file.File.Name
                }.AsReadOnly()
            },
            A2A.Models.DataPart data => new TextPart
            {
                MimeType = MediaTypeNames.Application.Json,
                Encoding = Encoding.UTF8,
                Text = JsonSerializer.Default.SerializeToText(data.Data),
                Metadata = data.Metadata?.AsReadOnly()!
            },
            _ => throw new NotSupportedException($"The specified {nameof(A2A.Models.Part)} type '{part.GetType().Name}' is not supported")
        };
    }

    /// <summary>
    /// Converts the <see cref=" A2A.Models.Part"/> into a new <see cref="MessageFragmentPart"/>.
    /// </summary>
    /// <param name="part">The <see cref="A2A.Models.Part"/> to convert.</param>
    /// <returns>A new <see cref="MessageFragmentPart"/>.</returns>
    public static MessageFragmentPart ToMessageFragmentPart(this A2A.Models.Part part)
    {
        ArgumentNullException.ThrowIfNull(part);
        return part switch
        {
            A2A.Models.TextPart text => new TextFragmentPart
            {
                Encoding = Encoding.UTF8,
                Text = text.Text,
                Metadata = text.Metadata?.AsReadOnly()!
            },
            A2A.Models.FilePart file => new BinaryFragmentPart
            {
                Uri = file.File.Uri,
                Data = string.IsNullOrWhiteSpace(file.File.Bytes) ? null : Convert.FromBase64String(file.File.Bytes),
                Metadata = new EquatableDictionary<string, object?>((file.Metadata ?? [])!)
                {
                    ["fileName"] = file.File.Name
                }.AsReadOnly()
            },
            A2A.Models.DataPart data => new TextFragmentPart
            {
                Encoding = Encoding.UTF8,
                Text = JsonSerializer.Default.SerializeToText(data.Data),
                Metadata = data.Metadata?.AsReadOnly()!
            },
            _ => throw new NotSupportedException($"The specified {nameof(A2A.Models.Part)} type '{part.GetType().Name}' is not supported")
        };
    }


}

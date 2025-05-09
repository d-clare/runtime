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
using static System.Net.Mime.MediaTypeNames;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="StreamingKernelContent"/>s
/// </summary>
public static class StreamingKernelContentExtensions
{

    /// <summary>
    /// Converts the <see cref="StreamingKernelContent"/> into a new <see cref="MessageFragmentPart"/>.
    /// </summary>
    /// <param name="content">The <see cref="StreamingKernelContent"/> to convert.</param>
    /// <returns>a new <see cref="MessagePart"/>.</returns>
    public static MessageFragmentPart ToMessageFragmentPart(this StreamingKernelContent content) => content switch
    {
        StreamingAnnotationContent annotation => new AnnotationFragmentPart
        {
            ModelId = annotation.ModelId,
            Quote = annotation.Quote,
            Start = annotation.StartIndex,
            End = annotation.EndIndex,
            Metadata = annotation.Metadata
        },
        StreamingTextContent text => new TextFragmentPart
        {
            ModelId = text.ModelId,
            Text = text.Text,
            Encoding = text.Encoding,
            Metadata = text.Metadata
        },
        _ => new TextFragmentPart
        {
            Text = string.Empty
        },
    };

}

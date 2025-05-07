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

using A2A.Models;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="Artifact"/>s
/// </summary>
public static class ArtifactExtensions
{

    /// <summary>
    /// Converts an <see cref="Artifact"/> into a sequence of <see cref="Integration.Models.StreamingChatMessageContent"/> entries,
    /// where each <see cref="Part"/> becomes a distinct message content block.
    /// </summary>
    /// <param name="artifact">The structured artifact to convert.</param>
    /// <param name="role">The chat role associated with each content part.</param>
    /// <returns>An ordered list of <see cref="Integration.Models.StreamingChatMessageContent"/> items.</returns>
    public static IEnumerable<Integration.Models.StreamingChatMessageContent> ToStreamingChatMessageContents(this Artifact artifact)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        if (artifact.Parts == null || artifact.Parts.Count == 0) yield break;
        foreach (var part in artifact.Parts)
        {
            switch (part)
            {
                case TextPart textPart:
                    yield return new Integration.Models.StreamingChatMessageContent(textPart.Text, AuthorRole.Assistant.Label, artifact.Metadata?.AsReadOnly()!);
                    break;
                case FilePart filePart:
                    var fileContentBuilder = new StringBuilder();
                    fileContentBuilder.AppendLine("----- FILE -----");
                    if (!string.IsNullOrWhiteSpace(filePart.File.Name)) fileContentBuilder.AppendLine($"Name    : {filePart.File.Name}");
                    if (!string.IsNullOrWhiteSpace(filePart.File.MimeType)) fileContentBuilder.AppendLine($"MIME    : {filePart.File.MimeType}");
                    if (!string.IsNullOrWhiteSpace(filePart.File.Bytes)) fileContentBuilder.AppendLine($"Base64  : {filePart.File.Bytes}");
                    else if (filePart.File.Uri is not null) fileContentBuilder.AppendLine($"URI     : {filePart.File.Uri}");
                    fileContentBuilder.AppendLine("----------------");
                    yield return new Integration.Models.StreamingChatMessageContent(fileContentBuilder.ToString(), AuthorRole.Assistant.Label, artifact.Metadata?.AsReadOnly()!);
                    break;
                case DataPart dataPart:
                    var jsonContentBuilder = new StringBuilder();
                    jsonContentBuilder.AppendLine("```json");
                    jsonContentBuilder.AppendLine(JsonSerializer.Serialize(dataPart.Data));
                    jsonContentBuilder.AppendLine("```");
                    yield return new Integration.Models.StreamingChatMessageContent(jsonContentBuilder.ToString(), AuthorRole.Assistant.Label, artifact.Metadata?.AsReadOnly()!);
                    break;
                default:
                    throw new NotSupportedException($"The specified part type '{part.Type ?? "None"}' is not supported");
            }
        }
    }

}

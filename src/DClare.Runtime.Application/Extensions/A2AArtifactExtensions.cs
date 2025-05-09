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

using Microsoft.SemanticKernel.ChatCompletion;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="A2A.Models.Artifact"/>s.
/// </summary>
public static class A2AArtifactExtensions
{

    /// <summary>
    /// Converts the <see cref="A2A.Models.Artifact"/> into a new <see cref="Message"/>.
    /// </summary>
    /// <param name="artifact">The <see cref="A2A.Models.Artifact"/> to convert.</param>
    /// <returns>A new <see cref="Message"/>.</returns>
    public static Message ToMessage(this A2A.Models.Artifact artifact, string? role = null)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        return new Message()
        {
            Role = string.IsNullOrWhiteSpace(role) ? AuthorRole.Assistant.Label : role,
            Parts = [.. artifact.Parts == null ? [] : artifact.Parts.Select(p => p.ToMessagePart())],
            Metadata = new EquatableDictionary<string, object?>((artifact.Metadata ?? [])!)
            {
                ["name"] = artifact.Name,
                ["description"] = artifact.Description
            }.AsReadOnly()
        };
    }

    /// <summary>
    /// Converts the <see cref="A2A.Models.Artifact"/> into a new <see cref="MessageFragment"/>.
    /// </summary>
    /// <param name="artifact">The <see cref="A2A.Models.Artifact"/> to convert.</param>
    /// <returns>A new <see cref="MessageFragment"/>.</returns>
    public static MessageFragment ToMessageFragment(this A2A.Models.Artifact artifact, string? role = null)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        return new MessageFragment()
        {
            Role = string.IsNullOrWhiteSpace(role) ? AuthorRole.Assistant.Label : role,
            Parts = [.. artifact.Parts == null ? [] : artifact.Parts.Select(p => p.ToMessageFragmentPart())],
            Metadata = new EquatableDictionary<string, object?>((artifact.Metadata ?? [])!)
            {
                ["name"] = artifact.Name,
                ["description"] = artifact.Description
            }.AsReadOnly()
        };
    }

}

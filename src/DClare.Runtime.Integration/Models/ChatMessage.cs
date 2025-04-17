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

namespace DClare.Runtime.Integration.Models;

/// <summary>
/// Represents a complete chat message
/// </summary>
/// <param name="Role">The message's role</param>
/// <param name="Content">The message's content</param>
/// <param name="Metadata">The message's metadata, if any</param>
public record ChatMessage(string Role, string? Content, IDictionary<string, object?>? Metadata = null);

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
/// Represents an object that describes the result of invoking a chat with a user message, including a unique response identifier and a stream of output messages
/// </summary>
/// <param name="Id">The response's unique identifier</param>
/// <param name="Stream">A stream of chat messages produced by the chat</param>
public record ChatResponseStream(string Id, IAsyncEnumerable<StreamingChatMessageContent> Stream);

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

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Defines the fundamentals of a high-level orchestration process involving one or more agents
/// </summary>
public interface IProcess
{

    /// <summary>
    /// Runs the process
    /// </summary>
    /// <param name="prompt">The prompt to process</param>
    /// <param name="sessionId">The id of the session, if any, to run the process for</param>
    /// <param name="parameters">A key/value mapping containing the invocation's parameters, if any</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="ChatResponse"/>, used to stream the process responses</returns>
    Task<ChatResponse> InvokeAsync(string prompt, string? sessionId = null, IDictionary<string, object>? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs the process
    /// </summary>
    /// <param name="prompt">The prompt to process</param>
    /// <param name="sessionId">The id of the session, if any, to run the process for</param>
    /// <param name="parameters">A key/value mapping containing the invocation's parameters, if any</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="ChatResponseStream"/>, used to stream the process response content</returns>
    Task<ChatResponseStream> InvokeStreamingAsync(string prompt, string? sessionId = null, IDictionary<string, object>? parameters = null, CancellationToken cancellationToken = default);

}

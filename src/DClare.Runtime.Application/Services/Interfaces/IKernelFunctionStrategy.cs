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
/// Defines the fundamentals of a strategy that encapsulates a kernel function and its associated configuration, enabling it to be invoked within agentic workflows
/// </summary>
public interface IKernelFunctionStrategy
{

    /// <summary>
    /// Invokes the configured kernel function
    /// </summary>
    /// <param name="arguments">Optional named arguments to pass to the function</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A stream of messages returned by the function</returns>
    IAsyncEnumerable<ChatMessage> InvokeAsync(IDictionary<string, object?>? arguments = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invokes the configured kernel function
    /// </summary>
    /// <param name="arguments">Optional named arguments to pass to the function</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A stream of message content returned by the function</returns>
    IAsyncEnumerable<Integration.Models.StreamingChatMessageContent> InvokeStreamingAsync(IDictionary<string, object?>? arguments = null, CancellationToken cancellationToken = default);

}
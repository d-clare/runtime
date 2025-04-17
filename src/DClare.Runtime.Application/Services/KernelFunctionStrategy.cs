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
/// Represents the default implementation of the <see cref="IKernelFunctionStrategy"/> interface
/// </summary>
/// <param name="function">The <see cref="KernelFunction"/> to invoke as part of the strategy</param>
/// <param name="kernel">The kernel used to run the function</param>
public class KernelFunctionStrategy(KernelFunction function, Kernel kernel)
    : IKernelFunctionStrategy
{

    /// <summary>
    /// Gets the <see cref="KernelFunction"/> to invoke as part of the strategy
    /// </summary>
    protected KernelFunction Function { get; } = function;

    /// <summary>
    /// Gets the kernel used to run the function
    /// </summary>
    protected Kernel Kernel { get; } = kernel;

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<ChatMessage> InvokeAsync(IDictionary<string, object?>? arguments = null, CancellationToken cancellationToken = default) => InvokeStreamingAsync(arguments, cancellationToken).AsMessageStreamAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<Integration.Models.StreamingChatMessageContent> InvokeStreamingAsync(IDictionary<string, object?>? arguments = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var kernelArguments = arguments == null ? null : new KernelArguments(arguments);
        await foreach (var token in Kernel.InvokeStreamingAsync(Function, kernelArguments, cancellationToken).ConfigureAwait(false)) yield return new(token.ToString(), Metadata: token.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
    }

}
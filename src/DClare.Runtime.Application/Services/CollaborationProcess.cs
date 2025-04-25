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

using DClare.Sdk.Models.Processes;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents a collaboration implementation of the <see cref="IProcess"/> interface
/// </summary>
/// <param name="definition">The <see cref="IProcess"/>'s definition</param>
/// <param name="components">A a collection, if any, containing the reusable components available to the <see cref="IProcess"/></param>
/// <param name="logger">The service used to perform logging</param>
public class CollaborationProcess(CollaborationAgenticProcessDefinition definition, ComponentCollectionDefinition? components, ILogger<CollaborationProcess> logger)
    : IProcess
{

    /// <summary>
    /// Gets the <see cref="IProcess"/>'s definition
    /// </summary>
    protected CollaborationAgenticProcessDefinition Definition { get; } = definition;

    /// <summary>
    /// Gets a collection, if any, containing the reusable components available to the <see cref="IProcess"/>
    /// </summary>
    protected ComponentCollectionDefinition? Components { get; } = components;

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <inheritdoc/>
    public Task<ChatResponse> InvokeAsync(string prompt, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ChatResponseStream> InvokeStreamingAsync(string prompt, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}
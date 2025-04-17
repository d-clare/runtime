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
/// Represents the default implementation of the <see cref="IAgenticProcessFactory"/> interface
/// </summary>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
public class AgenticProcessFactory(IServiceProvider serviceProvider)
    : IAgenticProcessFactory
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <inheritdoc/>
    public virtual Task<IAgenticProcess> CreateAsync(AgenticProcessDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(definition);
        return Task.FromResult<IAgenticProcess>(definition.Type switch
        {
            AgenticProcessType.Collaboration => ActivatorUtilities.CreateInstance<CollaborationAgenticProcess>(ServiceProvider, definition.Collaboration!, components!),
            AgenticProcessType.Convergence => ActivatorUtilities.CreateInstance<ConvergenceAgenticProcess>(ServiceProvider, definition.Convergence!, components!),
            _ => throw new NotSupportedException($"The specified agentic process type '{definition.Type}' is not supported")
        });
    }

}

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
/// Defines the fundamentals of a service used to create <see cref="IAgent"/>s.
/// </summary>
public interface IAgentFactory
{

    /// <summary>
    /// Creates a new <see cref="IAgent"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="IAgent"/> to create.</param>
    /// <param name="definition">The definition of the <see cref="IAgent"/> to create.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAgent"/>.</returns>
    Task<IAgent> CreateAsync(string name, AgentDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default);

}

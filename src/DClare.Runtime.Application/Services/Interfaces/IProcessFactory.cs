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
/// Defines the fundamentals of a service used to create <see cref="IProcess"/>es
/// </summary>
public interface IProcessFactory
{

    /// <summary>
    /// Creates a new <see cref="IProcess"/>
    /// </summary>
    /// <param name="definition">The definition of the <see cref="IProcess"/> to create</param>
    /// <param name="components">A collection, if any, containing the reusable components potentially referenced by the <see cref="IProcess"/> to create</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IProcess"/></returns>
    Task<IProcess> CreateAsync(ProcessDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default);

}

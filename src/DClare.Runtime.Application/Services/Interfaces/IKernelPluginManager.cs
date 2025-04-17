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
/// Defines the fundamentals of a service used to manage <see cref="KernelPlugin"/>s
/// </summary>
public interface IKernelPluginManager
{

    /// <summary>
    /// Gets or loads the specified <see cref="KernelPlugin"/>
    /// </summary>
    /// <param name="name">The name of the <see cref="KernelPlugin"/> to get or load</param>
    /// <param name="definition">A <see cref="ToolsetDefinition"/> that defines the <see cref="KernelPlugin"/> to load</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="KernelPlugin"/></returns>
    Task<KernelPlugin> GetOrLoadAsync(string name, ToolsetDefinition definition, CancellationToken cancellationToken = default);

}

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
/// Defines the fundamentals of a service used to create <see cref="Kernel"/>s.
/// </summary>
public interface IKernelFactory
{

    /// <summary>
    /// Creates a new <see cref="Kernel"/>.
    /// </summary>
    /// <param name="definition">The <see cref="KernelDefinition"/> used to configure the <see cref="Kernel"/> to create.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="Kernel"/>.</returns>
    Task<Kernel> CreateAsync(KernelDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default);

}

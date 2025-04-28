// Copyright � 2025-Present The DClare Authors
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

using DClare.Runtime.Integration.Queries.ApplicationManifest;

namespace DClare.Runtime.Application.Queries.ApplicationManifest;

/// <summary>
/// Represents the service used to handle <see cref="GetManifestQuery"/> instances
/// </summary>
/// <param name="manifestHandler">The service used to handle the application's <see cref="Manifest"/></param>
public class GetManifestQueryHandler(IManifestHandler manifestHandler)
    : IQueryHandler<GetManifestQuery, Manifest>
{

    /// <inheritdoc/>
    public async Task<IOperationResult<Manifest>> HandleAsync(GetManifestQuery query, CancellationToken cancellationToken = default)
    {
        return this.Ok(await manifestHandler.GetManifestAsync(cancellationToken).ConfigureAwait(false));
    }

}

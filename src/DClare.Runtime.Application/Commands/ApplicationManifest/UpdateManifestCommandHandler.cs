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

using DClare.Runtime.Integration.Commands.ApplicationManifest;

namespace DClare.Runtime.Application.Commands.ApplicationManifest;

/// <summary>
/// Represents the command used to update the application's manifest
/// </summary>
/// <param name="manifestHandler">The service used to handle the application's manifest</param>
public class UpdateManifestCommandHandler(IManifestHandler manifestHandler)
    : ICommandHandler<UpdateManifestCommand>
{

    /// <inheritdoc/>
    public async Task<IOperationResult> HandleAsync(UpdateManifestCommand command, CancellationToken cancellationToken = default)
    {
        await manifestHandler.SetManifestAsync(command.Manifest, cancellationToken).ConfigureAwait(false);
        return this.Ok();
    }

}

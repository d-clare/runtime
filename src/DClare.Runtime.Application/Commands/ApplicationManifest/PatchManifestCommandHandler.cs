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
using Neuroglia.Data.PatchModel.Services;

namespace DClare.Runtime.Application.Commands.ApplicationManifest;

/// <summary>
/// Represents the command used to patch the application's manifest
/// </summary>
/// <param name="manifestHandler">The service used to handle the application's manifest</param>
/// <param name="patchHandlers">An <see cref="IEnumerable{T}"/> containing all available <see cref="IPatchHandler"/> implementations</param>
public class PatchManifestCommandHandler(IManifestHandler manifestHandler, IEnumerable<IPatchHandler> patchHandlers)
    : ICommandHandler<PatchManifestCommand>
{

    /// <inheritdoc/>
    public async Task<IOperationResult> HandleAsync(PatchManifestCommand command, CancellationToken cancellationToken = default)
    {
        var manifest = await manifestHandler.GetManifestAsync(cancellationToken).ConfigureAwait(false);
        var patchHandler = patchHandlers.FirstOrDefault(h => h.Supports(command.Patch.Type)) ?? throw new ProblemDetailsException(Problems.UnsupportedPatchType(command.Patch.Type));
        manifest = (await patchHandler.ApplyPatchAsync(command.Patch, manifest, cancellationToken).ConfigureAwait(false))!;
        await manifestHandler.SetManifestAsync(manifest, cancellationToken).ConfigureAwait(false);
        return this.Ok();
    }

}
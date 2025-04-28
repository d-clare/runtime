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

using DClare.Runtime.Integration.Commands.ApplicationManifest;
using DClare.Runtime.Integration.Queries.ApplicationManifest;
using DClare.Sdk.Models;
using Neuroglia.Data;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage the application's manifest
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
[Route("api/[controller]"), ApiController]
public class ManifestController(IMediator mediator)
    : Controller
{

    /// <summary>
    /// Gets the application's manifest
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the action's result</returns>
    [HttpGet, Produces("application/x-yaml", "application/yaml", "text/yaml")]
    [ProducesResponseType(typeof(Manifest), (int)HttpStatusCode.OK)]
    public virtual async Task<IActionResult> GetManifestAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new GetManifestQuery(), cancellationToken).ConfigureAwait(false);
        return this.Process(result);
    }

    /// <summary>
    /// Updates the application's manifest
    /// </summary>
    /// <param name="manifest">The updated <see cref="Manifest"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the action's result</returns>
    [HttpPut, Consumes("application/x-yaml", "application/yaml", "text/yaml")]
    [ProducesResponseType(typeof(Manifest), (int)HttpStatusCode.NoContent)]
    public virtual async Task<IActionResult> UpdateManifestAsync([FromBody] Manifest manifest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new UpdateManifestCommand(manifest), cancellationToken).ConfigureAwait(false);
        return this.Process(result, (int)HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Patches the application's manifest
    /// </summary>
    /// <param name="patch">The patch to apply</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the action's result</returns>
    [HttpPatch]
    [ProducesResponseType(typeof(Manifest), (int)HttpStatusCode.NoContent)]
    public virtual async Task<IActionResult> UpdateManifestAsync([FromBody] Patch patch, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new PatchManifestCommand(patch), cancellationToken).ConfigureAwait(false);
        return this.Process(result, (int)HttpStatusCode.NoContent);
    }

}
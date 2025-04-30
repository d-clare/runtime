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

using DClare.Runtime.Integration.Commands.Processes;
using System.Text;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage Agentic Processes
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
[Route("api/[controller]"), ApiController]
public class ProcessesController(IMediator mediator, IJsonSerializer jsonSerializer)
    : Controller
{

    /// <summary>
    /// Invokes the specified process
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpPost("invoke")]
    [ProducesResponseType(typeof(ChatResponse), (int)HttpStatusCode.OK)]
    public virtual async Task<IActionResult> InvokeProcessAsync([FromBody] InvokeProcessCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(command, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) return this.Process(result);
        return Ok(await result.Data!.ToResponseAsync(true, cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Invokes the specified process
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpPost("invoke/stream")]
    [ProducesResponseType(typeof(ChatResponse), (int)HttpStatusCode.OK)]
    public virtual async Task<IActionResult> InvokeProcessStreamAsync([FromBody] InvokeProcessCommand command, CancellationToken cancellationToken)
    {
        if (!this.ModelState.IsValid) return ValidationProblem(this.ModelState);
        var result = await mediator.ExecuteAsync(command, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) return this.Process(result);
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        Response.Headers["X-Response-Id"] = result.Data!.Id;
        await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await foreach (var e in result.Data!.Stream.WithCancellation(cancellationToken))
            {
                var payload = e with
                {
                    Metadata = command.IncludeMetadata ? e.Metadata : null
                };
                var sseMessage = $"data: {jsonSerializer.SerializeToText(payload)}\n\n";
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), cancellationToken).ConfigureAwait(false);
                await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException) { }
        return Ok();
    }

}
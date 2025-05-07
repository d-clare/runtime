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

using DClare.Runtime.Integration.Commands.Agents;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage AI Agents
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonOptions">The service used to access the current <see cref="JsonOptions"/></param>
[Route("api/[controller]"), ApiController]
public class AgentsController(IMediator mediator, IJsonSerializer jsonSerializer, IOptions<JsonOptions> jsonOptions)
    : NamespacedResourceController<Agent>(mediator, jsonSerializer)
{

    /// <summary>
    /// Invokes the specified agent.
    /// </summary>
    /// <param name="namespace">The namespace the agent to invoke belongs to.</param>
    /// <param name="name">The name of the agent to invoke.</param>
    /// <param name="parameters">The invocation parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpPost("{namespace}/{name}/invoke", Name = "Invoke Agent")]
    [EndpointDescription("Invokes the specified AI agent using the provided input message and options, processes the interaction, and returns a chat response")]
    [ProducesResponseType(typeof(ChatResponse), (int)HttpStatusCode.OK)]
    public virtual async Task<IActionResult> InvokeAgentAsync([Description("The namespace the agent to invoke belongs to.")] string @namespace, [Description("The name of the agent to invoke.")] string name, [FromBody, Description("The invocation parameters.")] InvokeAgentCommandParameters parameters, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await Mediator.ExecuteAsync(new InvokeAgentCommand()
        {
            Agent = new()
            {
                Namespace = @namespace,
                Name = name
            },
            Parameters = parameters
        }, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) return this.Process(result);
        return Ok(await result.Data!.ToResponseAsync(parameters.Options?.IncludeMetadata ?? false, cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Invokes the specified agent.
    /// </summary>
    /// <param name="namespace">The namespace the agent to invoke belongs to.</param>
    /// <param name="name">The name of the agent to invoke.</param>
    /// <param name="parameters">The invocation parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpPost("{namespace}/{name}/invoke/stream", Name = "Invoke Agent (Streamed)")]
    [EndpointDescription("Invokes the specified AI agent using the provided input message and options, processes the interaction, and returns a streamed result")]
    [ProducesResponseType(typeof(IEnumerable<StreamingChatMessageContent>), (int)HttpStatusCode.OK)]
    public virtual async Task InvokeAgentStreamAsync([Description("The namespace the agent to invoke belongs to.")] string @namespace, [Description("The name of the agent to invoke.")] string name, [FromBody, Description("The invocation parameters.")] InvokeAgentCommandParameters parameters, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await System.Text.Json.JsonSerializer.SerializeAsync(Response.Body, ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState), jsonOptions.Value.JsonSerializerOptions, cancellationToken);
            return;
        }
        var result = await Mediator.ExecuteAsync(new InvokeAgentCommand()
        {
            Agent = new()
            {
                Namespace = @namespace,
                Name = name
            },
            Parameters = parameters
        }, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess())
        {
            Response.StatusCode = result.Status;
            await System.Text.Json.JsonSerializer.SerializeAsync(Response.Body, ProblemDetailsFactory.CreateProblemDetails(HttpContext), jsonOptions.Value.JsonSerializerOptions, cancellationToken);
            return;
        }
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
                    Metadata = parameters.Options?.IncludeMetadata ?? false ? e.Metadata : null
                };
                var sseMessage = $"data: {JsonSerializer.SerializeToText(payload)}\n\n";
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), cancellationToken).ConfigureAwait(false);
                await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException) { }
    }

}

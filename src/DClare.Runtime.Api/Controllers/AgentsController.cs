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
using DClare.Runtime.Integration.Commands.Resources;
using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage AI agents
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonOptions">The service used to access the current <see cref="JsonOptions"/></param>
[Route("api/agents"), ApiController]
public class AgentsController(IMediator mediator, IJsonSerializer jsonSerializer, IOptions<JsonOptions> jsonOptions)
    : ResourceControllerBase(mediator, jsonSerializer)
{

    /// <summary>
    /// Gets the definition of the managed agent.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("definition", Name = "Get Agent Definition")]
    [EndpointDescription("Gets the definition of the managed agent agent.")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetAgentDefinitionAsync(CancellationToken cancellationToken = default)
    {
        return this.Process(await Mediator.ExecuteAsync(new GetResourceDefinitionQuery<Agent>(), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Creates a new agent.
    /// </summary>
    /// <param name="agent">The agent to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPost(Name = "Create Agent")]
    [EndpointDescription("Creates a new agent.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> CreateAgentAsync([FromBody, Description("The agent to create.")] Agent agent, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new CreateResourceCommand<Agent>(agent), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Gets the agent with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the agent to get.</param>
    /// <param name="namespace">The namespace the agent to get belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/{name}", Name = "Get Agent")]
    [EndpointDescription("Gets the agent with the specified name and namespace.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> GetAgentAsync([Description("The name of the agent to get.")] string name, [Description("The namespace the agent to get belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new GetResourceQuery<Agent>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Enumerates matching agents.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet(Name = "Get Agents")]
    [EndpointDescription("Enumerates matching agents.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetAgentsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<Agent>(null, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching agents.
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}", Name = "Get Agents By Namespace")]
    [EndpointDescription("Enumerates the matching agents belonging to the specified namespace.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetAgentsAsync([Description("The namespace the agents to enumerate belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<Agent>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching agents.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("list", Name = "List Agents")]
    [EndpointDescription("List matching agents.")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListAgentsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<Agent>(null, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching agents.
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/list", Name = "List Agents By Namespace")]
    [EndpointDescription("List the matching agents belonging to the specified namespace..")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListAgentsAsync([Description("The namespace the agents to list belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<Agent>(@namespace, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Watches matching agents.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("watch", Name = "Watch Agents")]
    [EndpointDescription("Watches matching agents using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchAgentsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<Agent>(null, labelSelectors), cancellationToken).ConfigureAwait(false);
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await foreach (var e in response.Data!.WithCancellation(cancellationToken))
            {
                var sseMessage = $"data: {JsonSerializer.SerializeToText(e)}\n\n";
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), cancellationToken).ConfigureAwait(false);
                await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException) { }
    }

    /// <summary>
    /// Watches matching agents.
    /// </summary>
    /// <param name="namespace">The namespace the agents to watch belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/watch", Name = "Watch Agents By Namespace")]
    [EndpointDescription("Watches the matching agents belonging to the specified namespace using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchAgentsAsync([Description("The namespace the agents to watch belongs to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<Agent>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false);
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await foreach (var e in response.Data!.WithCancellation(cancellationToken))
            {
                var sseMessage = $"data: {JsonSerializer.SerializeToText(e)}\n\n";
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), cancellationToken).ConfigureAwait(false);
                await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException) { }
    }

    /// <summary>
    /// Monitors a specific agent.
    /// </summary>
    /// <param name="namespace">The namespace the agent to monitor belongs to.</param>
    /// <param name="name">The name of the agent to monitor.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/{name}/monitor", Name = "Monitor Agent")]
    [EndpointDescription("Monitors a specific agent using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task MonitorAgentAsync([Description("The name of the agent to monitor.")] string name, [Description("The namespace the agent to monitor belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.ExecuteAsync(new MonitorResourceQuery<Agent>(name, @namespace), cancellationToken).ConfigureAwait(false);
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await foreach (var e in response.Data!.WithCancellation(cancellationToken))
            {
                var sseMessage = $"data: {JsonSerializer.SerializeToText(e)}\n\n";
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), cancellationToken).ConfigureAwait(false);
                await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException) { }
    }

    /// <summary>
    /// Updates the specified agent.
    /// </summary>
    /// <param name="agent">The updated agent.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut(Name = "Update Agent")]
    [EndpointDescription("Updates the specified agent.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateAgentAsync([Description("The updated agent.")] Agent agent, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<Agent>(agent), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Updates the status of the specified agent.
    /// </summary>
    /// <param name="agent">The updated agent.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut("status", Name = "Update Agent Status")]
    [EndpointDescription("Updates the status of the specified agent.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateAgentStatusAsync([Description("The updated agent.")] Agent agent, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<Agent>(agent), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified agent.
    /// </summary>
    /// <param name="name">The name of the agent to patch.</param>
    /// <param name="namespace">The namespace the agent to patch belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}", Name = "Patch Agent")]
    [EndpointDescription("Patches the specified agent.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchAgentAsync([Description("The name of the agent to patch.")] string name, [Description("The namespace the agent to patch belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceCommand<Agent>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified resource's status.
    /// </summary>
    /// <param name="name">The name of the agent to patch the status of.</param>
    /// <param name="namespace">The namespace the agent to patch the status of belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}/status", Name = "Patch Agent Status")]
    [EndpointDescription("Patches the status of the specified agent.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchAgentStatusAsync([Description("The name of the agent to patch the status of.")] string name, [Description("The namespace the agent to patch the status of belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceStatusCommand<Agent>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

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
    public async Task<IActionResult> InvokeAgentAsync([Description("The namespace the agent to invoke belongs to.")] string @namespace, [Description("The name of the agent to invoke.")] string name, [FromBody, Description("The invocation parameters.")] InvokeAgentCommandParameters parameters, CancellationToken cancellationToken)
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
    public async Task InvokeAgentStreamAsync([Description("The namespace the agent to invoke belongs to.")] string @namespace, [Description("The name of the agent to invoke.")] string name, [FromBody, Description("The invocation parameters.")] InvokeAgentCommandParameters parameters, CancellationToken cancellationToken)
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

    /// <summary>
    /// Deletes the agent with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the agent to delete.</param>
    /// <param name="namespace">The namespace the agent to delete belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpDelete("{namespace}/{name}", Name = "Delete Agent")]
    [EndpointDescription("Deletes the specified agent.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> DeleteAgentAsync([Description("The name of the agent to delete.")] string name, [Description("The namespace the agent to delete belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new DeleteResourceCommand<Agent>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

}

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

using DClare.Runtime.Integration.Commands.Resources;
using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage Large Language Models (LLM).
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonOptions">The service used to access the current <see cref="JsonOptions"/></param>
[Route("api/llm"), ApiController]
public class LlmController(IMediator mediator, IJsonSerializer jsonSerializer, IOptions<JsonOptions> jsonOptions)
    : ResourceControllerBase(mediator, jsonSerializer)
{

    /// <summary>
    /// Gets the definition of the managed Large Language Model (LLM).
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("definition", Name = "Get LLM Definition")]
    [EndpointDescription("Gets the definition of the managed Large Language Model (LLM).")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetLlmDefinitionAsync(CancellationToken cancellationToken = default)
    {
        return this.Process(await Mediator.ExecuteAsync(new GetResourceDefinitionQuery<Llm>(), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Creates a new Large Language Model (LLM).
    /// </summary>
    /// <param name="llm">The Large Language Model (LLM) to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPost(Name = "Create Large Language Model (LLM)")]
    [EndpointDescription("Creates a new Large Language Model (LLM).")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> CreateLlmAsync([FromBody, Description("The Large Language Model (LLM) to create.")] Llm llm, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new CreateResourceCommand<Llm>(llm), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Gets the Large Language Model (LLM) with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the Large Language Model (LLM) to get.</param>
    /// <param name="namespace">The namespace the Large Language Model (LLM) to get belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/{name}", Name = "Get Large Language Model (LLM)")]
    [EndpointDescription("Gets the Large Language Model (LLM) with the specified name and namespace.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> GetLlmAsync([Description("The name of the Large Language Model (LLM) to get.")] string name, [Description("The namespace the Large Language Model (LLM) to get belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new GetResourceQuery<Llm>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Enumerates matchingLarge Language Models (LLM).
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet(Name = "Get Llms")]
    [EndpointDescription("Enumerates matchingLarge Language Models (LLM).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetLlmsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<Llm>(null, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matchingLarge Language Models (LLM).
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}", Name = "Get Large Language Models (LLM) By Namespace")]
    [EndpointDescription("Enumerates the matching Large Language Models (LLM) belonging to the specified namespace.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetLlmsAsync([Description("The namespace the Large Language Models (LLM) to enumerate belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<Llm>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matchingLarge Language Models (LLM).
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("list", Name = "List Llms")]
    [EndpointDescription("List matchingLarge Language Models (LLM).")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListLlmsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<Llm>(null, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matchingLarge Language Models (LLM).
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/list", Name = "List Large Language Models (LLM) By Namespace")]
    [EndpointDescription("List the matching Large Language Models (LLM) belonging to the specified namespace..")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListLlmsAsync([Description("The namespace the Large Language Models (LLM) to list belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<Llm>(@namespace, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Watches matchingLarge Language Models (LLM).
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("watch", Name = "Watch Llms")]
    [EndpointDescription("Watches matching Large Language Models (LLM) using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchLlmsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<Llm>(null, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Watches matchingLarge Language Models (LLM).
    /// </summary>
    /// <param name="namespace">The namespace the Large Language Models (LLM) to watch belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/watch", Name = "Watch Large Language Models (LLM) By Namespace")]
    [EndpointDescription("Watches the matching Large Language Models (LLM) belonging to the specified namespace using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchLlmsAsync([Description("The namespace the Large Language Models (LLM) to watch belongs to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<Llm>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Monitors a specific Large Language Model (LLM).
    /// </summary>
    /// <param name="namespace">The namespace the Large Language Model (LLM) to monitor belongs to.</param>
    /// <param name="name">The name of the Large Language Model (LLM) to monitor.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/{name}/monitor", Name = "Monitor Large Language Model (LLM)")]
    [EndpointDescription("Monitors a specific Large Language Model (LLM) using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task MonitorLlmAsync([Description("The name of the Large Language Model (LLM) to monitor.")] string name, [Description("The namespace the Large Language Model (LLM) to monitor belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.ExecuteAsync(new MonitorResourceQuery<Llm>(name, @namespace), cancellationToken).ConfigureAwait(false);
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
    /// Updates the specified Large Language Model (LLM).
    /// </summary>
    /// <param name="llm">The updated Large Language Model (LLM).</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut(Name = "Update Large Language Model (LLM)")]
    [EndpointDescription("Updates the specified Large Language Model (LLM).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateLlmAsync([Description("The updated Large Language Model (LLM).")] Llm llm, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<Llm>(llm), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Updates the status of the specified Large Language Model (LLM).
    /// </summary>
    /// <param name="llm">The updated Large Language Model (LLM).</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut("status", Name = "Update LLM Status")]
    [EndpointDescription("Updates the status of the specified Large Language Model (LLM).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateLlmStatusAsync([Description("The updated Large Language Model (LLM).")] Llm llm, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<Llm>(llm), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified Large Language Model (LLM).
    /// </summary>
    /// <param name="name">The name of the Large Language Model (LLM) to patch.</param>
    /// <param name="namespace">The namespace the Large Language Model (LLM) to patch belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}", Name = "Patch Large Language Model (LLM)")]
    [EndpointDescription("Patches the specified Large Language Model (LLM).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchLlmAsync([Description("The name of the Large Language Model (LLM) to patch.")] string name, [Description("The namespace the Large Language Model (LLM) to patch belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceCommand<Llm>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified resource's status.
    /// </summary>
    /// <param name="name">The name of the Large Language Model (LLM) to patch the status of.</param>
    /// <param name="namespace">The namespace the Large Language Model (LLM) to patch the status of belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}/status", Name = "Patch LLM Status")]
    [EndpointDescription("Patches the status of the specified Large Language Model (LLM).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchLlmStatusAsync([Description("The name of the Large Language Model (LLM) to patch the status of.")] string name, [Description("The namespace the Large Language Model (LLM) to patch the status of belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceStatusCommand<Llm>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Deletes the Large Language Model (LLM) with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the Large Language Model (LLM) to delete.</param>
    /// <param name="namespace">The namespace the Large Language Model (LLM) to delete belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpDelete("{namespace}/{name}", Name = "Delete Large Language Model (LLM)")]
    [EndpointDescription("Deletes the specified Large Language Model (LLM).")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> DeleteLlmAsync([Description("The name of the Large Language Model (LLM) to delete.")] string name, [Description("The namespace the Large Language Model (LLM) to delete belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new DeleteResourceCommand<Llm>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

}

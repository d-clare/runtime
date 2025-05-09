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
/// Represents the controller used to manage embedding models.
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonOptions">The service used to access the current <see cref="JsonOptions"/></param>
[Route("api/embedding"), ApiController]
public class EmbeddingModelsController(IMediator mediator, IJsonSerializer jsonSerializer)
    : ResourceControllerBase(mediator, jsonSerializer)
{

    /// <summary>
    /// Gets the definition of the managed embedding model.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("definition", Name = "Get Embedding Model Definition")]
    [EndpointDescription("Gets the definition of the managed embedding model embedding model.")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetEmbeddingModelDefinitionAsync(CancellationToken cancellationToken = default)
    {
        return this.Process(await Mediator.ExecuteAsync(new GetResourceDefinitionQuery<EmbeddingModel>(), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Creates a new embedding model.
    /// </summary>
    /// <param name="embeddingModel">The embedding model to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPost(Name = "Create Embedding Model")]
    [EndpointDescription("Creates a new embedding model.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> CreateEmbeddingModelAsync([FromBody, Description("The embedding model to create.")] EmbeddingModel embeddingModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new CreateResourceCommand<EmbeddingModel>(embeddingModel), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Gets the embedding model with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the embedding model to get.</param>
    /// <param name="namespace">The namespace the embedding model to get belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/{name}", Name = "Get Embedding Model")]
    [EndpointDescription("Gets the embedding model with the specified name and namespace.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> GetEmbeddingModelAsync([Description("The name of the embedding model to get.")] string name, [Description("The namespace the embedding model to get belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new GetResourceQuery<EmbeddingModel>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Enumerates matching embedding models.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet(Name = "Get Embedding Models")]
    [EndpointDescription("Enumerates matching embedding models.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetEmbeddingModelsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<EmbeddingModel>(null, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching embedding models.
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}", Name = "Get Embedding Models By Namespace")]
    [EndpointDescription("Enumerates the matching embedding models belonging to the specified namespace.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetEmbeddingModelsAsync([Description("The namespace the embedding models to enumerate belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<EmbeddingModel>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching embedding models.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("list", Name = "List Embedding Models")]
    [EndpointDescription("List matching embedding models.")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListEmbeddingModelsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<EmbeddingModel>(null, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching embedding models.
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/list", Name = "List Embedding Models By Namespace")]
    [EndpointDescription("List the matching embedding models belonging to the specified namespace..")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListEmbeddingModelsAsync([Description("The namespace the embedding models to list belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<EmbeddingModel>(@namespace, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Watches matching embedding models.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("watch", Name = "Watch Embedding Models")]
    [EndpointDescription("Watches matching embedding models using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchEmbeddingModelsAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<EmbeddingModel>(null, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Watches matching embedding models.
    /// </summary>
    /// <param name="namespace">The namespace the embedding models to watch belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/watch", Name = "Watch Embedding Models By Namespace")]
    [EndpointDescription("Watches the matching embedding models belonging to the specified namespace using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchEmbeddingModelsAsync([Description("The namespace the embedding models to watch belongs to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<EmbeddingModel>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Monitors a specific embedding model.
    /// </summary>
    /// <param name="namespace">The namespace the embedding model to monitor belongs to.</param>
    /// <param name="name">The name of the embedding model to monitor.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/{name}/monitor", Name = "Monitor Embedding Model")]
    [EndpointDescription("Monitors a specific embedding model using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task MonitorEmbeddingModelAsync([Description("The name of the embedding model to monitor.")] string name, [Description("The namespace the embedding model to monitor belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.ExecuteAsync(new MonitorResourceQuery<EmbeddingModel>(name, @namespace), cancellationToken).ConfigureAwait(false);
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
    /// Updates the specified embedding model.
    /// </summary>
    /// <param name="embeddingModel">The updated embedding model.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut(Name = "Update Embedding Model")]
    [EndpointDescription("Updates the specified embedding model.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateEmbeddingModelAsync([Description("The updated embedding model.")] EmbeddingModel embeddingModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<EmbeddingModel>(embeddingModel), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Updates the status of the specified embedding model.
    /// </summary>
    /// <param name="embeddingModel">The updated embedding model.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut("status", Name = "Update Embedding Model Status")]
    [EndpointDescription("Updates the status of the specified embedding model.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateEmbeddingModelStatusAsync([Description("The updated embedding model.")] EmbeddingModel embeddingModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<EmbeddingModel>(embeddingModel), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified embedding model.
    /// </summary>
    /// <param name="name">The name of the embedding model to patch.</param>
    /// <param name="namespace">The namespace the embedding model to patch belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}", Name = "Patch Embedding Model")]
    [EndpointDescription("Patches the specified embedding model.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchEmbeddingModelAsync([Description("The name of the embedding model to patch.")] string name, [Description("The namespace the embedding model to patch belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceCommand<EmbeddingModel>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified resource's status.
    /// </summary>
    /// <param name="name">The name of the embedding model to patch the status of.</param>
    /// <param name="namespace">The namespace the embedding model to patch the status of belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}/status", Name = "Patch Embedding Model Status")]
    [EndpointDescription("Patches the status of the specified embedding model.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchEmbeddingModelStatusAsync([Description("The name of the embedding model to patch the status of.")] string name, [Description("The namespace the embedding model to patch the status of belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceStatusCommand<EmbeddingModel>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Deletes the embedding model with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the embedding model to delete.</param>
    /// <param name="namespace">The namespace the embedding model to delete belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpDelete("{namespace}/{name}", Name = "Delete Embedding Model")]
    [EndpointDescription("Deletes the specified embedding model.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> DeleteEmbeddingModelAsync([Description("The name of the embedding model to delete.")] string name, [Description("The namespace the embedding model to delete belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new DeleteResourceCommand<EmbeddingModel>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

}

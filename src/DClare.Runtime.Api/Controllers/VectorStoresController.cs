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
using DClare.Runtime.Integration.Commands.VectorStores;
using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage vector stores
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonOptions">The service used to access the current <see cref="JsonOptions"/></param>
[Route("api/vector-stores"), ApiController]
public class VectorStoresController(IMediator mediator, IJsonSerializer jsonSerializer)
    : ResourceControllerBase(mediator, jsonSerializer)
{

    /// <summary>
    /// Gets the definition of the managed vector store.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("definition", Name = "Get Vector Store Definition")]
    [EndpointDescription("Gets the definition of the managed vector store vector store.")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetVectorStoreDefinitionAsync(CancellationToken cancellationToken = default)
    {
        return this.Process(await Mediator.ExecuteAsync(new GetResourceDefinitionQuery<VectorStore>(), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Creates a new vector store.
    /// </summary>
    /// <param name="vectorStore">The vector store to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPost(Name = "Create Vector Store ")]
    [EndpointDescription("Creates a new vector store.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> CreateVectorStoreAsync([FromBody, Description("The vector store to create.")] VectorStore vectorStore, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new CreateResourceCommand<VectorStore>(vectorStore), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Gets the vector store with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the vector store to get.</param>
    /// <param name="namespace">The namespace the vector store to get belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/{name}", Name = "Get Vector Store ")]
    [EndpointDescription("Gets the vector store with the specified name and namespace.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> GetVectorStoreAsync([Description("The name of the vector store to get.")] string name, [Description("The namespace the vector store to get belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new GetResourceQuery<VectorStore>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Enumerates matching vector stores.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet(Name = "Get Vector Stores")]
    [EndpointDescription("Enumerates matching vector stores.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetVectorStoresAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<VectorStore>(null, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching vector stores.
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}", Name = "Get Vector Stores By Namespace")]
    [EndpointDescription("Enumerates the matching vector stores belonging to the specified namespace.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetVectorStoresAsync([Description("The namespace the vector stores to enumerate belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<VectorStore>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching vector stores.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("list", Name = "List Vector Stores")]
    [EndpointDescription("List matching vector stores.")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListVectorStoresAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<VectorStore>(null, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching vector stores.
    /// </summary>
    /// <param name="namespace">The namespace to resources to list belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("{namespace}/list", Name = "List Vector Stores By Namespace")]
    [EndpointDescription("List the matching vector stores belonging to the specified namespace..")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListVectorStoresAsync([Description("The namespace the vector stores to list belong to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<VectorStore>(@namespace, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Watches matching vector stores.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("watch", Name = "Watch Vector Stores")]
    [EndpointDescription("Watches matching vector stores using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchVectorStoresAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<VectorStore>(null, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Watches matching vector stores.
    /// </summary>
    /// <param name="namespace">The namespace the vector stores to watch belong to.</param>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/watch", Name = "Watch Vector Stores By Namespace")]
    [EndpointDescription("Watches the matching vector stores belonging to the specified namespace using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchVectorStoresAsync([Description("The namespace the vector stores to watch belongs to.")] string @namespace, [Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<VectorStore>(@namespace, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Monitors a specific vector store.
    /// </summary>
    /// <param name="namespace">The namespace the vector store to monitor belongs to.</param>
    /// <param name="name">The name of the vector store to monitor.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{namespace}/{name}/monitor", Name = "Monitor Vector Store ")]
    [EndpointDescription("Monitors a specific vector store using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task MonitorVectorStoreAsync([Description("The name of the vector store to monitor.")] string name, [Description("The namespace the vector store to monitor belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.ExecuteAsync(new MonitorResourceQuery<VectorStore>(name, @namespace), cancellationToken).ConfigureAwait(false);
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
    /// Updates the specified vector store.
    /// </summary>
    /// <param name="vectorStore">The updated vector store.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut(Name = "Update Vector Store ")]
    [EndpointDescription("Updates the specified vector store.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateVectorStoreAsync([Description("The updated vector store.")] VectorStore vectorStore, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<VectorStore>(vectorStore), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Updates the status of the specified vector store.
    /// </summary>
    /// <param name="vectorStore">The updated vector store.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPut("status", Name = "Update Vector Store Status")]
    [EndpointDescription("Updates the status of the specified vector store.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> UpdateVectorStoreStatusAsync([Description("The updated vector store.")] VectorStore vectorStore, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UpdateResourceCommand<VectorStore>(vectorStore), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified vector store.
    /// </summary>
    /// <param name="name">The name of the vector store to patch.</param>
    /// <param name="namespace">The namespace the vector store to patch belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}", Name = "Patch Vector Store ")]
    [EndpointDescription("Patches the specified vector store.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchVectorStoreAsync([Description("The name of the vector store to patch.")] string name, [Description("The namespace the vector store to patch belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceCommand<VectorStore>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified vector store's status.
    /// </summary>
    /// <param name="name">The name of the vector store to patch the status of.</param>
    /// <param name="namespace">The namespace the vector store to patch the status of belongs to.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{namespace}/{name}/status", Name = "Patch Vector Store Status")]
    [EndpointDescription("Patches the status of the specified vector store.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchVectorStoreStatusAsync([Description("The name of the vector store to patch the status of.")] string name, [Description("The namespace the vector store to patch the status of belongs to.")] string @namespace, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceStatusCommand<VectorStore>(name, @namespace, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Performs a text-based search on the specified vector store.
    /// </summary>
    /// <param name="namespace">The namespace the vector store to search belongs to.</param>
    /// <param name="name">The name of the vector store to search.</param>
    /// <param name="parameters">The search parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPost("{namespace}/{name}/search/text", Name = "Search Text")]
    [EndpointDescription("Performs a text-based search on the specified vector store.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<SemanticSearchResult>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> SearchTextAsync([Description("The namespace the vector store to search belongs to.")] string @namespace, [Description("The name of vector store to search.")] string name, [FromBody, Description("The search parameters.")] SearchTextCommandParameters parameters, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new SearchTextCommand()
        {
            VectorStore = new()
            {
                Namespace = @namespace,
                Name = name
            },
            Parameters = parameters
        }, cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Performs a text-based search on the specified vector store.
    /// </summary>
    /// <param name="namespace">The namespace the vector store to search belongs to.</param>
    /// <param name="name">The name of the vector store to search.</param>
    /// <param name="parameters">The search parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPost("{namespace}/{name}/upload", Name = "Upload File")]
    [EndpointDescription("Upload a file to the specified vector store.")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> UploadFileAsync([Description("The namespace the vector store to upload a file to belongs to.")] string @namespace, [Description("The name of vector store to upload a file to.")] string name, [FromForm, Description("The upload parameters.")] UploadFileCommandParameters parameters, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new UploadFileCommand()
        {
            VectorStore = new()
            {
                Namespace = @namespace,
                Name = name
            },
            Parameters = parameters
        }, cancellationToken).ConfigureAwait(false), (int)HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Deletes the vector store with the specified name and namespace.
    /// </summary>
    /// <param name="name">The name of the vector store to delete.</param>
    /// <param name="namespace">The namespace the vector store to delete belongs to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpDelete("{namespace}/{name}", Name = "Delete Vector Store")]
    [EndpointDescription("Deletes the specified vector store.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> DeleteVectorStoreAsync([Description("The name of the vector store to delete.")] string name, [Description("The namespace the vector store to delete belongs to.")] string @namespace, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new DeleteResourceCommand<VectorStore>(name, @namespace), cancellationToken).ConfigureAwait(false));
    }

}

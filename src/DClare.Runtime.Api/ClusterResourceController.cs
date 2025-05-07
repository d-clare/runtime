using DClare.Runtime.Integration.Commands.Resources;
using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Api;

/// <summary>
/// Represents the base class of a <see cref="Controller"/> used to manage cluster <see cref="IResource"/>s.
/// </summary>
/// <typeparam name="TResource">The type of <see cref="IResource"/> to manage.</typeparam>
/// <param name="mediator">The service used to mediate calls.</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON.</param>
public abstract class ClusterResourceController<TResource>(IMediator mediator, IJsonSerializer jsonSerializer)
    : ResourceController<TResource>(mediator, jsonSerializer)
    where TResource : class, IResource, new()
{

    /// <summary>
    /// Gets the resource with the specified name.
    /// </summary>
    /// <param name="name">The name of the resource to get.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/>.</returns>
    [HttpGet("{name}", Name = "Get Cluster Resource")]
    [EndpointDescription("Gets the resource with the specified name.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> GetResourceAsync([Description("The name of the resource to get.")] string name, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new GetResourceQuery<TResource>(name, null), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching resources.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet(Name = "Get Cluster Resources")]
    [EndpointDescription("Enumerates matching resources.")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> GetResourcesAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new GetResourcesQuery<TResource>(null, labelSelectors), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Lists matching resources.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="maxResults">The maximum amount, if any, of results to list at once.</param>
    /// <param name="continuationToken">A token, defined by a previously retrieved collection, used to continue enumerating through matches.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpGet("list", Name = "List Cluster Resources")]
    [EndpointDescription("Lists matching resources.")]
    [ProducesResponseType(typeof(Collection<Resource>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> ListResourcesAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, [Description("The maximum amount, if any, of results to list at once.")] ulong? maxResults = null, [Description("A token, defined by a previously retrieved collection, used to continue enumerating through matches.")] string ? continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors)) return InvalidLabelSelector(labelSelector!);
        return this.Process(await Mediator.ExecuteAsync(new ListResourcesQuery<TResource>(null, labelSelectors, maxResults, continuationToken), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Watches matching resources.
    /// </summary>
    /// <param name="labelSelector">A comma-separated list of label selectors, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("watch", Name = "Watch Cluster Resources")]
    [EndpointDescription("Watches matching resources using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task WatchResourcesAsync([Description("A comma-separated list of label selectors, if any.")] string? labelSelector = null, CancellationToken cancellationToken = default)
    {
        if (!TryParseLabelSelectors(labelSelector, out var labelSelectors))
        {
            await WriteInvalidLabelSelectorResponseAsync(labelSelector!, cancellationToken).ConfigureAwait(false);
            return;
        }
        var response = await Mediator.ExecuteAsync(new WatchResourcesQuery<TResource>(null, labelSelectors), cancellationToken).ConfigureAwait(false);
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
    /// Monitors a specific resource.
    /// </summary>
    /// <param name="name">The name of the cluster resource to monitor.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    [HttpGet("{name}/monitor", Name = "Monitor Cluster Resource")]
    [EndpointDescription("Monitors a specific resource using Server Sent Events (SSE).")]
    [ProducesResponseType(typeof(IAsyncEnumerable<ResourceWatchEvent>), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task MonitorResourceAsync([Description("The name of the resource to monitor.")] string name, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.ExecuteAsync(new MonitorResourceQuery<TResource>(name, null), cancellationToken).ConfigureAwait(false);
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
    /// Patches the specified resource.
    /// </summary>
    /// <param name="name">The name of the resource to patch.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{name}", Name = "Patch Cluster Resource")]
    [Description("Patches the specified resource.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchResourceAsync([Description("The name of the resource to patch.")] string name, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceCommand<TResource>(name, null, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Patches the specified resource's status.
    /// </summary>
    /// <param name="name">The name of the resource to patch the status of.</param>
    /// <param name="patch">The patch to apply.</param>
    /// <param name="resourceVersion">The expected resource version, if any, used for optimistic concurrency.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpPatch("{name}/status", Name = "Patch Cluster Resource Status")]
    [Description("Patches the specified resource's status.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public virtual async Task<IActionResult> PatchResourceStatusAsync([Description("The name of the resource to patch.")] string name, [FromBody, Description("The patch to apply.")] Patch patch, [Description("The expected resource version, if any, used for optimistic concurrency.")] string? resourceVersion = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new PatchResourceStatusCommand<TResource>(name, null, patch, resourceVersion), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Deletes the resource with the specified name.
    /// </summary>
    /// <param name="name">The name of the resource to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    [HttpDelete("{name}", Name = "Delete Cluster Resource")]
    [Description("Deletes the resource with the specified name.")]
    [ProducesResponseType(typeof(Resource), (int)HttpStatusCode.Created)]
    [ProducesErrorResponseType(typeof(Neuroglia.ProblemDetails))]
    public async Task<IActionResult> DeleteResourceAsync([Description("The name of the resource to delete.")] string name, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return this.Process(await Mediator.ExecuteAsync(new DeleteResourceCommand<TResource>(name, null), cancellationToken).ConfigureAwait(false));
    }

}

using DClare.Runtime.Integration.Commands.Resources;
using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Api;

/// <summary>
/// Represents the base class of a <see cref="Controller"/> used to manage <see cref="IResource"/>s.
/// </summary>
/// <param name="mediator">The service used to mediate calls.</param>
/// <param name="jsonSerializer">The service used tro serialize/deserialize data to/from JSON.</param>
public abstract class ResourceControllerBase(IMediator mediator, IJsonSerializer jsonSerializer)
    : Controller
{

    /// <summary>
    /// Gets the service used to mediate calls.
    /// </summary>
    protected IMediator Mediator { get; } = mediator;

    /// <summary>
    /// Gets the service used tro serialize/deserialize data to/from JSON.
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <summary>
    /// Parses the specified string into a new <see cref="List{T}"/> of <see cref="LabelSelector"/>s.
    /// </summary>
    /// <param name="labelSelector">The string to parse.</param>
    /// <param name="labelSelectors">A new <see cref="List{T}"/> containing the parsed <see cref="LabelSelector"/>s.</param>
    /// <returns>A boolean indicating whether or not the input could be parse.</returns>
    protected virtual bool TryParseLabelSelectors(string? labelSelector, out IEnumerable<LabelSelector>? labelSelectors)
    {
        labelSelectors = null;
        try
        {
            if (!string.IsNullOrWhiteSpace(labelSelector)) labelSelectors = LabelSelector.ParseList(labelSelector);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Creates a new <see cref="IActionResult"/> that describes an error while parsing the request's label selector.
    /// </summary>
    /// <param name="labelSelector">The invalid label selector.</param>
    /// <returns>A new <see cref="IActionResult"/> used to describe the action's result.</returns>
    protected virtual IActionResult InvalidLabelSelector(string labelSelector)
    {
        ModelState.AddModelError(nameof(labelSelector), $"The specified value '{labelSelector}' is not a valid comma-separated label selector list");
        return ValidationProblem("Bad Request", statusCode: (int)HttpStatusCode.BadRequest, title: "Bad Request", modelStateDictionary: ModelState);
    }

    /// <summary>
    /// Writes to the response the description of an error that occurred while parsing the request's label selector.
    /// </summary>
    /// <param name="labelSelector">The invalid label selector.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task WriteInvalidLabelSelectorResponseAsync(string labelSelector, CancellationToken cancellationToken)
    {
        var problem = Problems.InvalidLabelSelector(labelSelector);
        var json = JsonSerializer.SerializeToText(problem);
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        Response.ContentType = MediaTypeNames.Application.Json;
        await Response.WriteAsync(json, cancellationToken).ConfigureAwait(false);
    }

}

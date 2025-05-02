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

using DClare.Runtime.Integration.Commands.Chats;
using DClare.Runtime.Integration.Queries.Chats;

namespace DClare.Runtime.Api.Controllers;

/// <summary>
/// Represents the controller used to manage chats
/// </summary>
/// <param name="mediator">The service used to mediate calls</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonOptions">The service used to access the current <see cref="JsonOptions"/></param>
[Route("api/[controller]"), ApiController]
public class ChatsController(IMediator mediator, IJsonSerializer jsonSerializer, IOptions<JsonOptions> jsonOptions)
    : Controller
{

    /// <summary>
    /// Gets the specified chat belonging to the current user
    /// </summary>
    /// <param name="id">The user-defined id of the chat to get</param>
    /// <param name="agent">The name of the agent the chat to get concerns</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpGet(Name = "Get Chat")]
    [EndpointDescription("Gets the chat with the specified key")]
    [ProducesResponseType(typeof(Chat), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetChatAsync([Description("The user-defined id of the chat to get")] string id, [Description("The name of the agent the chat to get concerns")] string agent, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new GetChatQuery()
        {
            Id = id,
            Agent = agent
        }, cancellationToken).ConfigureAwait(false);
        return this.Process(result);
    }

    /// <summary>
    /// Gets the specified chat
    /// </summary>
    /// <param name="key">The unique key of the chat to get</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpGet("{key}", Name = "Get Chat By Key")]
    [EndpointDescription("Gets the chat with the specified key")]
    [ProducesResponseType(typeof(Chat), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetChatByKeyAsync([Description("The unique key of the chat to get")] string key, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new GetChatByKeyQuery() 
        { 
            Key = key
        }, cancellationToken).ConfigureAwait(false);
        return this.Process(result);
    }

    /// <summary>
    /// Lists all the chats belonging to the current user
    /// </summary>
    /// <param name="agent">The name of the agent, if any, the chats to list belong to</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpGet("list", Name = "List Chats")]
    [EndpointDescription("Lists the chats belonging to the current user")]
    [ProducesResponseType(typeof(IEnumerable<Chat>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ListChatsAsync([Description("The name of the agent the chats to list apply to")] string? agent = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new ListChatsQuery()
        {
            Agent = agent
        }, cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess()) return this.Process(result);
        return this.Ok(await result.Data!.ToListAsync(cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Streams all the chats belonging to the current user
    /// </summary>
    /// <param name="agent">The name of the agent, if any, the chats to stream belong to</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpGet("stream", Name = "Streams Chats")]
    [EndpointDescription("Streams the chats belonging to the current user")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Chat>), (int)HttpStatusCode.OK)]
    public async Task StreamChatsAsync([Description("The name of the agent the chats to list apply to")] string? agent = null, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await System.Text.Json.JsonSerializer.SerializeAsync(Response.Body, ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState), jsonOptions.Value.JsonSerializerOptions, cancellationToken);
            return;
        }
        var result = await mediator.ExecuteAsync(new ListChatsQuery()
        {
            Agent = agent
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
        await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await foreach (var e in result.Data!.WithCancellation(cancellationToken))
            {
                var sseMessage = $"data: {jsonSerializer.SerializeToText(e)}\n\n";
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), cancellationToken).ConfigureAwait(false);
                await Response.Body.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException) { }
    }

    /// <summary>
    /// Renames the specified chat
    /// </summary>
    /// <param name="key">The unique key of the chat to rename</param>
    /// <param name="command">The command to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpPut("{key}/name", Name = "Rename Chat")]
    [EndpointDescription("Renames the chat with the specified key")]
    [ProducesResponseType(typeof(Chat), (int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> RenameChatAsync([Description("The unique key of the chat to rename")] string key, RenameChatCommand command, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        command.Key = key;
        var result = await mediator.ExecuteAsync(command, cancellationToken).ConfigureAwait(false);
        return this.Process(result, (int)HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Deletes the specified chat
    /// </summary>
    /// <param name="key">The unique key of the chat to delete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/> that describes the result of the operation</returns>
    [HttpDelete("{key}", Name = "Delete Chat")]
    [EndpointDescription("Deletes the chat with the specified key")]
    [ProducesResponseType(typeof(Chat), (int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteChatAsync([Description("The unique key of the chat to delete")] string key, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await mediator.ExecuteAsync(new DeleteChatCommand()
        {
            Key = key
        }, cancellationToken).ConfigureAwait(false);
        return this.Process(result, (int)HttpStatusCode.NoContent);
    }

}
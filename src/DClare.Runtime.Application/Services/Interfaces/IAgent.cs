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

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Defines the fundamentals of an AI Agent.
/// </summary>
public interface IAgent
{

    /// <summary>
    /// Gets the agent's name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the agent's description.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets a name/definition mapping of the agent's skills.
    /// </summary>
    IReadOnlyDictionary<string, AgentSkillDefinition> Skills { get; }

    /// <summary>
    /// Invokes the agent with the provided user message, optionally scoped to a session, and yields a stream of responses.
    /// </summary>
    /// <param name="message">The input message that the agent should respond to.</param>
    /// <param name="options">The options, if any, used to configure the agent's invocation.</param>
    /// <param name="cancellationToken">Token to cancel the streaming operation.</param>
    /// <returns>A new <see cref="ChatResponse"/> that describes the result of the invocation.</returns>
    Task<ChatResponse> InvokeAsync(string message, AgentInvocationOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invokes the agent with the provided user message, optionally scoped to a session, and yields a stream of response content.
    /// </summary>
    /// <param name="message">The input message that the agent should respond to.</param>
    /// <param name="options">The options, if any, used to configure the agent's invocation.</param>
    /// <param name="cancellationToken">Token to cancel the streaming operation.</param>
    /// <returns>A new <see cref="ChatResponseStream"/> that describes the result of the invocation.</returns>
    Task<ChatResponseStream> InvokeStreamingAsync(string message, AgentInvocationOptions? options = null, CancellationToken cancellationToken = default);

}

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

namespace DClare.Runtime.Integration.Commands.Agents;

/// <summary>
/// Represents the command to invoke an AI Agent with a message, optionally within a conversational session
/// </summary>
public class InvokeAgentCommand
    : Command<ChatResponseStream>
{

    /// <summary>
    /// Gets/sets the name of the agent to invoke
    /// </summary>
    [Required, MinLength(1)]
    public virtual string Agent { get; set; } = null!;

    /// <summary>
    /// Gets/sets the input message to process
    /// </summary>
    [Required, MinLength(1)]
    public virtual string Message { get; set; } = null!;

    /// <summary>
    /// Gets/sets the session identifier, if any, to maintain conversational context across invocations
    /// </summary>
    public virtual string? SessionId { get; set; }

    /// <summary>
    /// Gets/sets a key/value mapping of the invocation's parameters, if any
    /// </summary>
    public virtual IDictionary<string, object>? Parameters { get; set; }

}

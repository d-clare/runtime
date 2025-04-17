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

using DClare.Runtime.Integration.Commands.Processes;

namespace DClare.Runtime.Application.Commands.Processes;

/// <summary>
/// Represents the service used to process <see cref="InvokeProcessCommand"/>s
/// </summary>
public class InvokeProcessCommandHandler(IOptions<ApplicationOptions> options, IAgenticProcessFactory processFactory)
    : ICommandHandler<InvokeProcessCommand, ChatResponseStream>
{

    /// <inheritdoc/>
    public async Task<IOperationResult<ChatResponseStream>> HandleAsync(InvokeProcessCommand command, CancellationToken cancellationToken = default)
    {
        if (options.Value.Components == null || options.Value.Components.Processes == null || !options.Value.Components.Processes.TryGetValue(command.Process, out var processDefinition) || processDefinition == null) throw new ProblemDetailsException(Problems.AgenticProcessNotFound(command.Process));
        var process = await processFactory.CreateAsync(processDefinition, options.Value.Components, cancellationToken).ConfigureAwait(false);
        var response = await process.InvokeStreamingAsync(command.Message, command.SessionId, cancellationToken).ConfigureAwait(false);
        return this.Ok(response);
    }

}

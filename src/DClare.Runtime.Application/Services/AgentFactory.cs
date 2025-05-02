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

using A2A.Client;
using A2A.Client.Configuration;
using A2A.Client.Services;
using Neuroglia.Data.PatchModel.Services;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAgentFactory"/> interface
/// </summary>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
/// <param name="kernelFactory">The service used to create <see cref="Kernel"/>s</param>
/// <param name="httpClient">The service used to perform HTTP requests</param>
public class AgentFactory(IServiceProvider serviceProvider, IKernelFactory kernelFactory, HttpClient httpClient)
    : IAgentFactory
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the service used to create <see cref="Kernel"/>s
    /// </summary>
    protected IKernelFactory KernelFactory { get; } = kernelFactory;

    /// <summary>
    /// Gets the service used to perform HTTP requests
    /// </summary>
    protected HttpClient HttpClient { get; } = httpClient;

    /// <inheritdoc/>
    public virtual Task<IAgent> CreateAsync(string name, AgentDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        var agentDefinition = definition;
        if (!string.IsNullOrWhiteSpace(definition.Use))
        {
            if (components == null || components.Agents == null || !components.Agents.TryGetValue(definition.Use, out agentDefinition) || agentDefinition == null) throw new ProblemDetailsException(Problems.AgentNotFound(definition.Use));
        }
        return agentDefinition.Type switch
        {
            AgentType.Hosted => CreateAgentAsync(name, definition.Hosted!, components, cancellationToken),
            AgentType.Remote => CreateAgentProxyAsync(name, definition.Remote!, components, cancellationToken),
            _ => throw new NotSupportedException($"The specified agent type '{agentDefinition.Type}' is not supported")
        };
    }

    /// <summary>
    /// Creates a new <see cref="Agent"/>
    /// </summary>
    /// <param name="name">The agent's name</param>
    /// <param name="definition">The agent's definition</param>
    /// <param name="components">A collection, if any, containing the reusable components potentially referenced by the <see cref="IAgent"/> to create</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAgent"/></returns>
    protected virtual async Task<IAgent> CreateAgentAsync(string name, HostedAgentDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        var agentDefinition = definition;
        if (!string.IsNullOrWhiteSpace(definition.Extends))
        {
            if (components == null || components.Agents == null || !components.Agents.TryGetValue(definition.Extends, out var extendedAgentDefinition) || extendedAgentDefinition == null) throw new ProblemDetailsException(Problems.AgentNotFound(definition.Extends));
            var patchHandler = new JsonMergePatchHandler();
            agentDefinition = (await patchHandler.ApplyPatchAsync(extendedAgentDefinition, agentDefinition, cancellationToken).ConfigureAwait(false))!;
        }
        var kernel = await KernelFactory.CreateAsync(agentDefinition.Kernel, components, cancellationToken).ConfigureAwait(false);
        return ActivatorUtilities.CreateInstance<Agent>(ServiceProvider, name, agentDefinition, kernel);
    }

    /// <summary>
    /// Creates a new remote <see cref="IAgent"/>
    /// </summary>
    /// <param name="name">The agent's name</param>
    /// <param name="definition">The agent's definition</param>
    /// <param name="components">A collection, if any, containing the reusable components potentially referenced by the <see cref="IAgent"/> to create</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAgent"/></returns>
    protected virtual Task<IAgent> CreateAgentProxyAsync(string name, RemoteAgentDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        return definition.Channel.Type switch
        {
            AgentCommunicationChannelType.A2A => CreateRemoteA2AAgentAsync(name, definition, components, cancellationToken),
            _ => throw new NotSupportedException($"The specified channel type '{definition.Channel.Type}' is not supported")
        };
        
    }

    protected virtual async Task<IAgent> CreateRemoteA2AAgentAsync(string name, RemoteAgentDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        if (definition.Channel.A2A == null) throw new ProblemDetailsException(Problems.InvalidConfiguration(name));
        var discoveryDocument = await HttpClient.GetA2ADiscoveryDocumentAsync(definition.Channel.A2A.Endpoint.Uri, cancellationToken).ConfigureAwait(false);
        var manifest = discoveryDocument.Agents.Single();
        var a2aClientOptions = Options.Create(new A2AProtocolClientOptions()
        {
            Endpoint = definition.Channel.A2A.Endpoint.Uri
        });
        var a2aClient = ActivatorUtilities.CreateInstance<A2AProtocolHttpClient>(ServiceProvider, HttpClient, a2aClientOptions);
        return ActivatorUtilities.CreateInstance<A2AAgentProxy>(ServiceProvider, name, definition, manifest, a2aClient);
    }

}
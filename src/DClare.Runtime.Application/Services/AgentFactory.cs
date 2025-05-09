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

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAgentFactory"/> interface.
/// </summary>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/>.</param>
/// <param name="componentDefinitionResolver">The service used to resolve <see cref="ReferenceableComponentDefinition"/>s.</param>
/// <param name="kernelFactory">The service used to create <see cref="Kernel"/>s.</param>
/// <param name="httpClient">The service used to perform HTTP requests.</param>
public class AgentFactory(IServiceProvider serviceProvider, IComponentDefinitionResolver componentDefinitionResolver, IKernelFactory kernelFactory, HttpClient httpClient)
    : IAgentFactory
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the service used to resolve <see cref="ReferenceableComponentDefinition"/>s.
    /// </summary>
    protected IComponentDefinitionResolver ComponentDefinitionResolver { get; } = componentDefinitionResolver;

    /// <summary>
    /// Gets the service used to create <see cref="Kernel"/>s.
    /// </summary>
    protected IKernelFactory KernelFactory { get; } = kernelFactory;

    /// <summary>
    /// Gets the service used to perform HTTP requests.
    /// </summary>
    protected HttpClient HttpClient { get; } = httpClient;

    /// <inheritdoc/>
    public virtual async Task<IAgent> CreateAsync(string name, AgentDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        var agentDefinition = definition;
        if(!string.IsNullOrWhiteSpace(agentDefinition.Use)) agentDefinition = await ComponentDefinitionResolver.ResolveAsync<AgentDefinition>(agentDefinition.Use, context, cancellationToken).ConfigureAwait(false);
        return agentDefinition.Type switch
        {
            AgentType.Hosted => await CreateHostedAgentAsync(name, agentDefinition.Hosted!, context, cancellationToken).ConfigureAwait(false),
            AgentType.Remote => await CreateAgentProxyAsync(name, agentDefinition.Remote!, context, cancellationToken).ConfigureAwait(false),
            _ => throw new NotSupportedException($"The specified agent type '{agentDefinition.Type}' is not supported")
        };
    }

    /// <summary>
    /// Creates a new hosted <see cref="IAgent"/> based on the specified name and definition.
    /// </summary>
    /// <param name="name">The name of the agent to create.</param>
    /// <param name="definition">The definition describing the hosted agent and its configuration.</param>
    /// <param name="context"> An optional <see cref="ComponentResolutionContext"/> specifying the scope and available components to use during resolution.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAgent"/>.</returns>
    protected virtual async Task<IAgent> CreateHostedAgentAsync(string name, HostedAgentDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        var kernelDefinition = new KernelDefinition()
        {
            Llm = definition.Llm,
            Knowledge = definition.Knowledge,
            Toolsets = definition.Toolsets
        };
        var kernel = await KernelFactory.CreateAsync(kernelDefinition, context, cancellationToken).ConfigureAwait(false);
        return ActivatorUtilities.CreateInstance<HostedAgent>(ServiceProvider, name, definition, kernel, context ?? new() { Scope = ComponentScope.Global });
    }

    /// <summary>
    /// Creates a new <see cref="IAgent"/> proxy based on the specified name and definition.
    /// </summary>
    /// <param name="name">The name of the agent to create.</param>
    /// <param name="definition">The definition describing the agent proxy and its configuration.</param>
    /// <param name="context"> An optional <see cref="ComponentResolutionContext"/> specifying the scope and available components to use during resolution.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAgent"/>.</returns>
    protected virtual Task<IAgent> CreateAgentProxyAsync(string name, RemoteAgentDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        return definition.Channel.Type switch
        {
            AgentCommunicationChannelType.A2A => CreateA2AAgentProxyAsync(name, definition, context, cancellationToken),
            _ => throw new NotSupportedException($"The specified channel type '{definition.Channel.Type}' is not supported")
        };
    }

    /// <summary>
    /// Creates a new A2A <see cref="IAgent"/> proxy based on the specified name and definition.
    /// </summary>
    /// <param name="name">The name of the agent to create.</param>
    /// <param name="definition">The definition describing the agent proxy and its configuration.</param>
    /// <param name="context"> An optional <see cref="ComponentResolutionContext"/> specifying the scope and available components to use during resolution.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAgent"/>.</returns>
    protected virtual async Task<IAgent> CreateA2AAgentProxyAsync(string name, RemoteAgentDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        if (definition.Channel.A2A == null) throw new ProblemDetailsException(Problems.InvalidConfiguration(name));
        var endpointUri = definition.Channel.A2A.Endpoint.T1Value?.Uri ?? definition.Channel.A2A.Endpoint.T2Value!;
        var discoveryDocument = await HttpClient.GetA2ADiscoveryDocumentAsync(endpointUri, cancellationToken).ConfigureAwait(false);
        var manifest = discoveryDocument.Agents.Single();
        var a2aClientOptions = Options.Create(new A2AProtocolClientOptions()
        {
            Endpoint = endpointUri
        });
        var a2aClient = ActivatorUtilities.CreateInstance<A2AProtocolHttpClient>(ServiceProvider, HttpClient, a2aClientOptions);
        return ActivatorUtilities.CreateInstance<A2AAgentProxy>(ServiceProvider, name, definition, manifest, a2aClient);
    }

}

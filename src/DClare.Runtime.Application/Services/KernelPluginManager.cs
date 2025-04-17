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

using McpDotNet.Client;
using McpDotNet.Configuration;
using Microsoft.SemanticKernel.Plugins.OpenApi;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IKernelPluginManager"/> interface
/// </summary>
public class KernelPluginManager
    : IKernelPluginManager
{

    /// <summary>
    /// Gets a list containing all loaded <see cref="KernelPlugin"/>s
    /// </summary>
    protected List<KernelPlugin> Plugins { get; } = [];

    /// <inheritdoc/>
    public virtual async Task<KernelPlugin> GetOrLoadAsync(string name, ToolsetDefinition definition, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(definition);
        return definition.Type switch
        {
            ToolsetType.Mcp => await LoadMcpPluginAsync(name, definition, cancellationToken).ConfigureAwait(false),
            ToolsetType.OpenApi => await LoadOpenApiPluginAsync(name, definition, cancellationToken).ConfigureAwait(false),
            _ => throw new NotSupportedException($"The specified toolset type '{definition.Type}' is not supported")
        };
    }

    /// <summary>
    /// Loads the specified MCP plugin
    /// </summary>
    /// <param name="name">The plugin's name</param>
    /// <param name="definition">The plugin's definition</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The loaded <see cref="KernelPlugin"/></returns>
    protected virtual async Task<KernelPlugin> LoadMcpPluginAsync(string name, ToolsetDefinition definition, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(definition);
        if (definition.Mcp == null) throw new NullReferenceException($"The '{nameof(definition.Mcp)}' property must be configured for a toolset of type '{ToolsetType.Mcp}'");
        var serverConfig = new McpServerConfig()
        {
            Id = name.ToCamelCase(),
            Name = name,
            TransportType = definition.Mcp.Transport.Type,
            TransportOptions = definition.Mcp.Transport.Options == null ? null : new(definition.Mcp.Transport.Options),
            Location = definition.Mcp.Transport.Type switch
            {
                McpTransportType.Http => definition.Mcp.Transport.Http!.Endpoint.Uri.OriginalString,
                McpTransportType.Stdio => definition.Mcp.Transport.Stdio!.Command,
                _ => throw new NotSupportedException()
            },
            Arguments = definition.Mcp.Transport.Stdio?.Arguments?.ToArray()
        };
        var clientOptions = new McpClientOptions()
        {
            ClientInfo = new()
            {
                Name = definition.Mcp.Client.Implementation.Name,
                Version = definition.Mcp.Client.Implementation.Version
            },
            ProtocolVersion = definition.Mcp.Client.ProtocolVersion,
            InitializationTimeout = definition.Mcp.Client.Timeout?.ToTimeSpan() ?? TimeSpan.FromSeconds(60)
        };
        await using var mcpClient = await McpClientFactory.CreateAsync(serverConfig, clientOptions, cancellationToken: cancellationToken).ConfigureAwait(false);
        var tools = await mcpClient.GetAIFunctionsAsync(cancellationToken);
        return Plugins.AddFromFunctions(name, tools.Select(t => t.AsKernelFunction()));
    }

    /// <summary>
    /// Loads the specified OpenAPI plugin
    /// </summary>
    /// <param name="name">The plugin's name</param>
    /// <param name="definition">The plugin's definition</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The loaded <see cref="KernelPlugin"/></returns>
    protected virtual async Task<KernelPlugin> LoadOpenApiPluginAsync(string name, ToolsetDefinition definition, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(definition);
        if (definition.OpenApi == null) throw new NullReferenceException($"The '{nameof(definition.OpenApi)}' property must be configured for a toolset of type '{ToolsetType.OpenApi}'");
        var plugin = (await OpenApiKernelPluginFactory.CreateFromOpenApiAsync(name, definition.OpenApi.Document.Endpoint.Uri, cancellationToken: cancellationToken).ConfigureAwait(false))!;
        Plugins.Add(plugin);
        return plugin;
    }

}

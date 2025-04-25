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

using Microsoft.SemanticKernel.Plugins.OpenApi;
using ModelContextProtocol.SemanticKernel.Extensions;
using ModelContextProtocol.SemanticKernel.Options;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IKernelPluginManager"/> interface
/// </summary>
/// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
public class KernelPluginManager(ILoggerFactory loggerFactory)
    : IKernelPluginManager
{

    /// <summary>
    /// Gets the service used to create <see cref="ILogger"/>s
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; } = loggerFactory;

    /// <summary>
    /// Gets a list containing all loaded <see cref="KernelPlugin"/>s
    /// </summary>
    protected KernelPluginCollection Plugins { get; } = [];

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
        return definition.Mcp.Transport.Type switch
        {
            McpTransportType.Http => await Plugins.AddMcpFunctionsFromSseServerAsync(definition.Mcp.Client.Implementation.Name, definition.Mcp.Transport.Http!.Endpoint.Uri, LoggerFactory, cancellationToken).ConfigureAwait(false),
            McpTransportType.Stdio => await Plugins.AddMcpFunctionsFromStdioServerAsync(definition.Mcp.Client.Implementation.Name, definition.Mcp.Transport.Stdio!.Command, definition.Mcp.Transport.Stdio.Arguments, null, LoggerFactory, cancellationToken).ConfigureAwait(false),
            _ => throw new NotSupportedException($"The specified MCP transport type '{definition.Mcp.Transport.Type}' is not supported")
        };;
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

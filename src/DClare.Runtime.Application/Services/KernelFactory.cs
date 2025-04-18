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

using Neuroglia.Data.PatchModel.Services;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IKernelFactory"/> interface
/// </summary>
/// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
/// <param name="oauth2TokenManager">The service used to manage <see cref="OAuth2Token"/>s</param>
/// <param name="pluginManager">The service used to provide <see cref="KernelPlugin"/>s</param>
public class KernelFactory(ILoggerFactory loggerFactory, IOAuth2TokenManager oauth2TokenManager, IKernelPluginManager pluginManager)
    : IKernelFactory
{

    /// <summary>
    /// Gets the service used to create <see cref="ILogger"/>s
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; } = loggerFactory;

    /// <summary>
    /// Gets the service used to manage <see cref="OAuth2Token"/>s
    /// </summary>
    protected IOAuth2TokenManager OAuth2TokenManager { get; } = oauth2TokenManager;

    /// <summary>
    /// Gets the service used to manage <see cref="KernelPlugin"/>s
    /// </summary>
    protected IKernelPluginManager PluginManager { get; } = pluginManager;

    /// <inheritdoc/>
    public virtual async Task<Kernel> CreateAsync(KernelDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(definition);
        var kernelDefinition = definition;
        if (!string.IsNullOrWhiteSpace(definition.Use))
        {
            if (components == null || components.Kernels == null || !components.Kernels.TryGetValue(definition.Use, out kernelDefinition) || kernelDefinition == null) throw new ProblemDetailsException(Problems.KernelNotFound(definition.Use));
        }
        else if (!string.IsNullOrWhiteSpace(definition.Extends))
        {
            if (components == null || components.Kernels == null || !components.Kernels.TryGetValue(definition.Extends, out var extendedKernelDefinition) || extendedKernelDefinition == null) throw new ProblemDetailsException(Problems.KernelNotFound(definition.Extends));
            var patchHandler = new JsonMergePatchHandler();
            kernelDefinition = (await patchHandler.ApplyPatchAsync(extendedKernelDefinition, kernelDefinition, cancellationToken).ConfigureAwait(false))!;
        }
        var kernelBuilder = Kernel.CreateBuilder();
        if (kernelDefinition.Reasoning != null) kernelBuilder = await BuildReasoningCapabilityAsync(kernelBuilder, kernelDefinition.Reasoning, cancellationToken).ConfigureAwait(false);
        if (kernelDefinition.Embedding != null) kernelBuilder = await BuildEmbeddingCapabilityAsync(kernelBuilder, kernelDefinition.Embedding, cancellationToken).ConfigureAwait(false);
        if (kernelDefinition.Toolsets != null && kernelDefinition.Toolsets.Count > 0)
        {
            foreach (var toolset in kernelDefinition.Toolsets)
            {
                var toolsetName = toolset.Key;
                var toolsetDefinition = toolset.Value;
                if(!string.IsNullOrWhiteSpace(toolsetDefinition.Use))
                {
                    toolsetName = toolsetDefinition.Use;
                    if (components?.Toolsets == null || !components.Toolsets.TryGetValue(toolsetName, out toolsetDefinition) || toolsetDefinition == null) throw new ProblemDetailsException(Problems.ToolsetNotFound(toolsetName));
                }
                kernelBuilder.Plugins.Add(await PluginManager.GetOrLoadAsync(toolsetName, toolsetDefinition, cancellationToken).ConfigureAwait(false));
            }
        }
        return kernelBuilder.Build();
    }

    /// <summary>
    /// Builds and configures the specified reasoning capability
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure</param>
    /// <param name="reasoningCapability">The definition of the reasoning capability to build</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The configured <see cref="IKernelBuilder"/></returns>
    protected virtual async Task<IKernelBuilder> BuildReasoningCapabilityAsync(IKernelBuilder kernelBuilder, ReasoningCapabilityDefinition reasoningCapability, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(reasoningCapability);
        var apiKey = reasoningCapability.Api.Endpoint.Authentication?.Scheme switch
        {
            AuthenticationScheme.ApiKey => reasoningCapability.Api.Endpoint.Authentication.ApiKey!.Key,
            AuthenticationScheme.Bearer => reasoningCapability.Api.Endpoint.Authentication.Bearer!.Token,
            AuthenticationScheme.OAuth2 or AuthenticationScheme.OpenIDConnect => (await OAuth2TokenManager.GetTokenAsync(reasoningCapability.Api.Endpoint.Authentication.OAuth2!, cancellationToken).ConfigureAwait(false))?.AccessToken,
            _ => null
        };
        switch (reasoningCapability.Provider)
        {
            case ReasoningModelProvider.AzureOpenAI:
                if (reasoningCapability.Api.Properties == null || !reasoningCapability.Api.Properties.TryGetValue("deployment", out var value) || value is not string deployment || string.IsNullOrWhiteSpace(deployment)) throw new NullReferenceException($"The 'deployment' API property must be set when using the '{ReasoningModelProvider.AzureOpenAI}' reasoning model provider");
                if (reasoningCapability.Api.Endpoint == null) throw new NullReferenceException($"'{nameof(reasoningCapability)}.{nameof(reasoningCapability.Api)}.{nameof(reasoningCapability.Api.Endpoint)}' must be set when using the '{ReasoningModelProvider.AzureOpenAI}' reasoning model provider");
                if (string.IsNullOrWhiteSpace(apiKey)) throw new NullReferenceException("Failed to resolve the API Key to use");
                kernelBuilder.AddAzureOpenAIChatCompletion(deployment, reasoningCapability.Api.Endpoint.Uri.OriginalString, apiKey, reasoningCapability.Provider, reasoningCapability.Model);
                break;
            case ReasoningModelProvider.Gemini:
                if (string.IsNullOrWhiteSpace(apiKey)) throw new NullReferenceException("Failed to resolve the API Key to use");
                kernelBuilder.AddGoogleAIGeminiChatCompletion(reasoningCapability.Model, apiKey, serviceId: reasoningCapability.Provider);
                break;
            case ReasoningModelProvider.HuggingFace:
                if (reasoningCapability.Api.Endpoint == null) throw new NullReferenceException($"'{nameof(reasoningCapability)}.{nameof(reasoningCapability.Api)}.{nameof(reasoningCapability.Api.Endpoint)}' must be set when using the '{ReasoningModelProvider.HuggingFace}' reasoning model provider");
                kernelBuilder.AddHuggingFaceChatCompletion(reasoningCapability.Api.Endpoint.Uri, apiKey, reasoningCapability.Provider);
                break;
            case ReasoningModelProvider.MistralAI:
                if (string.IsNullOrWhiteSpace(apiKey)) throw new NullReferenceException("Failed to resolve the API Key to use");
                kernelBuilder.AddMistralChatCompletion(reasoningCapability.Model, apiKey, reasoningCapability.Api.Endpoint.Uri, reasoningCapability.Provider);
                break;
            case ReasoningModelProvider.Ollama:
                if (reasoningCapability.Api.Endpoint == null) throw new NullReferenceException($"'{nameof(reasoningCapability)}.{nameof(reasoningCapability.Api)}.{nameof(reasoningCapability.Api.Endpoint)}' must be set when using the '{ReasoningModelProvider.Ollama}' reasoning model provider");
                kernelBuilder.AddOllamaChatCompletion(reasoningCapability.Model, reasoningCapability.Api.Endpoint.Uri, reasoningCapability.Provider);
                break;
            case ReasoningModelProvider.Onnx:
                if (reasoningCapability.Api.Properties == null || !reasoningCapability.Api.Properties.TryGetValue("path", out value) || value is not string path || string.IsNullOrWhiteSpace(path)) throw new NullReferenceException($"The 'path' API property must be set when using the '{ReasoningModelProvider.Onnx}' reasoning model provider");
                kernelBuilder.AddOnnxRuntimeGenAIChatCompletion(reasoningCapability.Model, path, reasoningCapability.Provider);
                break;
            case ReasoningModelProvider.OpenAI:
                var organization = reasoningCapability.Api.Properties == null || !reasoningCapability.Api.Properties.TryGetValue("organization", out value) ? null : value as string;
                if (string.IsNullOrWhiteSpace(apiKey)) throw new NullReferenceException("Failed to resolve the API Key to use");
                if (reasoningCapability.Api.Endpoint.Uri == null) kernelBuilder.AddOpenAIChatCompletion(reasoningCapability.Model, apiKey, organization, reasoningCapability.Provider);
                else kernelBuilder.AddOpenAIChatCompletion(reasoningCapability.Model, reasoningCapability.Api.Endpoint.Uri, apiKey, organization, reasoningCapability.Provider);
                break;
            default:
                throw new NotSupportedException($"The specified reasoning model provider '{reasoningCapability.Provider}' is not supported");
        }
        kernelBuilder.Services.AddSingleton(new PromptExecutionSettings()
        {
            ServiceId = reasoningCapability.Provider,
            ModelId = reasoningCapability.Model,
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
            ExtensionData = reasoningCapability.Settings
        });
        return kernelBuilder;
    }

    /// <summary>
    /// Builds and configures the specified embedding capability
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure</param>
    /// <param name="embeddingCapability">The definition of the embedding capability to build</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The configured <see cref="IKernelBuilder"/></returns>
    protected virtual Task<IKernelBuilder> BuildEmbeddingCapabilityAsync(IKernelBuilder kernelBuilder, EmbeddingCapabilityDefinition embeddingCapability, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(embeddingCapability);
        throw new NotImplementedException(); //todo
    }

}

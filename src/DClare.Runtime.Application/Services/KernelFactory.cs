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

using Microsoft.SemanticKernel.Connectors.Onnx;
using Microsoft.SemanticKernel.Connectors.Weaviate;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IKernelFactory"/> interface.
/// </summary>
/// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s.</param>
/// <param name="componentDefinitionResolver">The service used to resolve <see cref="ReferenceableComponentDefinition"/>s.</param>
/// <param name="pluginManager">The service used to manage <see cref="KernelPlugin"/>s.</param>
/// <param name="oauth2TokenManager">The service used to manage <see cref="OAuth2Token"/>s</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON.</param>
public class KernelFactory(ILoggerFactory loggerFactory, IComponentDefinitionResolver componentDefinitionResolver, IKernelPluginManager pluginManager, IOAuth2TokenManager oauth2TokenManager, IJsonSerializer jsonSerializer)
    : IKernelFactory
{

    /// <summary>
    /// Gets the service used to create <see cref="ILogger"/>s.
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; } = loggerFactory;

    /// <summary>
    /// Gets the service used to resolve <see cref="ReferenceableComponentDefinition"/>s.
    /// </summary>
    protected IComponentDefinitionResolver ComponentDefinitionResolver { get; } = componentDefinitionResolver;

    /// <summary>
    /// Gets the service used to manage <see cref="KernelPlugin"/>s
    /// </summary>
    protected IKernelPluginManager PluginManager { get; } = pluginManager;

    /// <summary>
    /// Gets the service used to manage <see cref="OAuth2Token"/>s
    /// </summary>
    protected IOAuth2TokenManager OAuth2TokenManager { get; } = oauth2TokenManager;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON.
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <inheritdoc/>
    public virtual async Task<Kernel> CreateAsync(KernelDefinition definition, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(definition);
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.Services.AddSingleton(LoggerFactory);
        if (definition.Llm != null) kernelBuilder = await ConfigureLlmAsync(kernelBuilder, definition.Llm, context, cancellationToken).ConfigureAwait(false);
        if (definition.Knowledge != null) kernelBuilder = await ConfigureKnowledgeBaseAsync(kernelBuilder, definition.Knowledge, context, cancellationToken);
        if (definition.Toolsets != null) kernelBuilder = await ConfigureToolsetsAsync(kernelBuilder, definition.Toolsets, context, cancellationToken);
        return kernelBuilder.Build();
    }

    /// <summary>
    /// Configures the Large Language Model (LLM) used by the <see cref="Kernel"/> to create.
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure.</param>
    /// <param name="definition">An object used to configure the <see cref="Kernel"/>'s Large Language Model (LLM).</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The configured <see cref="IKernelBuilder"/>.</returns>
    protected virtual async Task<IKernelBuilder> ConfigureLlmAsync(IKernelBuilder kernelBuilder, LlmDefinition definition, ComponentResolutionContext? context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(definition);
        var llm = definition;
        if (!string.IsNullOrWhiteSpace(llm.Use)) llm = await ComponentDefinitionResolver.ResolveAsync<LlmDefinition>(llm.Use, context, cancellationToken).ConfigureAwait(false);
        var authentication = llm.Api.Endpoint.T1Value?.Authentication;
        var apiKey = authentication?.Scheme switch
        {
            AuthenticationScheme.ApiKey => authentication.ApiKey!.Key,
            AuthenticationScheme.Bearer => authentication.Bearer!.Token,
            AuthenticationScheme.OAuth2 or AuthenticationScheme.OpenIDConnect => (await OAuth2TokenManager.GetTokenAsync(authentication.OAuth2!, cancellationToken).ConfigureAwait(false))?.AccessToken,
            _ => null
        };
        var endpointUri = llm.Api.Endpoint.T1Value == null ? llm.Api.Endpoint.T2Value : llm.Api.Endpoint.T1Value.Uri;
        var provider = llm.Provider.T1Value == null ? llm.Provider.T2Value : llm.Provider.T1Value.Name;
        switch (provider)
        {
            case LlmProvider.AzureOpenAI:
                if (llm.Api.Properties == null || !llm.Api.Properties.TryGetValue("deployment", out var value) || value is not string deployment || string.IsNullOrWhiteSpace(deployment)) throw new NullReferenceException($"The 'deployment' API property must be set when using the '{LlmProvider.AzureOpenAI}' LLM provider");
                if (endpointUri == null) throw new NullReferenceException($"'{nameof(llm)}.{nameof(llm.Api)}.{nameof(llm.Api.Endpoint)}' must be set when using the '{LlmProvider.AzureOpenAI}' LLM provider");
                apiKey = EnsureApiKeyIsResolved(apiKey);
                kernelBuilder.AddAzureOpenAIChatCompletion(deployment, endpointUri.OriginalString, apiKey, provider, llm.Model);
                break;
            case LlmProvider.Gemini:
                apiKey = EnsureApiKeyIsResolved(apiKey);
                kernelBuilder.AddGoogleAIGeminiChatCompletion(llm.Model, apiKey, serviceId: provider);
                break;
            case LlmProvider.HuggingFace:
                if (endpointUri == null) throw new NullReferenceException($"'{nameof(llm)}.{nameof(llm.Api)}.{nameof(llm.Api.Endpoint)}' must be set when using the '{LlmProvider.HuggingFace}' LLM provider");
                kernelBuilder.AddHuggingFaceChatCompletion(endpointUri, apiKey, provider);
                break;
            case LlmProvider.MistralAI:
                apiKey = EnsureApiKeyIsResolved(apiKey);
                kernelBuilder.AddMistralChatCompletion(llm.Model, apiKey, endpointUri, provider);
                break;
            case LlmProvider.Ollama:
                if (endpointUri == null) throw new NullReferenceException($"'{nameof(llm)}.{nameof(llm.Api)}.{nameof(llm.Api.Endpoint)}' must be set when using the '{LlmProvider.Ollama}' LLM provider");
                kernelBuilder.AddOllamaChatCompletion(llm.Model, endpointUri, provider);
                break;
            case LlmProvider.Onnx:
                if (llm.Api.Properties == null || !llm.Api.Properties.TryGetValue("path", out value) || value is not string path || string.IsNullOrWhiteSpace(path)) throw new NullReferenceException($"The 'path' API property must be set when using the '{LlmProvider.Onnx}' LLM provider");
                kernelBuilder.AddOnnxRuntimeGenAIChatCompletion(llm.Model, path, provider);
                break;
            case LlmProvider.OpenAI:
                var organization = llm.Api.Properties == null || !llm.Api.Properties.TryGetValue("organization", out value) ? null : value as string;
                apiKey = EnsureApiKeyIsResolved(apiKey);
                if (endpointUri == null) kernelBuilder.AddOpenAIChatCompletion(llm.Model, apiKey, organization, provider);
                else kernelBuilder.AddOpenAIChatCompletion(llm.Model, endpointUri, apiKey, organization, provider);
                break;
            default:
                throw new NotSupportedException($"The specified LLM provider '{provider}' is not supported");
        }
        kernelBuilder.Services.AddSingleton(new PromptExecutionSettings()
        {
            ServiceId = provider,
            ModelId = llm.Model,
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
            ExtensionData = llm.Provider.T1Value?.Configuration
        });
        return kernelBuilder;
    }

    /// <summary>
    /// Configures the knowledge base used by the <see cref="Kernel"/>.
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure.</param>
    /// <param name="definition">An object used to configure the <see cref="Kernel"/>'s knowledge base.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The configured <see cref="IKernelBuilder"/>.</returns>
    protected virtual async Task<IKernelBuilder> ConfigureKnowledgeBaseAsync(IKernelBuilder kernelBuilder, KnowledgeDefinition definition, ComponentResolutionContext? context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(definition);
        kernelBuilder = await ConfigureEmbeddingAsync(kernelBuilder, definition.Embedding, context, cancellationToken).ConfigureAwait(false);
        kernelBuilder = await ConfigureVectorStoreAsync(kernelBuilder, definition.Store, context, cancellationToken).ConfigureAwait(false);
        if (definition.Graph != null) await ConfigureGraphDatabaseAsync(kernelBuilder, definition.Graph, context, cancellationToken).ConfigureAwait(false);
        return kernelBuilder;
    }

    /// <summary>
    /// Configures the embedding model used by the <see cref="Kernel"/> to vectorize data.
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure.</param>
    /// <param name="definition">An object used to configure the embedding model used by the <see cref="Kernel"/>.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The configured <see cref="IKernelBuilder"/>.</returns>
    protected virtual async Task<IKernelBuilder> ConfigureEmbeddingAsync(IKernelBuilder kernelBuilder, EmbeddingModelDefinition definition, ComponentResolutionContext? context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(definition);
        var embeddingModel = definition;
        if (!string.IsNullOrWhiteSpace(embeddingModel.Use)) embeddingModel = await ComponentDefinitionResolver.ResolveAsync<EmbeddingModelDefinition>(embeddingModel.Use, context, cancellationToken).ConfigureAwait(false);
        var authentication = embeddingModel.Api.Endpoint.T1Value?.Authentication;
        var apiKey = authentication?.Scheme switch
        {
            AuthenticationScheme.ApiKey => authentication.ApiKey!.Key,
            AuthenticationScheme.Bearer => authentication.Bearer!.Token,
            AuthenticationScheme.OAuth2 or AuthenticationScheme.OpenIDConnect => (await OAuth2TokenManager.GetTokenAsync(authentication.OAuth2!, cancellationToken).ConfigureAwait(false))?.AccessToken,
            _ => null
        };
        var endpointUri = embeddingModel.Api.Endpoint.T1Value == null ? embeddingModel.Api.Endpoint.T2Value : embeddingModel.Api.Endpoint.T1Value.Uri;
        var providerConfiguration = embeddingModel.Provider.T1Value?.Configuration;
        object? dimensions = null;
        var provider = embeddingModel.Provider.T1Value == null ? embeddingModel.Provider.T2Value : embeddingModel.Provider.T1Value.Name;
        switch (provider)
        {
            case LlmProvider.AzureOpenAI:
                if (embeddingModel.Api.Properties == null || !embeddingModel.Api.Properties.TryGetValue("deployment", out var value) || value is not string deployment || string.IsNullOrWhiteSpace(deployment)) throw new NullReferenceException($"The 'deployment' API property must be set when using the '{LlmProvider.AzureOpenAI}' LLM provider");
                if (endpointUri == null) throw new NullReferenceException($"'{nameof(embeddingModel)}.{nameof(embeddingModel.Api)}.{nameof(embeddingModel.Api.Endpoint)}' must be set when using the '{LlmProvider.AzureOpenAI}' LLM provider");
                apiKey = EnsureApiKeyIsResolved(apiKey);
                providerConfiguration?.TryGetValue("dimensions", out dimensions);
                kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(deployment, endpointUri.OriginalString, apiKey, provider, embeddingModel.Model, dimensions: dimensions as int?);
                break;
            case LlmProvider.Gemini:
                apiKey = EnsureApiKeyIsResolved(apiKey);
                kernelBuilder.AddGoogleAIEmbeddingGeneration(embeddingModel.Model, apiKey, serviceId: provider);
                break;
            case LlmProvider.HuggingFace:
                if (endpointUri == null) throw new NullReferenceException($"'{nameof(embeddingModel)}.{nameof(embeddingModel.Api)}.{nameof(embeddingModel.Api.Endpoint)}' must be set when using the '{LlmProvider.HuggingFace}' LLM provider");
                kernelBuilder.AddHuggingFaceTextEmbeddingGeneration(endpointUri, apiKey, provider);
                break;
            case LlmProvider.MistralAI:
                apiKey = EnsureApiKeyIsResolved(apiKey);
                kernelBuilder.AddMistralTextEmbeddingGeneration(embeddingModel.Model, apiKey, endpointUri, provider);
                break;
            case LlmProvider.Ollama:
                if (endpointUri == null) throw new NullReferenceException($"'{nameof(embeddingModel)}.{nameof(embeddingModel.Api)}.{nameof(embeddingModel.Api.Endpoint)}' must be set when using the '{LlmProvider.Ollama}' LLM provider");
                kernelBuilder.AddOllamaTextEmbeddingGeneration(embeddingModel.Model, endpointUri, provider);
                break;
            case LlmProvider.Onnx:
                if (embeddingModel.Api.Properties == null || !embeddingModel.Api.Properties.TryGetValue("path", out value) || value is not string path || string.IsNullOrWhiteSpace(path)) throw new NullReferenceException($"The 'path' API property must be set when using the '{LlmProvider.Onnx}' LLM provider");
                var options = providerConfiguration == null ? null : (BertOnnxOptions)JsonSerializer.Convert(providerConfiguration, typeof(BertOnnxOptions))!;
                kernelBuilder.AddBertOnnxTextEmbeddingGeneration(embeddingModel.Model, path, options, provider);
                break;
            case LlmProvider.OpenAI:
                var organization = embeddingModel.Api.Properties == null || !embeddingModel.Api.Properties.TryGetValue("organization", out value) ? null : value as string;
                apiKey = EnsureApiKeyIsResolved(apiKey);
                providerConfiguration?.TryGetValue("dimensions", out dimensions);
                kernelBuilder.AddOpenAITextEmbeddingGeneration(embeddingModel.Model, apiKey, organization, provider, dimensions: dimensions as int?);
                break;
            default:
                throw new NotSupportedException($"The specified embedding model provider '{provider}' is not supported");
        }
        return kernelBuilder;
    }

    /// <summary>
    /// Configures the vector store used by the <see cref="Kernel"/> for indexing and retrieving semantic representations.
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure.</param>
    /// <param name="definition">An object used to configure the vector store component of the <see cref="Kernel"/>.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The configured <see cref="IKernelBuilder"/>.</returns>
    protected virtual async Task<IKernelBuilder> ConfigureVectorStoreAsync(IKernelBuilder kernelBuilder, VectorStoreDefinition definition, ComponentResolutionContext? context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(definition);
        var vectorStore = definition;
        if (!string.IsNullOrWhiteSpace(vectorStore.Use)) vectorStore = await ComponentDefinitionResolver.ResolveAsync<VectorStoreDefinition>(vectorStore.Use, context, cancellationToken).ConfigureAwait(false);
        var collection = vectorStore.Provider.Configuration?.TryGetValue("collection", out var configurationValue) == true && configurationValue != null ? JsonSerializer.Convert<string>(configurationValue) : null;
        var authentication = vectorStore.Provider.Configuration?.TryGetValue("authentication", out configurationValue) == true && configurationValue != null ? JsonSerializer.Convert<AuthenticationPolicyDefinition>(configurationValue) : null;
        var apiKey = authentication?.Scheme switch
        {
            AuthenticationScheme.ApiKey => authentication.ApiKey!.Key,
            AuthenticationScheme.Bearer => authentication.Bearer!.Token,
            AuthenticationScheme.OAuth2 or AuthenticationScheme.OpenIDConnect => (await OAuth2TokenManager.GetTokenAsync(authentication.OAuth2!, cancellationToken).ConfigureAwait(false))?.AccessToken,
            _ => null
        };
        switch (vectorStore.Provider.Name)
        {
            case VectorStoreProvider.Pinecone:
                if (string.IsNullOrWhiteSpace(collection)) throw new NullReferenceException($"The '{nameof(collection)}' configuration property must be set when using the '{VectorStoreProvider.Pinecone}' vector store provider");
                apiKey = EnsureApiKeyIsResolved(apiKey);
                kernelBuilder.AddPineconeVectorStoreRecordCollection<TextEmbeddingRecord<Guid>>(collection, apiKey);
                kernelBuilder.Services.AddTransient<IVectorStoreRecordCollection, VectorStoreRecordCollection<Guid>>();
                break;
            case VectorStoreProvider.Qdrant:
                if (string.IsNullOrWhiteSpace(collection)) throw new NullReferenceException($"The '{nameof(collection)}' configuration property must be set when using the '{VectorStoreProvider.Qdrant}' vector store provider");
                var host = vectorStore.Provider.Configuration?.TryGetValue("host", out configurationValue) == true && configurationValue != null ? JsonSerializer.Convert<string>(configurationValue) : null;
                if (string.IsNullOrWhiteSpace(host)) throw new NullReferenceException($"The '{nameof(host)}' configuration property must be set when using the '{VectorStoreProvider.Qdrant}' vector store provider");
                var port = vectorStore.Provider.Configuration?.TryGetValue("port", out configurationValue) == true && configurationValue != null ? JsonSerializer.Convert<int>(configurationValue) : 6334;
                var https = vectorStore.Provider.Configuration?.TryGetValue("https", out configurationValue) == true && configurationValue != null && JsonSerializer.Convert<bool>(configurationValue);
                kernelBuilder.AddQdrantVectorStoreRecordCollection<Guid, TextEmbeddingRecord<Guid>>(collection, host, port, https, apiKey);
                kernelBuilder.Services.AddTransient<IVectorStoreRecordCollection, VectorStoreRecordCollection<Guid>>();
                break;
            case VectorStoreProvider.Redis:
                if (string.IsNullOrWhiteSpace(collection)) throw new NullReferenceException($"The '{nameof(collection)}' configuration property must be set when using the '{VectorStoreProvider.Redis}' vector store provider");
                var connectionString = vectorStore.Provider.Configuration?.TryGetValue("connectionString", out configurationValue) == true && configurationValue != null ? JsonSerializer.Convert<string>(configurationValue) : null;
                if (string.IsNullOrWhiteSpace(connectionString)) throw new NullReferenceException($"The '{nameof(connectionString)}' configuration property must be set when using the '{VectorStoreProvider.Redis}' vector store provider");
                kernelBuilder.AddRedisJsonVectorStoreRecordCollection<TextEmbeddingRecord<string>>(collection, connectionString);
                kernelBuilder.Services.AddTransient<IVectorStoreRecordCollection, VectorStoreRecordCollection<string>>();
                break;
            case VectorStoreProvider.Weaviate:
                if (string.IsNullOrWhiteSpace(collection)) throw new NullReferenceException($"The '{nameof(collection)}' configuration property must be set when using the '{VectorStoreProvider.Weaviate}' vector store provider");
                var endpoint = vectorStore.Provider.Configuration?.TryGetValue("endpoint", out configurationValue) == true && configurationValue != null ? JsonSerializer.Convert<Uri>(configurationValue) : null;
                kernelBuilder.AddWeaviateVectorStoreRecordCollection(collection, null, new WeaviateVectorStoreRecordCollectionOptions<TextEmbeddingRecord<Guid>>()
                {
                    Endpoint = endpoint,
                    ApiKey = apiKey
                });
                kernelBuilder.Services.AddTransient<IVectorStoreRecordCollection, VectorStoreRecordCollection<Guid>>();
                break;
            default:
                throw new NotSupportedException($"The specified vector store provider '{vectorStore.Provider.Name}' is not supported");
        }
        return kernelBuilder;
    }
    
    /// <summary>
    /// Configures the graph database used by the <see cref="Kernel"/> for storing and querying structured knowledge.
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure.</param>
    /// <param name="definition">An object used to configure the graph database integration for the <see cref="Kernel"/>.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The configured <see cref="IKernelBuilder"/>.</returns>
    protected virtual async Task<IKernelBuilder> ConfigureGraphDatabaseAsync(IKernelBuilder kernelBuilder, KnowledgeGraphDefinition definition, ComponentResolutionContext? context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(definition);
        var graphDatabase = definition;
        if (!string.IsNullOrWhiteSpace(graphDatabase.Use)) graphDatabase = await ComponentDefinitionResolver.ResolveAsync<KnowledgeGraphDefinition>(graphDatabase.Use, context, cancellationToken).ConfigureAwait(false);
        switch (graphDatabase.Provider.Name)
        {
            case KnowledgeGraphProvider.Neo4j:

                break;
            default:
                throw new NotSupportedException($"The specified graph database provider '{graphDatabase.Provider.Name}' is not supported");
        }
        return kernelBuilder;
    }

    /// <summary>
    /// Configures the toolsets to be registered and made available to the <see cref="Kernel"/> during execution.
    /// </summary>
    /// <param name="kernelBuilder">The <see cref="IKernelBuilder"/> to configure.</param>
    /// <param name="toolsets">A dictionary of toolset definitions to be registered, keyed by their logical name.</param>
    /// <param name="context">The current <see cref="ComponentResolutionContext"/>, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The configured <see cref="IKernelBuilder"/>.</returns>
    protected virtual async Task<IKernelBuilder> ConfigureToolsetsAsync(IKernelBuilder kernelBuilder, EquatableDictionary<string, ToolsetDefinition> toolsets, ComponentResolutionContext? context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(kernelBuilder);
        ArgumentNullException.ThrowIfNull(toolsets);
        foreach (var toolset in toolsets)
        {
            var toolsetName = toolset.Key;
            var toolsetDefinition = toolset.Value;
            if (!string.IsNullOrWhiteSpace(toolsetDefinition.Use)) toolsetDefinition = await ComponentDefinitionResolver.ResolveAsync<ToolsetDefinition>(toolsetDefinition.Use, context, cancellationToken).ConfigureAwait(false);
            kernelBuilder.Plugins.Add(await PluginManager.GetOrLoadAsync(toolsetName, toolsetDefinition, cancellationToken).ConfigureAwait(false));
        }
        return kernelBuilder;
    }

    static string EnsureApiKeyIsResolved(string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) throw new InvalidOperationException("Failed to resolve the API Key to use.");
        return apiKey!;
    }

}

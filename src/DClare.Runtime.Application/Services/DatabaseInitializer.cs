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
/// Represents the service used to initialize the Synapse resource database.
/// </summary>
public class DatabaseInitializer(ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IYamlSerializer yamlSerializer)
    : Neuroglia.Data.Infrastructure.ResourceOriented.Services.DatabaseInitializer(loggerFactory, serviceProvider)
{

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from YAML.
    /// </summary>
    protected IYamlSerializer YamlSerializer { get; } = yamlSerializer;

    /// <summary>
    /// Gets the path to the directory that contains seed files.
    /// </summary>
    protected string SeedDataDirectoryPath = Path.Combine(AppContext.BaseDirectory, "data", "seed");

    /// <inheritdoc/>
    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        try
        {
            await InitializeAsync(stoppingToken).ConfigureAwait(false);
        }
        catch (ProblemDetailsException ex) when (ex.Problem.Status == (int)HttpStatusCode.Conflict || (ex.Problem.Status == (int)HttpStatusCode.BadRequest && ex.Problem.Title == "Conflict")) { }
    }

    /// <inheritdoc/>
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedResourceDefinitionsAsync(cancellationToken).ConfigureAwait(false);
        await SeedAuthenticationPoliciesAsync(cancellationToken).ConfigureAwait(false);
        await SeedToolsetsAsync(cancellationToken).ConfigureAwait(false);
        await SeedPromptTemplatesAsync(cancellationToken).ConfigureAwait(false);
        await SeedFunctionsAsync(cancellationToken).ConfigureAwait(false);
        await SeedMemoryDefinitionsAsync(cancellationToken).ConfigureAwait(false);
        await SeedEmbeddingModelsAsync(cancellationToken).ConfigureAwait(false);
        await SeedVectorStoresAsync(cancellationToken).ConfigureAwait(false);
        await SeedKnowledgeGraphsAsync(cancellationToken).ConfigureAwait(false);
        await SeedLargeLanguageModelsAsync(cancellationToken).ConfigureAwait(false);
        await SeedAgentsAsync(cancellationToken).ConfigureAwait(false);
        await SeedWorkflowsAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Seeds <see cref="ResourceDefinition"/>s.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task SeedResourceDefinitionsAsync(CancellationToken cancellationToken)
    {
        foreach (var definition in DClareResources.Definitions.AsEnumerable())
        {
            try { await Database.CreateResourceAsync(definition, cancellationToken: cancellationToken).ConfigureAwait(false); }
            catch (ProblemDetailsException ex) when (ex.Problem.Status == (int)HttpStatusCode.Conflict || (ex.Problem.Status == (int)HttpStatusCode.BadRequest && ex.Problem.Title == "Conflict")) { continue; }
        }
    }

    protected virtual Task SeedAuthenticationPoliciesAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

    protected virtual Task SeedToolsetsAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

    protected virtual Task SeedPromptTemplatesAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

    protected virtual Task SeedFunctionsAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

    protected virtual Task SeedMemoryDefinitionsAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

    /// <summary>
    /// Seeds <see cref="EmbeddingModel"/>s.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task SeedEmbeddingModelsAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(SeedDataDirectoryPath, "embedding-models");
        if (!Directory.Exists(path)) return;
        foreach (var file in Directory.GetFiles(path, "*.yaml"))
        {
            var yaml = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
            try
            {
                var resource = YamlSerializer.Deserialize<EmbeddingModel>(yaml)!;
                await Database.CreateResourceAsync(resource, false, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred while importing a EmbeddingModel resource from file '{file}': {ex}", file, ex);
                continue;
            }
        }
    }

    /// <summary>
    /// Seeds <see cref="VectorStore"/>s.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task SeedVectorStoresAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(SeedDataDirectoryPath, "vector-stores");
        if (!Directory.Exists(path)) return;
        foreach (var file in Directory.GetFiles(path, "*.yaml"))
        {
            var yaml = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
            try
            {
                var resource = YamlSerializer.Deserialize<VectorStore>(yaml)!;
                await Database.CreateResourceAsync(resource, false, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred while importing a VectorStore resource from file '{file}': {ex}", file, ex);
                continue;
            }
        }
    }

    protected virtual Task SeedKnowledgeGraphsAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

    /// <summary>
    /// Seeds <see cref="Llm"/>s.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task SeedLargeLanguageModelsAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(SeedDataDirectoryPath, "llms");
        if (!Directory.Exists(path)) return;
        foreach (var file in Directory.GetFiles(path, "*.yaml"))
        {
            var yaml = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
            try
            {
                var resource = YamlSerializer.Deserialize<Llm>(yaml)!;
                await Database.CreateResourceAsync(resource, false, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred while importing an Llm resource from file '{file}': {ex}", file, ex);
                continue;
            }
        }
    }

    /// <summary>
    /// Seeds <see cref="Agent"/>s.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    protected virtual async Task SeedAgentsAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(SeedDataDirectoryPath, "agents");
        if (!Directory.Exists(path)) return;
        foreach (var file in Directory.GetFiles(path, "*.yaml"))
        {
            var yaml = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
            try
            {
                var resource = YamlSerializer.Deserialize<Agent>(yaml)!;
                await Database.CreateResourceAsync(resource, false, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred while importing an Agent resource from file '{file}': {ex}", file, ex);
                continue;
            }
        }
    }

    protected virtual Task SeedWorkflowsAsync(CancellationToken cancellationToken)
    {
        //todo: implement
        return Task.CompletedTask;
    }

}

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
/// Represents the default implementation of the <see cref="IComponentDefinitionResolver"/> interface.
/// </summary>
/// <param name="resources">The repository used to manage the application's resources.</param>
public class ComponentDefinitionResolver(IResourceRepository resources)
    : IComponentDefinitionResolver
{

    /// <summary>
    /// Gets the repository used to manage the application's resources.
    /// </summary>
    protected IResourceRepository Resources { get; } = resources;

    /// <inheritdoc/>
    public virtual async Task<TComponent> ResolveAsync<TComponent>(string reference, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
        where TComponent : ReferenceableComponentDefinition, new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        if (context != null && context.Scope == ComponentScope.Contextual) return await ResolveContextualResourceAsync<TComponent>(reference, context.Components, cancellationToken).ConfigureAwait(false);
        else return await ResolveGlobalResourceAsync<TComponent>(reference, cancellationToken).ConfigureAwait(false);
    }

    protected virtual async Task<TComponent> ResolveContextualResourceAsync<TComponent>(string reference, ComponentDefinitionCollection? components, CancellationToken cancellationToken)
        where TComponent : ReferenceableComponentDefinition, new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var referenceComponents = reference.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (referenceComponents.Length == 1) return components?.Get<TComponent>(reference) ?? throw new ProblemDetailsException(Problems.ComponentNotFound<TComponent>(reference));
        else return await ResolveGlobalResourceAsync<TComponent>(referenceComponents.Last(), cancellationToken).ConfigureAwait(false);
    }

    protected virtual async Task<TComponent> ResolveGlobalResourceAsync<TComponent>(string reference, CancellationToken cancellationToken)
        where TComponent : ReferenceableComponentDefinition, new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var components = reference.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (components.Length != 2) throw new ProblemDetailsException(Problems.InvalidQualifiedName(reference));
        var @namespace = components[1];
        var name = components[0];
        var componentType = typeof(TComponent);
        if (componentType == typeof(AgentDefinition))
        {
            var resource = await Resources.GetAsync<Agent>(name, @namespace, cancellationToken).ConfigureAwait(false) ?? throw new ProblemDetailsException(Problems.ComponentNotFound<TComponent>(reference));
            return (resource.Spec.Definition as TComponent)!;
        }
        else if (componentType == typeof(EmbeddingModelDefinition))
        {
            var resource = await Resources.GetAsync<EmbeddingModel>(name, @namespace, cancellationToken).ConfigureAwait(false) ?? throw new ProblemDetailsException(Problems.ComponentNotFound<TComponent>(reference));
            return (resource.Spec.Definition as TComponent)!;
        }
        else if (componentType == typeof(VectorStoreDefinition))
        {
            var resource = await Resources.GetAsync<VectorStore>(name, @namespace, cancellationToken).ConfigureAwait(false) ?? throw new ProblemDetailsException(Problems.ComponentNotFound<TComponent>(reference));
            return (resource.Spec.Definition as TComponent)!;
        }
        else throw new NotSupportedException($"The specified component type '{componentType.Name}' is not supported in this context");
    }

}

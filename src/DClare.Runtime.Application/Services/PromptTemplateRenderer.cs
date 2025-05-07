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

using Duende.IdentityModel.Client;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IPromptTemplateRenderer"/> interface.
/// </summary>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/>.</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON.</param>
public class PromptTemplateRenderer(IServiceProvider serviceProvider, IJsonSerializer jsonSerializer)
    : IPromptTemplateRenderer
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from JSON.
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <inheritdoc/>
    public virtual async Task<string> RenderAsync(PromptTemplateDefinition definition, Kernel kernel, IDictionary<string, object>? arguments = null, ComponentResolutionContext? context = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(kernel);
        if (definition.Format == PromptTemplateFormat.Echo) return definition.Content;
        var factoryType = definition.Format switch
        {
            PromptTemplateFormat.Handlebars => typeof(HandlebarsPromptTemplateFactory),
            PromptTemplateFormat.Liquid => typeof(LiquidPromptTemplateFactory),
            PromptTemplateFormat.SemanticKernel => typeof(KernelPromptTemplateFactory),
            _ => throw new NotSupportedException($"The specified prompt template format '{definition.Format}' is not supported")
        };
        var factory = (IPromptTemplateFactory)ActivatorUtilities.CreateInstance(ServiceProvider, factoryType);
        var templateConfig = new PromptTemplateConfig()
        {
            TemplateFormat = definition.Format,
            Template = definition.Content,
            InputVariables = definition.InputVariables?.Select(v => new InputVariable()
            {
                Name = v.Name,
                Description = v.Description,
                IsRequired = v.Required,
                Default = v.Default,
                Sample = v.Sample,
                JsonSchema = v.Schema == null ? null : JsonSerializer.SerializeToText(v.Schema),
                AllowDangerouslySetContent = v.AllowDangerousContent
            }).ToList() ?? [],
            OutputVariable = definition.OutputVariable == null ? null : new()
            {
                Description = definition.OutputVariable.Description,
                JsonSchema = definition.OutputVariable.Schema == null ? null : JsonSerializer.SerializeToText(definition.OutputVariable.Schema)
            }
        };
        var template = factory.Create(templateConfig);
        var templateArguments = arguments == null ? null : new KernelArguments(arguments!);
        return await template.RenderAsync(kernel, templateArguments, cancellationToken).ConfigureAwait(false);
    }

}
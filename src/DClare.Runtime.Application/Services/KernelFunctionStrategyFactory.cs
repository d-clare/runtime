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

using System.Text.Json;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IKernelFunctionStrategyFactory"/> interface
/// </summary>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
/// <param name="kernelFactory">The service used to create <see cref="Kernel"/>s</param>
/// <param name="jsonSerializer">The service used to serialize/deserialize data to/from JSON</param>
/// <param name="jsonSerializerOptions">The service used to access the current <see cref="System.Text.Json.JsonSerializerOptions"/></param>
public class KernelFunctionStrategyFactory(IServiceProvider serviceProvider, IKernelFactory kernelFactory, IJsonSerializer jsonSerializer, IOptions<JsonSerializerOptions> jsonSerializerOptions)
    : IKernelFunctionStrategyFactory
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
    /// Gets the service used to serialize/deserialize data to/from JSON
    /// </summary>
    protected IJsonSerializer JsonSerializer { get; } = jsonSerializer;

    /// <summary>
    /// Gets the current <see cref="System.Text.Json.JsonSerializerOptions"/>
    /// </summary>
    protected JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions.Value;

    /// <inheritdoc/>
    public virtual async Task<IKernelFunctionStrategy> CreateAsync(KernelFunctionStrategyDefinition definition, ComponentCollectionDefinition? components = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(definition);
        var kernel = await KernelFactory.CreateAsync(definition.Kernel, components, cancellationToken).ConfigureAwait(false);
        var promptExecutionSettings = kernel.Services.GetRequiredService<PromptExecutionSettings>();
        var promptTemplate = new PromptTemplateConfig()
        { 
            AllowDangerouslySetContent = true,
            Template = definition.Function.Template.Content,
            TemplateFormat = definition.Function.Template.Format,
            InputVariables = definition.Function.Template.InputVariables?.Select(v => new InputVariable() 
            { 
                Name = v.Name,
                Description = v.Description,
                IsRequired = v.Required,
                Default = v.Default,
                Sample = v.Sample,
                AllowDangerouslySetContent = v.AllowDangerousContent,
                JsonSchema = v.Schema == null ? null : JsonSerializer.SerializeToText(v.Schema)
            }).ToList() ?? [],
            OutputVariable = definition.Function.Template.OutputVariable == null ? null : new OutputVariable()
            {
                Description = definition.Function.Template.OutputVariable.Description,
                JsonSchema = definition.Function.Template.OutputVariable.Schema == null ? null : JsonSerializer.SerializeToText(definition.Function.Template.OutputVariable.Schema)
            },
            ExecutionSettings = new()
            {
                { promptExecutionSettings.ServiceId!, promptExecutionSettings }
            }
        };
        var function = kernel.CreateFunctionFromPrompt(promptTemplate, JsonSerializerOptions);
        return ActivatorUtilities.CreateInstance<KernelFunctionStrategy>(ServiceProvider, kernel, function);
    }

}
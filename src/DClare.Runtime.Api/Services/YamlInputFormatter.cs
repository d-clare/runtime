// Copyright � 2025-Present The DClare Authors
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

using Microsoft.AspNetCore.Mvc.Formatters;
using Neuroglia.Serialization;
using System.Text;

namespace DClare.Runtime.Api.Services;

/// <summary>
/// Represents the <see cref="TextInputFormatter"/> used to deserialize data from YAML
/// </summary>
public class YamlInputFormatter 
    : TextInputFormatter
{

    /// <summary>
    /// Initializes a new <see cref="YamlInputFormatter"/>
    /// </summary>
    /// <param name="yamlSerializer">The service used to serialize/deserialize data to/from YAML</param>
    /// <exception cref="ArgumentNullException"></exception>
    public YamlInputFormatter(IYamlSerializer yamlSerializer)
    {
        YamlSerializer = yamlSerializer;
        SupportedMediaTypes.Add("application/x-yaml");
        SupportedMediaTypes.Add("application/yaml");
        SupportedMediaTypes.Add("text/yaml");
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from YAML
    /// </summary>
    protected IYamlSerializer YamlSerializer { get; }

    /// <inheritdoc/>
    protected override bool CanReadType(Type type) => true;

    /// <inheritdoc/>
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
        var yaml = await reader.ReadToEndAsync().ConfigureAwait(false);
        var result = YamlSerializer.Deserialize(yaml, context.ModelType);
        return await InputFormatterResult.SuccessAsync(result);
    }

}

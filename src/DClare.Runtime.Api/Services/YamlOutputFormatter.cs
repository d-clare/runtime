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
using System.Text;

namespace DClare.Runtime.Api.Services;

/// <summary>
/// Represents the <see cref="TextInputFormatter"/> used to serialize data to YAML
/// </summary>
public class YamlOutputFormatter 
    : TextOutputFormatter
{

    /// <summary>
    /// Initializes a new <see cref="YamlOutputFormatter"/>
    /// </summary>
    /// <param name="yamlSerializer">The service used to serialize/deserialize data to/from YAML</param>
    public YamlOutputFormatter(IYamlSerializer yamlSerializer)
    {
        YamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
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
    protected override bool CanWriteType(Type? type) => true;

    /// <inheritdoc/>
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var yaml = YamlSerializer.SerializeToText(context.Object!);
        context.HttpContext.Response.ContentType = "text/yaml";
        await context.HttpContext.Response.WriteAsync(yaml, selectedEncoding);
    }

}
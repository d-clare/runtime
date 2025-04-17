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

using A2A.Models;
using A2A.Server;
using A2A.Server.Infrastructure;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s
/// </summary>
public static class IServiceCollectionExtensions
{

    /// <summary>
    /// Adds and configures the <see cref="AgentCard"/> used to document the application's hosted agents
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddA2AAgents(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new ApplicationOptions();
        configuration.Bind(options);
        if (options.Components?.Agents != null)
        {
            foreach (var agent in options.Components.Agents.Where(a => a.Value.Hosted != null))
            {
                services.AddA2AWellKnownAgent((provider, builder) =>
                {
                    builder
                        .WithName(agent.Key)
                        .WithDescription(agent.Value.Hosted!.Description!)
                        .WithVersion("1.0.0")
                        .WithUrl(new($"/a2a/{agent.Key}", UriKind.RelativeOrAbsolute));
                    if (agent.Value.Hosted.Skills != null)
                    {
                        foreach (var skill in agent.Value.Hosted.Skills)
                        {
                            builder
                                .WithSkill(skillBuilder => skillBuilder
                                    .WithId(skill.Name.ToCamelCase())
                                    .WithName(skill.Name)
                                    .WithDescription(skill.Description!));
                        }
                    }
                });
                services.AddA2AProtocolServer(agent.Key, builder =>
                {
                    builder
                        .UseAgentRuntime(provider => ActivatorUtilities.CreateInstance<A2AAgentRuntime>(provider, agent.Key, agent.Value, options.Components))
                        .UseDistributedCacheTaskRepository();
                });
            }
        }
        return services;
    }

}
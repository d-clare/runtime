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

using DClare.Runtime.Application.Commands.Resources;
using DClare.Runtime.Application.Queries.Resources;
using DClare.Runtime.Integration.Commands.Resources;
using DClare.Runtime.Integration.Queries.Resources;

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s.
/// </summary>
public static class IServiceCollectionExtensions
{

    const string InstanceNameConfigurationPropertyName = "instanceName";

    /// <summary>
    /// Adds and configures the application's cache
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new ApplicationOptions();
        configuration.Bind(options);
        switch (options.Cache.Provider)
        {
            case CacheProvider.Memory:
                services.AddDistributedMemoryCache();
                break;
            case CacheProvider.Redis:
                services.AddStackExchangeRedisCache(redis =>
                {
                    var connectionString = configuration.GetConnectionString("redis");
                    redis.Configuration = connectionString;
                    if (options.Cache.Configuration == null) return;
                    var configurationProperties = new Dictionary<string, object>(options.Cache.Configuration, StringComparer.OrdinalIgnoreCase);
                    if (configurationProperties.TryGetValue(InstanceNameConfigurationPropertyName, out var value) && value is string instanceName) redis.InstanceName = instanceName;
                });
                break;
        }
        return services;
    }

    /// <summary>
    /// Registers and configures generic resource query handlers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddResourceQueries(this IServiceCollection services)
    {
        var resourceTypes = typeof(Workflow).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && !t.IsInterface && typeof(Resource).IsAssignableFrom(t)).ToList();
        resourceTypes.Add(typeof(Namespace));
        foreach (var queryableType in resourceTypes)
        {
            var serviceLifetime = ServiceLifetime.Scoped;
            Type queryType;
            Type resultType;
            Type handlerServiceType;
            Type handlerImplementationType;

            queryType = typeof(GetResourceQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(queryableType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(GetResourceQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            queryType = typeof(GetResourceDefinitionQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<IResourceDefinition>);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(GetResourceDefinitionQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            queryType = typeof(GetResourcesQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(typeof(IAsyncEnumerable<>).MakeGenericType(queryableType));
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(GetResourcesQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            queryType = typeof(ListResourcesQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(typeof(Neuroglia.Data.Infrastructure.ResourceOriented.ICollection<>).MakeGenericType(queryableType));
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(ListResourcesQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            queryType = typeof(WatchResourcesQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(typeof(IAsyncEnumerable<>).MakeGenericType(typeof(IResourceWatchEvent<>).MakeGenericType(queryableType)));
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(WatchResourcesQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            queryType = typeof(MonitorResourceQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(typeof(IAsyncEnumerable<>).MakeGenericType(typeof(IResourceWatchEvent<>).MakeGenericType(queryableType)));
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(MonitorResourceQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));
        }
        return services;
    }

    /// <summary>
    /// Registers and configures generic resource command handlers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddResourceCommands(this IServiceCollection services)
    {
        var resourceTypes = typeof(Workflow).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && !t.IsInterface && typeof(Resource).IsAssignableFrom(t)).ToList();
        resourceTypes.Add(typeof(Namespace));
        foreach (var resourceType in resourceTypes)
        {
            var serviceLifetime = ServiceLifetime.Scoped;
            Type commandType;
            Type resultType;
            Type handlerServiceType;
            Type handlerImplementationType;

            commandType = typeof(CreateResourceCommand<>).MakeGenericType(resourceType);
            resultType = typeof(IOperationResult<>).MakeGenericType(resourceType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, resultType);
            handlerImplementationType = typeof(CreateResourceCommandHandler<>).MakeGenericType(resourceType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            commandType = typeof(UpdateResourceCommand<>).MakeGenericType(resourceType);
            resultType = typeof(IOperationResult<>).MakeGenericType(resourceType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, resultType);
            handlerImplementationType = typeof(UpdateResourceCommandHandler<>).MakeGenericType(resourceType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            commandType = typeof(UpdateResourceStatusCommand<>).MakeGenericType(resourceType);
            resultType = typeof(IOperationResult<>).MakeGenericType(resourceType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, resultType);
            handlerImplementationType = typeof(UpdateResourceStatusCommandHandler<>).MakeGenericType(resourceType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            commandType = typeof(PatchResourceCommand<>).MakeGenericType(resourceType);
            resultType = typeof(IOperationResult<>).MakeGenericType(resourceType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, resultType);
            handlerImplementationType = typeof(PatchResourceCommandHandler<>).MakeGenericType(resourceType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            commandType = typeof(PatchResourceStatusCommand<>).MakeGenericType(resourceType);
            resultType = typeof(IOperationResult<>).MakeGenericType(resourceType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, resultType);
            handlerImplementationType = typeof(PatchResourceStatusCommandHandler<>).MakeGenericType(resourceType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

            commandType = typeof(DeleteResourceCommand<>).MakeGenericType(resourceType);
            resultType = typeof(IOperationResult<>).MakeGenericType(resourceType);
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, resultType);
            handlerImplementationType = typeof(DeleteResourceCommandHandler<>).MakeGenericType(resourceType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));

        }
        return services;
    }

}

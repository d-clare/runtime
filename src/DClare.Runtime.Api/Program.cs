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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ApplicationOptions>().Bind(builder.Configuration).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddOpenApi();
builder.Services.AddDClare();
builder.Services.AddMediator(options =>
{
    options.ScanAssembly(typeof(ApplicationOptions).Assembly);
});
builder.Services.AddResourceCommands();
builder.Services.AddResourceQueries();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddCache(builder.Configuration);
builder.Services.AddRedisDatabase(builder.Configuration.GetConnectionString("redis")!);
builder.Services.AddHostedService<DClare.Runtime.Application.Services.DatabaseInitializer>();
builder.Services.AddSingleton<IUserAccessor, HttpContextUserAccessor>();
builder.Services.AddScoped<IUserInfoProvider, UserInfoProvider>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IAdmissionControl, AdmissionControl>();
builder.Services.AddScoped<IVersionControl, VersionControl>();
builder.Services.AddSingleton<IPatchHandler, JsonMergePatchHandler>();
builder.Services.AddSingleton<IPatchHandler, JsonPatchHandler>();
builder.Services.AddSingleton<IPatchHandler, JsonStrategicMergePatchHandler>();
builder.Services.AddSingleton<IOAuth2TokenManager, OAuth2TokenManager>();
builder.Services.AddScoped<IComponentDefinitionResolver, ComponentDefinitionResolver>();
builder.Services.AddSingleton<IChatSessionStore, DistributedCacheChatSessionStore>();
builder.Services.AddSingleton<IKernelPluginManager, KernelPluginManager>();
builder.Services.AddTransient<IKernelFactory, KernelFactory>();
builder.Services.AddTransient<IAgentFactory, AgentFactory>();
builder.Services.AddJsonPatchHandler();
builder.Services.AddJsonMergePatchHandler();
builder.Services.AddJsonStrategicMergePatchHandler();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddControllers(options =>
{
#pragma warning disable ASP0000
    var provider = builder.Services.BuildServiceProvider();
#pragma warning restore ASP0000
    var yamlSerializer = provider.GetRequiredService<IYamlSerializer>();
    options.Filters.Add<ProblemDetailsExceptionFilter>();
    options.InputFormatters.Add(new YamlInputFormatter(yamlSerializer));
    options.OutputFormatters.Add(new YamlOutputFormatter(yamlSerializer));
}).AddJsonOptions(options =>
{
    JsonSerializer.DefaultOptionsConfiguration(options.JsonSerializerOptions);
});

var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
});
app.MapOpenApi();
app.MapScalarApiReference("/api/doc", options =>
{
    options.WithTitle("DClare Runtime API");
});
app.MapControllers();
app.MapA2AWellKnownAgentEndpoint();
app.MapA2AAgentHttpEndpoint();
await app.RunAsync();

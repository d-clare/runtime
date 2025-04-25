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

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddYamlFile("manifest.yaml", true);
builder.Services.AddOptions<ApplicationOptions>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services
    .AddControllers(options =>
{
    options.Filters.Add<ProblemDetailsExceptionFilter>();
})
    .AddJsonOptions(options =>
{
    JsonSerializer.DefaultOptionsConfiguration(options.JsonSerializerOptions);
});
builder.Services.AddOpenApi();
builder.Services.AddDClare();
builder.Services.AddMediator(options =>
{
    options.ScanAssembly(typeof(ApplicationOptions).Assembly);
});
builder.Services.AddHttpClient();
builder.Services.AddA2AAgents(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.AddSingleton<IOAuth2TokenManager, OAuth2TokenManager>();
builder.Services.AddSingleton<IChatHistoryManager, ChatHistoryManager>();
builder.Services.AddSingleton<IKernelPluginManager, KernelPluginManager>();
builder.Services.AddTransient<IKernelFactory, KernelFactory>();
builder.Services.AddTransient<IAgentFactory, AgentFactory>();
builder.Services.AddTransient<IProcessFactory, ProcessFactory>();
builder.Services.AddTransient<IKernelFunctionStrategyFactory, KernelFunctionStrategyFactory>();

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference("/api/doc", options =>
{
    options.WithTitle("DClare Runtime API");
});
app.MapControllers();
app.MapA2AWellKnownAgentEndpoint();
app.MapA2AAgentHttpEndpoint();

await app.RunAsync();

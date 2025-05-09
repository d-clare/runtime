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

global using DClare.Runtime.Application.Configuration;
global using DClare.Runtime.Application.Services;
global using DClare.Runtime.Infrastructure.Services;
global using DClare.Runtime.Integration.Models;
global using DClare.Runtime.Integration.Resources;
global using DClare.Sdk;
global using DClare.Sdk.Models;
global using Duende.IdentityModel;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.SemanticKernel;
global using Neuroglia;
global using Neuroglia.Data.Infrastructure.ResourceOriented;
global using Neuroglia.Data.Infrastructure.ResourceOriented.Services;
global using Neuroglia.Mediation;
global using Neuroglia.Security.Services;
global using Neuroglia.Serialization;
global using System.Collections.Concurrent;
global using System.Linq.Expressions;
global using System.Net;
global using System.Net.Mime;
global using System.Runtime.CompilerServices;
global using System.Security.Claims;
global using System.Text;
global using YamlDotNet.Serialization;

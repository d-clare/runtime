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

using System.Security.Cryptography;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the service used to initialize the Synapse resource database
/// </summary>
/// <inheritdoc/>
public class DatabaseInitializer(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    : Neuroglia.Data.Infrastructure.ResourceOriented.Services.DatabaseInitializer(loggerFactory, serviceProvider)
{

    /// <inheritdoc/>
    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        try
        {
            await InitializeAsync(stoppingToken).ConfigureAwait(false);
        }
        catch (ProblemDetailsException ex) when (ex.Problem.Status == (int)HttpStatusCode.Conflict || (ex.Problem.Status == (int)HttpStatusCode.BadRequest && ex.Problem.Title == "Conflict")) { }
    }

    /// <inheritdoc/>
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        foreach (var definition in DClareResources.Definitions.AsEnumerable())
        {
            try { await Database.CreateResourceAsync(definition, cancellationToken: cancellationToken).ConfigureAwait(false); }
            catch (ProblemDetailsException ex) when (ex.Problem.Status == (int)HttpStatusCode.Conflict || (ex.Problem.Status == (int)HttpStatusCode.BadRequest && ex.Problem.Title == "Conflict")) { continue; }
        }
    }

}

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

using Microsoft.Extensions.Caching.Distributed;

namespace DClare.Runtime.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IManifestHandler"/> interface
/// </summary>
/// <param name="yamlSerializer">The service used to serialize/deserialize data to/from YAML</param>
/// <param name="cache">The service used to cache data</param>
public class ManifestHandler(IYamlSerializer yamlSerializer, IDistributedCache cache)
    : IManifestHandler
{

    const string ManifestFilePath = "manifest.yaml";

    /// <summary>
    /// Gets the service used to serialize/deserialize data to/from YAML
    /// </summary>
    protected IYamlSerializer YamlSerializer { get; } = yamlSerializer;

    /// <summary>
    /// Gets the service used to cache data
    /// </summary>
    protected IDistributedCache Cache { get; } = cache;

    /// <summary>
    /// Gets the application's manifest
    /// </summary>
    protected Manifest? Manifest { get; private set; }

    /// <inheritdoc/>
    public virtual async Task<Manifest> GetManifestAsync(CancellationToken cancellationToken = default)
    {
        if (Manifest == null)
        {
            var yaml = File.ReadAllText(ManifestFilePath);
            Manifest = YamlSerializer.Deserialize<Manifest>(yaml)!;
            yaml = await Cache.GetStringAsync(BuildManifestCacheKey(Manifest.Metadata.Name), cancellationToken).ConfigureAwait(false);
            if(!string.IsNullOrWhiteSpace(yaml)) Manifest = YamlSerializer.Deserialize<Manifest>(yaml)!;
        }
        return Manifest;
    }

    /// <inheritdoc/>
    public virtual async Task SetManifestAsync(Manifest manifest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(manifest);
        Manifest = manifest;
        var yaml = YamlSerializer.SerializeToText(manifest);
        File.WriteAllText(ManifestFilePath, yaml);
        await Cache.SetStringAsync(BuildManifestCacheKey(Manifest.Metadata.Name), yaml, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Generate a new cache key used to store the application's manifest
    /// </summary>
    /// <param name="name">The name of the manifest to store</param>
    /// <returns>A new cache key used to store the application's manifest</returns>
    protected virtual string BuildManifestCacheKey(string name) => $"dclare-manifest:{name}";

}

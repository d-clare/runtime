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

namespace DClare.Runtime.Integration.Resources;

/// <summary>
/// Represents a resource used to describe and configure an embedding model.
/// </summary>
[Description("Represents a resource used to describe and configure an embedding model.")]
[DataContract]
public record EmbeddingModel
    : Resource<EmbeddingModelSpec>
{

    /// <summary>
    /// Gets the <see cref="EmbeddingModel"/> resource definition.
    /// </summary>
    public static readonly ResourceDefinitionInfo ResourceDefinition = new EmbeddingModelResourceDefinition()!;

    /// <inheritdoc/>
    public EmbeddingModel() : base(ResourceDefinition) { }

    /// <inheritdoc/>
    public EmbeddingModel(ResourceMetadata metadata, EmbeddingModelSpec spec) : base(ResourceDefinition, metadata, spec) { }

}

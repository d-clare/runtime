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

namespace DClare.Runtime;

/// <summary>
/// Exposes information about DClare resources.
/// </summary>
public static class DClareResources
{

    /// <summary>
    /// Exposes DClare resource definitions.
    /// </summary>
    public static class Definitions
    {

        /// <summary>
        /// Gets the definition of Agent resources
        /// </summary>
        public static ResourceDefinition Agent { get; } = new AgentResourceDefinition();

        /// <summary>
        /// Gets the definition of Embedding Model resources
        /// </summary>
        public static ResourceDefinition EmbeddingModel { get; } = new EmbeddingModelResourceDefinition();

        /// <summary>
        /// Gets the definition of Agent resources
        /// </summary>
        public static ResourceDefinition VectorStore { get; } = new VectorStoreResourceDefinition();

        /// <summary>
        /// Gets the definition of Workflow resources
        /// </summary>
        public static ResourceDefinition Workflow { get; } = new WorkflowResourceDefinition();

        /// <summary>
        /// Gets the definition of WorkflowInstance resources
        /// </summary>
        public static ResourceDefinition WorkflowInstance { get; } = new WorkflowInstanceResourceDefinition();

        /// <summary>
        /// Gets a new <see cref="IEnumerable{T}"/> containing DClare default resource definitions
        /// </summary>
        /// <returns>A new <see cref="IEnumerable{T}"/> containing DClare default resource definitions</returns>
        public static IEnumerable<ResourceDefinition> AsEnumerable()
        {
            yield return Agent;
            yield return EmbeddingModel;
            yield return VectorStore;
            yield return Workflow;
            yield return WorkflowInstance;
        }

    }

}

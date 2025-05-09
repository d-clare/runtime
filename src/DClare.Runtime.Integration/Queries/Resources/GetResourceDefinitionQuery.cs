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

namespace DClare.Runtime.Integration.Queries.Resources;

/// <summary>
/// Represents the <see cref="IQuery{TResult}"/> used to get the definition of the specified <see cref="IResource"/> type.
/// </summary>
/// <typeparam name="TResource">The type of the <see cref="IResource"/> to get the definition of.</typeparam>
[Description("Represents the query used to get the definition of the specified resource type.")]
public class GetResourceDefinitionQuery<TResource>
    : Query<IResourceDefinition>
    where TResource : class, IResource, new()
{



}

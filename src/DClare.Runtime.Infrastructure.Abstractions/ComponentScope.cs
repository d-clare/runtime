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
/// Enumerates all supported component scopes.
/// </summary>
public static class ComponentScope
{

    /// <summary>
    /// Indicates a contextual component.
    /// </summary>
    public const string Contextual = "contextual";
    /// <summary>
    /// Indicates a global component.
    /// </summary>
    public const string Global = "global";

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> containing all supported values.
    /// </summary>
    /// <returns>A <see cref="IEnumerable{T}"/> containing all supported values.</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Contextual;
        yield return Global;
    }

}
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
/// Enumerates the supported operation types for JSON Patch documents, as defined by RFC 6902.
/// </summary>
public static class JsonPatchOperationType
{

    /// <summary>
    /// The "add" operation adds a value to the target location.
    /// </summary>
    public const string Add = "add";
    /// <summary>
    /// The "remove" operation removes the value at the target location.
    /// </summary>
    public const string Remove = "remove";
    /// <summary>
    /// The "replace" operation replaces the value at the target location with a new value.
    /// </summary>
    public const string Replace = "replace";
    /// <summary>
    /// The "move" operation moves a value from a source location to a target location.
    /// </summary>
    public const string Move = "move";
    /// <summary>
    /// The "copy" operation copies a value from a source location to a target location.
    /// </summary>
    public const string Copy = "copy";
    /// <summary>
    /// The "test" operation tests that a value at the target location is equal to a specified value.
    /// </summary>
    public const string Test = "test";

    /// <summary>
    /// Returns a new <see cref="IEnumerable{T}"/> containing all supported values.
    /// </summary>
    /// <returns>A new <see cref="IEnumerable{T}"/> containing all supported values.</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Add;
        yield return Remove;
        yield return Replace;
        yield return Move;
        yield return Copy;
        yield return Test;
    }

}

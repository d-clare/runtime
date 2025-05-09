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

namespace DClare.Runtime.Application;

/// <summary>
/// Defines extensions for <see cref="ISerializer"/>s
/// </summary>
public static class ISerializerExtensions
{

    /// <summary>
    /// Converts the specified value into a new value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to convert the specified value to.</typeparam>
    /// <param name="serializer">The extended <see cref="ISerializer"/>.</param>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static T? Convert<T>(this Neuroglia.Serialization.ISerializer serializer, object? value)
    {
        if (value == null) return default;
        return (T?)serializer.Convert(value, typeof(T));
    }

}
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
/// Enumerates default task statuses
/// </summary>
public static class TaskInstanceStatus
{

    /// <summary>
    /// Indicates that the task has been created and is pending execution
    /// </summary>
    public const string Pending = "pending";
    /// <summary>
    /// Indicates that the task is running
    /// </summary>
    public const string Running = "running";
    /// <summary>
    /// Indicates that the task encountered an error or exception during execution
    /// </summary>
    public const string Faulted = "faulted";
    /// <summary>
    /// Indicates that the task has been explicitly omitted or bypassed during execution, probably because that task failed the condition defined by its `if` property
    /// </summary>
    public const string Skipped = "skipped";
    /// <summary>
    /// Indicates that the task was suspended
    /// </summary>
    public const string Suspended = "suspended";
    /// <summary>
    /// Indicates that the task was terminated or aborted before completion
    /// </summary>
    public const string Cancelled = "cancelled";
    /// <summary>
    /// Indicates that the task ran to completion
    /// </summary>
    public const string Completed = "completed";

    /// <summary>
    /// Returns a new <see cref="IEnumerable{T}"/> containing all supported values.
    /// </summary>
    /// <returns>A new <see cref="IEnumerable{T}"/> containing all supported values.</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Pending;
        yield return Running;
        yield return Faulted;
        yield return Skipped;
        yield return Cancelled;
        yield return Completed;
    }

}

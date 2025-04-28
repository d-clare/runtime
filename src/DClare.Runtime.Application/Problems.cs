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
/// Exposes information about the problems that can occur during the application's execution
/// </summary>
public static class Problems
{

    /// <summary>
    /// Exposes the types of DClare Runtime related problems
    /// </summary>
    public static class Types
    {

        static readonly Uri BaseUri = new("https://runtime.d-clare.ai/problems/types/");

        /// <summary>
        /// Exposes the types of DClare Runtime problems related to agents
        /// </summary>
        public static class Agents
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "agents/");

            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find an agent
            /// </summary>
            public static readonly Uri NotFound = new(BaseUri, "not-found");
            /// <summary>
            /// Gets the type of problems that occur when an error is raise during communication with a remote agent
            /// </summary>
            public static readonly Uri CommunicationError = new(BaseUri, "communication-error");
            /// <summary>
            /// Gets the type of problems that occur due to invalid or incomplete agent configuration
            /// </summary>
            public static readonly Uri InvalidConfiguration = new(BaseUri, "invalid-configuration");

        }

        /// <summary>
        /// Exposes the types of DClare Runtime problems related to agentic processes
        /// </summary>
        public static class AgenticProcesses
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "agentic-processes/");

            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find an agentic process
            /// </summary>
            public static readonly Uri NotFound = new(BaseUri, "not-found");

        }

        /// <summary>
        /// Exposes the types of DClare Runtime problems related to kernels
        /// </summary>
        public static class Kernels
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "kernels");

            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find data
            /// </summary>
            public static readonly Uri NotFound = new(BaseUri, "not-found");

        }

        /// <summary>
        /// Exposes the types of DClare Runtime problems related to toolsets
        /// </summary>
        public static class Toolsets
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "toolsets/");

            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find a toolset
            /// </summary>
            public static readonly Uri NotFound = new(BaseUri, "not-found");

        }

        /// <summary>
        /// Gets the type of problems that occur due to unprocessable or invalid data
        /// </summary>
        public static readonly Uri Unprocessable = new(BaseUri, "unprocessable");

    }

    /// <summary>
    /// Exposes the titles of DClare Runtime related problems
    /// </summary>
    public static class Titles
    {

        /// <summary>
        /// Exposes the titles of DClare Runtime problems related to agents
        /// </summary>
        public static class Agents
        {

            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find an agent
            /// </summary>
            public const string NotFound = "Agent Not Found";
            /// <summary>
            /// Gets the title of problems that occur when an error is raise during communication with a remote agent
            /// </summary>
            public const string CommunicationError = "Agent Communication Error";
            /// <summary>
            /// Gets the title of problems that occur due to invalid or missing agent configuration
            /// </summary>
            public const string InvalidConfiguration = "Invalid Configuration";

        }

        /// <summary>
        /// Exposes the titles of DClare Runtime problems related to agentic processes
        /// </summary>
        public static class AgenticProcesses
        {

            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find an agentic process
            /// </summary>
            public const string NotFound = "Agentic Process Not Found";

        }

        /// <summary>
        /// Exposes the titles of DClare Runtime problems related to kernels
        /// </summary>
        public static class Kernels
        {

            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find a kernel
            /// </summary>
            public const string NotFound = "Kernel Not Found";

        }

        /// <summary>
        /// Exposes the titles of DClare Runtime problems related to toolsets
        /// </summary>
        public static class Toolsets
        {

            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find a toolset
            /// </summary>
            public const string NotFound = "Toolset Not Found";

        }

        /// <summary>
        /// Gets the title of problems that occur due to unprocessable or invalid data
        /// </summary>
        public const string Unprocessable = "Unprocessable";

    }

    /// <summary>
    /// Exposes the statuses of DClare Runtime related problems
    /// </summary>
    public static class Statuses
    {

        /// <summary>
        /// Gets the status of problems that occur when the runtime fails to communicate with a remote agent or service, typically due to an invalid response or an unreachable endpoint
        /// </summary>
        public const int BadGateway = 502;
        /// <summary>
        /// Gets the status of problems that occur due to unprocessable or invalid data
        /// </summary>
        public const int Unprocessable = 422;
        /// <summary>
        /// Gets the status of problems that occur when the runtime fails to find data
        /// </summary>
        public const int NotFound = 404;

    }

    /// <summary>
    /// Exposes the details of DClare Runtime related problems
    /// </summary>
    public static class Details
    {

        /// <summary>
        /// Exposes the details of problems related to agents
        /// </summary>
        public static class Agents
        {

            /// <summary>
            /// Gets the detail of a problem that occurs when the runtime fails to find a specific agent
            /// </summary>
            public const string NotFound = "Failed to found an agent with the specified name '{name}'";
            /// <summary>
            /// Gets the detail of a problem that occurs when an error is raise during communication with a remote agent
            /// </summary>
            public const string CommunicationError = "An error occurred while communicating with the remote agent '{name}': {error}";
            /// <summary>
            /// Gets the detail of a problem that occurs due to invalid or missing agent configuration
            /// </summary>
            public const string InvalidConfiguration = "The configuration of agent '{name}' is invalid or incomplete.";

        }

        /// <summary>
        /// Exposes the details of problems related to agentic processes
        /// </summary>
        public static class AgenticProcesses
        {

            /// <summary>
            /// Gets the detail of a problem that occurs when the runtime fails to find a specific agentic process
            /// </summary>
            public const string NotFound = "Failed to found an agentic process with the specified name '{name}'";

        }

        /// <summary>
        /// Exposes the details of problems related to kernels
        /// </summary>
        public static class Kernels
        {

            /// <summary>
            /// Gets the detail of a problem that occurs when the runtime fails to find a specific kernel
            /// </summary>
            public const string NotFound = "Failed to found a kernel with the specified name '{name}'";

        }

        /// <summary>
        /// Exposes the details of problems related to toolsets
        /// </summary>
        public static class Toolsets
        {

            /// <summary>
            /// Gets the detail of a problem that occurs when the runtime fails to find a specific toolset
            /// </summary>
            public const string NotFound = "Failed to found a toolset with the specified name '{name}'";

        }

        /// <summary>
        /// Gets the details of a problem that occurs when attempting to apply an unsupported type of patch
        /// </summary>
        public const string UnsupportedPatchType = "The specified patch type '{type}' is not supported";

    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific agent
    /// </summary>
    /// <param name="name">The name of the agent that cannot be found</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails AgentNotFound(string name) => new(Types.Agents.NotFound, Titles.Agents.NotFound, Statuses.NotFound, StringFormatter.Format(Details.Agents.NotFound, name));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe an error that occurred while communicating with a remote agent
    /// </summary>
    /// <param name="name">The name of the kernel that cannot be found</param>
    /// <param name="error">The error that has occurred</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails AgentCommunicationError(string name, string error) => new(Types.Agents.CommunicationError, Titles.Agents.CommunicationError, Statuses.BadGateway, StringFormatter.Format(Details.Agents.CommunicationError, name, error));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific process
    /// </summary>
    /// <param name="name">The name of the process that cannot be found</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails AgenticProcessNotFound(string name) => new(Types.AgenticProcesses.NotFound, Titles.AgenticProcesses.NotFound, Statuses.NotFound, StringFormatter.Format(Details.AgenticProcesses.NotFound, name));

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> describing a configuration issue
    /// </summary>
    /// <param name="name">The name of the affected component (e.g., agent, kernel)</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails InvalidConfiguration(string name) => new(Types.Agents.InvalidConfiguration, Titles.Agents.InvalidConfiguration, Statuses.Unprocessable, StringFormatter.Format(Details.Agents.InvalidConfiguration, name));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific kernel
    /// </summary>
    /// <param name="name">The name of the kernel that cannot be found</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails KernelNotFound(string name) => new(Types.Kernels.NotFound, Titles.Kernels.NotFound, Statuses.NotFound, StringFormatter.Format(Details.Kernels.NotFound, name));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific toolset
    /// </summary>
    /// <param name="name">The name of the toolset that cannot be found</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails ToolsetNotFound(string name) => new(Types.Toolsets.NotFound, Titles.Toolsets.NotFound, Statuses.NotFound, StringFormatter.Format(Details.Toolsets.NotFound, name));

    public static ProblemDetails UnsupportedPatchType(string type) => new(Types.Unprocessable, Titles.Unprocessable, Statuses.Unprocessable, StringFormatter.Format(Details.UnsupportedPatchType, type));

}

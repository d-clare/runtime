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

#pragma warning disable IDE0130 // Namespace does not match folder structure

using Neuroglia.Data.Infrastructure.ResourceOriented;

namespace DClare.Runtime;

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
        /// Exposes the types of DClare Runtime problems related to components.
        /// </summary>
        public static class Components
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "components/");

            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find a component.
            /// </summary>
            public static readonly Uri NotFound = new(BaseUri, "not-found");

        }

        /// <summary>
        /// Exposes the types of DClare Runtime problems related to resources.
        /// </summary>
        public static class Resources
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "resources/");

            /// <summary>
            /// Gets the type of problems that occur when processing an invalid resource label selector
            /// </summary>
            public static readonly Uri InvalidLabelSelector = new(BaseUri, "invalid-label-selector");
            /// <summary>
            /// Gets the type of problems that occur when processing an invalid resource qualified name.
            /// </summary>
            public static readonly Uri InvalidQualifiedName = new(BaseUri, "invalid-qualified-name");
            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find a resource
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
        /// Exposes the types of DClare Runtime problems related to workflow definitions.
        /// </summary>
        public static class WorkflowDefinitions
        {

            static readonly Uri BaseUri = new(Types.BaseUri, "workflows-definitions/");

            /// <summary>
            /// Gets the type of problems that occur when the runtime fails to find a workflow definition.
            /// </summary>
            public static readonly Uri NotFound = new(BaseUri, "not-found");

        }

        /// <summary>
        /// Gets the type of problems that occur due to a forbidden action
        /// </summary>
        public static readonly Uri Forbidden = new(BaseUri, "forbidden");
        /// <summary>
        /// Gets the type of problems that occur due to an unauthorized action
        /// </summary>
        public static readonly Uri Unauthorized = new(BaseUri, "unauthorized");
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
        /// Exposes the titles of DClare Runtime problems related to components.
        /// </summary>
        public static class Components
        {

            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find a component.
            /// </summary>
            public const string NotFound = "{ComponentType} Not Found";

        }

        /// <summary>
        /// Exposes the titles of DClare Runtime problems related to resources.
        /// </summary>
        public static class Resources
        {

            /// <summary>
            /// Gets the title of problems that occur when processing an invalid resource label selector.
            /// </summary>
            public const string InvalidLabelSelector = "Invalid Resource Label Selector";
            /// <summary>
            /// Gets the title of problems that occur when processing an invalid resource qualified name.
            /// </summary>
            public const string InvalidQualifiedName = "Invalid Namespaced Resource Qualified Name";
            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find a resource
            /// </summary>
            public const string NotFound = "Resource Not Found";

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
        /// Exposes the titles of DClare Runtime problems related to workflow definitions.
        /// </summary>
        public static class WorkflowDefinitions
        {

            /// <summary>
            /// Gets the title of problems that occur when the runtime fails to find a workflow definition.
            /// </summary>
            public const string NotFound = "Workflow Definition Not Found";

        }

        /// <summary>
        /// Gets the title of problems that occur due to a forbidden action
        /// </summary>
        public const string Forbidden = "Forbidden";
        /// <summary>
        /// Gets the title of problems that occur due to an unauthorized action
        /// </summary>
        public const string Unauthorized = "Unauthorized";
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
        /// Gets the status of problems that occur due to a forbidden action
        /// </summary>
        public const int Forbidden = 403;
        /// <summary>
        /// Gets the status of problems that occur due to an unauthorized action
        /// </summary>
        public const int Unauthorized = 401;
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
        /// Exposes the details of problems related to components
        /// </summary>
        public static class Components
        {

            /// <summary>
            /// Gets the detail of a problem that occurs when the runtime fails to find a specific component.
            /// </summary>
            public const string NotFound = "Failed to found a component of type '{type}' with the specified reference '{reference}'";

        }

        /// <summary>
        /// Exposes the details of DClare Runtime problems related to resources.
        /// </summary>
        public static class Resources
        {

            /// <summary>
            /// Gets the detail of problems that occur when processing an invalid resource label selector.
            /// </summary>
            public const string InvalidLabelSelector = "The specified value '{selector}' is not a valid resource label selector";
            /// <summary>
            /// Gets the detail of problems that occur when processing an invalid resource qualified name.
            /// </summary>
            public const string InvalidQualifiedName = "The specified value '{name}' is not a valid namespaced resource qualified name";

            /// <summary>
            /// Exposes the details of DClare Runtime problems related to namespaced resources.
            /// </summary>
            public static class Namespaced
            {

                /// <summary>
                /// Gets the detail of a problem that occurs when the runtime fails to find a specific resource
                /// </summary>
                public const string NotFound = "Failed to found a resource of type '{type}' with the specified name '{name}.{namespace}'";

            }

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
        /// Exposes the details of DClare Runtime problems related to workflow definitions.
        /// </summary>
        public static class WorkflowDefinitions
        {

            /// <summary>
            /// Gets the detail of a problem that occurs when the runtime fails to find a specific workflow definition.
            /// </summary>
            public const string NotFound = "Failed to found a workflow definition with the specified name '{name}.{namespace}' and version '{version}'";

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
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific component.
    /// </summary>
    /// <typeparam name="TComponent">The type of component that cannot be found.</typeparam>
    /// <param name="reference">The reference to the component that cannot be found.</param>
    /// <returns>A new <see cref="ProblemDetails"/>.</returns>
    public static ProblemDetails ComponentNotFound<TComponent>(string reference) where TComponent : ReferenceableComponentDefinition => new(Types.Components.NotFound, StringFormatter.Format(Titles.Components.NotFound, typeof(TComponent).Name), Statuses.NotFound, StringFormatter.Format(Details.Components.NotFound, typeof(TComponent).Name, reference));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the trying to execute a forbidden action
    /// </summary>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails Forbidden() => new(Types.Forbidden, Titles.Forbidden, Statuses.Forbidden);

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the trying to execute an unauthorized action
    /// </summary>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails Unauthorized() => new(Types.Unauthorized, Titles.Unauthorized, Statuses.Unauthorized);

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> describing a configuration issue
    /// </summary>
    /// <param name="name">The name of the affected component (e.g., agent, kernel)</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails InvalidConfiguration(string name) => new(Types.Agents.InvalidConfiguration, Titles.Agents.InvalidConfiguration, Statuses.Unprocessable, StringFormatter.Format(Details.Agents.InvalidConfiguration, name));

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> describing an invalid label selector
    /// </summary>
    /// <param name="labelSelector">The invalid label selector</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails InvalidLabelSelector(string labelSelector) => new(Types.Resources.InvalidLabelSelector, Titles.Resources.InvalidLabelSelector, Statuses.Unprocessable, StringFormatter.Format(Details.Resources.InvalidLabelSelector, labelSelector));

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> describing an error that occurred due to an invalid resource qualified name.
    /// </summary>
    /// <param name="name">The invalid resource qualified name.</param>
    /// <returns>A new <see cref="ProblemDetails"/>.</returns>
    public static ProblemDetails InvalidQualifiedName(string name) => new(Types.Resources.InvalidQualifiedName, Titles.Resources.InvalidQualifiedName, Statuses.Unprocessable, StringFormatter.Format(Details.Resources.InvalidQualifiedName, name));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that cannot be found.</typeparam>
    /// <param name="name">The name of the resource that cannot be found.</param>
    /// <param name="namespace">The namespace the resource that cannot be found belongs to.</param>
    /// <returns>A new <see cref="ProblemDetails"/>.</returns>
    public static ProblemDetails ResourceNotFound<TResource>(string name, string @namespace) where TResource : IResource => new(Types.Resources.NotFound, Titles.Resources.NotFound, Statuses.NotFound, StringFormatter.Format(Details.Resources.Namespaced.NotFound, typeof(TResource).Name, name, @namespace));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific toolset
    /// </summary>
    /// <param name="name">The name of the toolset that cannot be found</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails ToolsetNotFound(string name) => new(Types.Toolsets.NotFound, Titles.Toolsets.NotFound, Statuses.NotFound, StringFormatter.Format(Details.Toolsets.NotFound, name));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when attempting to apply a patch of unsupported type
    /// </summary>
    /// <param name="type">The unsupported patch type</param>
    /// <returns>A new <see cref="ProblemDetails"/></returns>
    public static ProblemDetails UnsupportedPatchType(string type) => new(Types.Unprocessable, Titles.Unprocessable, Statuses.Unprocessable, StringFormatter.Format(Details.UnsupportedPatchType, type));

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> used to describe a problem that occurs when the runtime fails to find a specific workflow definition.
    /// </summary>
    /// <param name="name">The name of the workflow definition that cannot be found.</param>
    /// <param name="namespace">The namespace the workflow definition that cannot be found belongs to.</param>
    /// <param name="version">The version of the workflow definition that cannot be found.</param>
    /// <returns>A new <see cref="ProblemDetails"/>.</returns>
    public static ProblemDetails WorkflowDefinitionNotFound(string name, string @namespace, string version) => new(Types.WorkflowDefinitions.NotFound, Titles.WorkflowDefinitions.NotFound, Statuses.NotFound, StringFormatter.Format(Details.WorkflowDefinitions.NotFound, name, @namespace, version));

}

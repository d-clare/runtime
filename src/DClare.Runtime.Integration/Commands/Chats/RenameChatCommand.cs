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

namespace DClare.Runtime.Integration.Commands.Chats;

/// <summary>
/// Represents the command used to rename a chat
/// </summary>
[DataContract, Description("The command used to rename a chat")]
public class RenameChatCommand
    : Command
{

    /// <summary>
    /// Gets/sets the unique key of the chat to rename
    /// </summary>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual string Key { get; set; } = null!;

    /// <summary>
    /// Gets/sets the chat's name
    /// </summary>
    [Description("The chat's name")]
    [Required, MinLength(1)]
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual required string Name { get; set; }

}

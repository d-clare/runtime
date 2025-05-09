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

using System.Text.Json;

namespace DClare.Runtime.Integration.Serialization.Json;

/// <summary>
/// Represents an <see cref="JsonConverter"/> used to convert <see cref="Message"/> to and from JSON.
/// </summary>
public class JsonMessageConverter
    : JsonConverter<Message>
{

    /// <inheritdoc/>
    public override Message? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        string? role = null;
        string? content = null;
        List<MessagePart> parts = [];
        Dictionary<string, object?>? metadata = null;
        Dictionary<string, object>? extensionData = null;
        foreach (var property in root.EnumerateObject())
        {
            switch (property.Name)
            {
                case "role":
                    role = property.Value.GetString();
                    break;
                case "content":
                    content = property.Value.GetString();
                    break;
                case "parts":
                    parts = JsonSerializer.Deserialize<List<MessagePart>>(property.Value.GetRawText(), options)!;
                    break;
                case "metadata":
                    metadata = JsonSerializer.Deserialize<Dictionary<string, object?>>(property.Value.GetRawText(), options);
                    break;
                default:
                    extensionData ??= [];
                    extensionData[property.Name] = JsonSerializer.Deserialize<object>(property.Value.GetRawText(), options)!;
                    break;
            }
        }
        var finalParts = parts;
        if (content != null)
        {
            finalParts.Insert(0, new TextPart
            {
                MimeType = MediaTypeNames.Text.Plain,
                Text = content
            });
        }
        return new Message
        {
            Role = role ?? throw new JsonException("Missing required 'role' property."),
            Parts = finalParts,
            Metadata = metadata,
            ExtensionData = extensionData
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new SerializableMessage()
        {
            Role = value.Role,
            Author = value.Author,
            Content = value.Content,
            Parts = value.Parts,
            Metadata = value.Metadata,
            ExtensionData = value.ExtensionData,
        }, options);
    }

    record SerializableMessage
        : Message
    {


    }

}

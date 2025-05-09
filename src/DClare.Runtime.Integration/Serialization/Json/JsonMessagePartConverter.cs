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
/// Represents an <see cref="JsonConverter"/> used to convert <see cref="MessagePart"/> to and from JSON.
/// </summary>
public class JsonMessagePartConverter
    : JsonConverter<MessagePart>
{

    /// <inheritdoc/>
    public override MessagePart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        bool hasText = root.TryGetProperty(nameof(TextPart.Text).ToCamelCase(), out _);
        bool hasUri = root.TryGetProperty(nameof(BinaryPart.Uri).ToCamelCase(), out _);
        bool hasData = root.TryGetProperty(nameof(BinaryPart.Data).ToCamelCase(), out _);
        bool hasQuote = root.TryGetProperty(nameof(AnnotationPart.Quote).ToCamelCase(), out _);
        if (hasText) return JsonSerializer.Deserialize<TextPart>(root.GetRawText(), options);
        if (hasUri || hasData) return JsonSerializer.Deserialize<BinaryPart>(root.GetRawText(), options);
        if (hasQuote) return JsonSerializer.Deserialize<AnnotationPart>(root.GetRawText(), options);
        throw new JsonException($"Unable to determine {nameof(MessagePart)} type.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, MessagePart value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);

}

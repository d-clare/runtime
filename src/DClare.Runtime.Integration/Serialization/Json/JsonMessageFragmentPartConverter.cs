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
/// Represents an <see cref="JsonConverter"/> used to convert <see cref="MessageFragmentPart"/> to and from JSON.
/// </summary>
public class JsonMessageFragmentPartConverter
    : JsonConverter<MessageFragmentPart>
{

    /// <inheritdoc/>
    public override MessageFragmentPart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        bool hasText = root.TryGetProperty(nameof(TextFragmentPart.Text).ToCamelCase(), out _);
        bool hasUri = root.TryGetProperty(nameof(BinaryFragmentPart.Uri).ToCamelCase(), out _);
        bool hasData = root.TryGetProperty(nameof(BinaryPart.Data).ToCamelCase(), out _);
        bool hasQuote = root.TryGetProperty(nameof(AnnotationFragmentPart.Quote).ToCamelCase(), out _);
        if (hasText) return JsonSerializer.Deserialize<TextFragmentPart>(root.GetRawText(), options);
        if (hasUri || hasData) return JsonSerializer.Deserialize<BinaryFragmentPart>(root.GetRawText(), options);
        if (hasQuote) return JsonSerializer.Deserialize<AnnotationFragmentPart>(root.GetRawText(), options);
        throw new JsonException($"Unable to determine {nameof(MessageFragmentPart)} type.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, MessageFragmentPart value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);

}

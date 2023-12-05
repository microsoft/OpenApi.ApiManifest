// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.OpenApi.ApiManifest;

public class Extensions : Dictionary<string, JsonNode?>
{
    public Extensions() : base(StringComparer.OrdinalIgnoreCase) { }

    public static Extensions Load(JsonElement value)
    {
        var extensions = new Extensions();
        foreach (var property in value.EnumerateObject())
        {
            if (property.Value.ValueKind != JsonValueKind.Null)
            {
                var extensionValue = JsonSerializer.Deserialize<JsonNode>(property.Value.GetRawText());
                extensions.Add(property.Name, extensionValue);
            }
        }
        return extensions;
    }

    public void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();
        foreach (var extension in this)
        {
            writer.WritePropertyName(extension.Key);
            if (extension.Value is not null)
                writer.WriteRawValue(extension.Value.ToJsonString());
        }
        writer.WriteEndObject();
    }
}
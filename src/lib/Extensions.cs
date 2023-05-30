using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.OpenApi.ApiManifest;

public class Extensions : Dictionary<string, JsonNode>
{
    public static Extensions Load(JsonElement value)
    {
        var extensions = new Extensions();
        foreach(var property in value.EnumerateObject())
        {
            if (property.Value.ValueKind != JsonValueKind.Null) {
                extensions.Add(property.Name, JsonSerializer.Deserialize<JsonObject>(property.Value.GetRawText()));
            } 
        }
        return extensions;
    }

    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        foreach(var extension in this)
        {
            writer.WritePropertyName(extension.Key);
            writer.WriteRawValue(extension.Value.ToJsonString());
        }
        writer.WriteEndObject();
    }    
}
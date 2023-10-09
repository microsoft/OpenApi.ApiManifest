using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.OpenApi.ApiManifest;

public class AccessRequest
{
    // TODO: Add validation. Type is required and is unique for the described API according to RAR - https://www.rfc-editor.org/rfc/rfc9396.
    private const string TypeProperty = "type";
    // TODO: Rename to 'actions' to match RAR spec.
    private const string ContentProperty = "content";

    public string? Type { get; set; }
    public JsonObject? Content { get; set; }

    internal static AccessRequest Load(JsonElement content)
    {
        var accessRequest = new AccessRequest();
        ParsingHelpers.ParseMap(content, accessRequest, handlers);
        return accessRequest;

    }
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        if (!String.IsNullOrWhiteSpace(Type)) writer.WriteString(TypeProperty, Type);
        if (Content != null)
        {
            writer.WritePropertyName(ContentProperty);
            writer.WriteRawValue(Content.ToJsonString());
        }
        writer.WriteEndObject();
    }

    private static readonly FixedFieldMap<AccessRequest> handlers = new()
    {
        { TypeProperty, (o,v) => {o.Type = v.GetString(); } },
        { ContentProperty, (o,v) => {o.Content = JsonSerializer.Deserialize<JsonObject>(v);  } },
    };
}
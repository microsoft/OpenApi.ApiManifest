
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class Api
{
    public string? Type { get; set; }
    public string? Url { get; set; }
    public bool? IsUserAuthenticated { get; set; }

    public static Api Load(JsonElement value)
    {
        var api = new Api();
        ParsingHelpers.ParseMap(value, api, handlers);
        return api;
    }

    // Create handlers FixedFieldMap for Api
    private static readonly FixedFieldMap<Api> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "url", (o,v) => {o.Url = v.GetString();  } },
        { "is_user_authenticated", (o,v) => {o.IsUserAuthenticated = v.GetBoolean(); }},
    };

    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        writer.WriteString("url", Url);
        writer.WriteBoolean("is_user_authenticated", IsUserAuthenticated ?? false);
        writer.WriteEndObject();
    }
}



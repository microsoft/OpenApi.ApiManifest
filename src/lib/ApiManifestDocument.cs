using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class ApiManifestDocument
{
    public Publisher? Publisher { get; set; }
    public List<ApiDependency> ApiDependencies { get; set; } = new List<ApiDependency>();

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (Publisher != null) writer.WritePropertyName("publisher");
        Publisher?.Write(writer);

        if (ApiDependencies.Count > 0) writer.WritePropertyName("apiDependencies");
        writer.WriteStartArray();
        foreach (var apiDependency in ApiDependencies)
        {
            apiDependency.Write(writer);
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
    // Load method
    public static ApiManifestDocument Load(JsonElement value)
    {
        var apiManifest = new ApiManifestDocument();
        ParsingHelpers.ParseMap(value, apiManifest, handlers);
        return apiManifest;
    }
    // Create fixed field map for ApiManifest
    private static FixedFieldMap<ApiManifestDocument> handlers = new()
    {
        { "publisher", (o,v) => {o.Publisher = Publisher.Load(v);  } },
        { "apiDependencies", (o,v) => {o.ApiDependencies = ParsingHelpers.GetList(v, ApiDependency.Load);  } },
    };
}

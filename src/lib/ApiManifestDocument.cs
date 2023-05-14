using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class ApiManifestDocument
{
    public Publisher? Publisher { get; set; }
    public List<ApiDependency> ApiDependencies { get; set; } = new List<ApiDependency>();

    private const string PublisherProperty = "publisher";
    private const string ApiDependenciesProperty = "apiDependencies";

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (Publisher != null) writer.WritePropertyName(PublisherProperty);
        Publisher?.Write(writer);

        if (ApiDependencies.Count > 0) writer.WritePropertyName(ApiDependenciesProperty);
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
        { PublisherProperty, (o,v) => {o.Publisher = Publisher.Load(v);  } },
        { ApiDependenciesProperty, (o,v) => {o.ApiDependencies = ParsingHelpers.GetList(v, ApiDependency.Load);  } },
    };
}

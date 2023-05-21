using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class ApiManifestDocument
{
    public Publisher? Publisher { get; set; }
    public ApiDependencies ApiDependencies { get; set; } = new ApiDependencies();

    private const string PublisherProperty = "publisher";
    private const string ApiDependenciesProperty = "apiDependencies";

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (Publisher != null) writer.WritePropertyName(PublisherProperty);
        Publisher?.Write(writer);

        if (ApiDependencies.Count > 0) writer.WritePropertyName(ApiDependenciesProperty);
        writer.WriteStartObject();
        foreach (var apiDependency in ApiDependencies)
        {
            writer.WritePropertyName(apiDependency.Key);
            apiDependency.Value.Write(writer);
        }
        writer.WriteEndObject();

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
        { ApiDependenciesProperty, (o,v) => {o.ApiDependencies = new ApiDependencies(ParsingHelpers.GetMap(v, ApiDependency.Load));  } },
    };
}

public class ApiDependencies : Dictionary<string, ApiDependency>
{
    public ApiDependencies(IDictionary<string, ApiDependency> dictionary) : base(dictionary, StringComparer.OrdinalIgnoreCase) { }
    public ApiDependencies() : base(StringComparer.OrdinalIgnoreCase) { }
}
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;
public class ApiDependency
{
    public string? ApiDescripionUrl { get; set; }
    public Auth? Auth { get; set; }
    public List<Request> Requests { get; set; } = new List<Request>();

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(ApiDescripionUrl)) writer.WriteString("apiDescripionUrl", ApiDescripionUrl);
        if (Auth != null) writer.WritePropertyName("auth");
        Auth?.Write(writer);

        if (Requests.Count > 0) writer.WritePropertyName("requests");
        writer.WriteStartArray();
        foreach (var request in Requests)
        {
            request.Write(writer);
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
    // Fixed fieldmap for ApiDependency
    private static FixedFieldMap<ApiDependency> handlers = new()
    {
        { "apiDescripionUrl", (o,v) => {o.ApiDescripionUrl = v.GetString();  } },
        { "auth", (o,v) => {o.Auth = Auth.Load(v);  } },
        { "requests", (o,v) => {o.Requests = ParsingHelpers.GetList(v, Request.Load);  } },
    };

    // Load Method
    internal static ApiDependency Load(JsonElement value)
    {
        var apiDependency = new ApiDependency();
        ParsingHelpers.ParseMap(value, apiDependency, handlers);
        return apiDependency;
    }
}

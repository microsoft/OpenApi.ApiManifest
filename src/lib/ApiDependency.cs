using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;
public class ApiDependency
{
    public string? ApiDescripionUrl { get; set; }
    public string? BaseUrl { get; set; }
    public Auth? Auth { get; set; }
    public List<Request> Requests { get; set; } = new List<Request>();

    private const string ApiDescriptionUrlProperty = "apiDescripionUrl";
    private const string AuthProperty = "auth";
    private const string RequestsProperty = "requests";
    private const string BaseUrlProperty = "baseUrl";

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(ApiDescripionUrl)) writer.WriteString(ApiDescriptionUrlProperty, ApiDescripionUrl);
        if (Auth != null) {
            writer.WritePropertyName(AuthProperty);
            Auth?.Write(writer);
        }
        if (!String.IsNullOrWhiteSpace(BaseUrl)) writer.WriteString(BaseUrlProperty, BaseUrl);

        if (Requests.Count > 0) {
            writer.WritePropertyName(RequestsProperty);
            writer.WriteStartArray();
            foreach (var request in Requests)
            {
                request.Write(writer);
            }
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
    // Fixed fieldmap for ApiDependency
    private static FixedFieldMap<ApiDependency> handlers = new()
    {
        { ApiDescriptionUrlProperty, (o,v) => {o.ApiDescripionUrl = v.GetString();  } },
        { AuthProperty, (o,v) => {o.Auth = Auth.Load(v);  } },
        { RequestsProperty, (o,v) => {o.Requests = ParsingHelpers.GetList(v, Request.Load);  } },
        { BaseUrlProperty, (o,v) => {o.BaseUrl = v.GetString();  } },
    };

    // Load Method
    internal static ApiDependency Load(JsonElement value)
    {
        var apiDependency = new ApiDependency();
        ParsingHelpers.ParseMap(value, apiDependency, handlers);
        return apiDependency;
    }
}

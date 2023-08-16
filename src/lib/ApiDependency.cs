using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;
public class ApiDependency
{
    public string? ApiDescriptionUrl { get; set; }
    public string? ApiDescriptionVersion { get; set; }
    public Auth? Auth { get; set; }
    public List<Request> Requests { get; set; } = new List<Request>();
    public Extensions? Extensions { get; set; }

    private const string ApiDescriptionUrlProperty = "apiDescriptionUrl";
    private const string ApiDescriptionVersionProperty = "apiDescriptionVersion";
    private const string AuthProperty = "auth";
    private const string RequestsProperty = "requests";
    private const string ExtensionsProperty = "extensions";

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!string.IsNullOrWhiteSpace(ApiDescriptionUrl)) writer.WriteString(ApiDescriptionUrlProperty, ApiDescriptionUrl);
        if (!string.IsNullOrWhiteSpace(ApiDescriptionVersion)) writer.WriteString(ApiDescriptionVersionProperty, ApiDescriptionVersion);

        if (Auth != null)
        {
            writer.WritePropertyName(AuthProperty);
            Auth?.Write(writer);
        }

        if (Requests.Count > 0)
        {
            writer.WritePropertyName(RequestsProperty);
            writer.WriteStartArray();
            foreach (var request in Requests)
            {
                request.Write(writer);
            }
            writer.WriteEndArray();
        }

        if (Extensions != null)
        {
            writer.WritePropertyName(ExtensionsProperty);
            Extensions?.Write(writer);
        }

        writer.WriteEndObject();
    }
    // Fixed fieldmap for ApiDependency
    private static readonly FixedFieldMap<ApiDependency> handlers = new()
    {
        { ApiDescriptionUrlProperty, (o,v) => {o.ApiDescriptionUrl = v.GetString();  } },
        { ApiDescriptionVersionProperty, (o,v) => {o.ApiDescriptionVersion = v.GetString();  } },
        { AuthProperty, (o,v) => {o.Auth = Auth.Load(v);  } },
        { RequestsProperty, (o,v) => {o.Requests = ParsingHelpers.GetList(v, Request.Load);  } },
        { ExtensionsProperty, (o,v) => {o.Extensions = Extensions.Load(v);  } }
    };

    // Load Method
    internal static ApiDependency Load(JsonElement value)
    {
        var apiDependency = new ApiDependency();
        ParsingHelpers.ParseMap(value, apiDependency, handlers);
        return apiDependency;
    }
}

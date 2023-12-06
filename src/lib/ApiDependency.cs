using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;
public class ApiDependency
{
    public string? ApiDescriptionUrl { get; set; }
    public string? ApiDescriptionVersion { get; set; }
    private string? _apiDeploymentBaseUrl;
    public string? ApiDeploymentBaseUrl
    {
        get { return _apiDeploymentBaseUrl; }
        set
        {
            ValidationHelpers.ValidateBaseUrl(nameof(ApiDeploymentBaseUrl), value);
            _apiDeploymentBaseUrl = value;
        }
    }
    public AuthorizationRequirements? AuthorizationRequirements { get; set; }
    public IList<RequestInfo> Requests { get; set; } = new List<RequestInfo>();
    public Extensions? Extensions { get; set; }

    private const string ApiDescriptionUrlProperty = "apiDescriptionUrl";
    private const string ApiDescriptionVersionProperty = "apiDescriptionVersion";
    private const string ApiDeploymentBaseUrlProperty = "apiDeploymentBaseUrl";
    private const string AuthorizationRequirementsProperty = "authorizationRequirements";
    private const string RequestsProperty = "requests";
    private const string ExtensionsProperty = "extensions";

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();

        if (!string.IsNullOrWhiteSpace(ApiDescriptionUrl)) writer.WriteString(ApiDescriptionUrlProperty, ApiDescriptionUrl);
        if (!string.IsNullOrWhiteSpace(ApiDescriptionVersion)) writer.WriteString(ApiDescriptionVersionProperty, ApiDescriptionVersion);
        if (!string.IsNullOrWhiteSpace(ApiDeploymentBaseUrl)) writer.WriteString(ApiDeploymentBaseUrlProperty, ApiDeploymentBaseUrl);

        if (AuthorizationRequirements != null)
        {
            writer.WritePropertyName(AuthorizationRequirementsProperty);
            AuthorizationRequirements.Write(writer);
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
            Extensions.Write(writer);
        }

        writer.WriteEndObject();
    }


    // Load Method
    internal static ApiDependency Load(JsonElement value)
    {
        var apiDependency = new ApiDependency();
        ParsingHelpers.ParseMap(value, apiDependency, handlers);
        return apiDependency;
    }

    // Fixed fieldmap for ApiDependency
    private static readonly FixedFieldMap<ApiDependency> handlers = new()
    {
        { ApiDescriptionUrlProperty, (o,v) => {o.ApiDescriptionUrl = v.GetString();  } },
        { ApiDescriptionVersionProperty, (o,v) => {o.ApiDescriptionVersion = v.GetString();  } },
        { ApiDeploymentBaseUrlProperty, (o,v) => {o.ApiDeploymentBaseUrl = v.GetString();  } },
        { AuthorizationRequirementsProperty, (o,v) => {o.AuthorizationRequirements = AuthorizationRequirements.Load(v);  } },
        { RequestsProperty, (o,v) => {o.Requests = ParsingHelpers.GetList(v, RequestInfo.Load);  } },
        { ExtensionsProperty, (o,v) => {o.Extensions = Extensions.Load(v);  } }
    };
}

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;
public class RequestInfo
{
    public string? Method { get; set; }
    public string? UriTemplate { get; set; }
    public string? DataClassification { get; set; }
    public bool Exclude { get; set; }

    private const string MethodProperty = "method";
    private const string UriTemplateProperty = "uriTemplate";
    private const string DataClassificationProperty = "dataClassification";
    private const string ExcludeProperty = "exclude";

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(Method)) writer.WriteString(MethodProperty, Method);
        if (!String.IsNullOrWhiteSpace(UriTemplate)) writer.WriteString(UriTemplateProperty, UriTemplate);
        if (!String.IsNullOrWhiteSpace(DataClassification)) writer.WriteString(DataClassificationProperty, DataClassification);
        if (Exclude) writer.WriteBoolean(ExcludeProperty, Exclude);

        writer.WriteEndObject();
    }
    // Fixed fieldmap for Request
    private static readonly FixedFieldMap<RequestInfo> handlers = new()
    {
        { MethodProperty, (o,v) => {o.Method = v.GetString();  } },
        { UriTemplateProperty, (o,v) => {o.UriTemplate = v.GetString();  } },
        { DataClassificationProperty, (o,v) => {o.DataClassification = v.GetString();  } },
        { ExcludeProperty, (o,v) => {o.Exclude = v.GetBoolean();  } },
    };

    // Load Method
    internal static RequestInfo Load(JsonElement value)
    {
        var request = new RequestInfo();
        ParsingHelpers.ParseMap(value, request, handlers);
        return request;
    }
}
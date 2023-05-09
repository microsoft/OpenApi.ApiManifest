using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;
public class Request
{
    public string? Method { get; set; }
    public string? UriTemplate { get; set; }
    public string? DataClassification { get; set; }

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(Method)) writer.WriteString("method", Method);
        if (!String.IsNullOrWhiteSpace(UriTemplate)) writer.WriteString("uriTemplate", UriTemplate);
        if (!String.IsNullOrWhiteSpace(DataClassification)) writer.WriteString("dataClassification", DataClassification);

        writer.WriteEndObject();
    }
    // Fixed fieldmap for Request
    private static FixedFieldMap<Request> handlers = new()
    {
        { "method", (o,v) => {o.Method = v.GetString();  } },
        { "uriTemplate", (o,v) => {o.UriTemplate = v.GetString();  } },
        { "dataClassification", (o,v) => {o.DataClassification = v.GetString();  } },
    };

    // Load Method
    internal static Request Load(JsonElement value)
    {
        var request = new Request();
        ParsingHelpers.ParseMap(value, request, handlers);
        return request;
    }
}
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class Auth
{
    public string? ClientIdentifier { get; set; }
    public List<string>? AccessReference { get; set; }
    public List<AccessRequest>? Access { get; set; }

    private const string ClientIdentifierProperty = "clientIdentifier";
    private const string AccessProperty = "access";

    // Fixed fieldmap for Auth
    private static FixedFieldMap<Auth> handlers = new()
    {
        { ClientIdentifierProperty, (o,v) => { o.ClientIdentifier = v.GetString();  } },
        { AccessProperty, (o,v) => { LoadAccessProperty(o, v); }}
    };

    private static void LoadAccessProperty(Auth o, JsonElement v)
    {
        var content = v.EnumerateArray().FirstOrDefault();
        if (content.ValueKind == JsonValueKind.String)
        {
            o.AccessReference = ParsingHelpers.GetListOfString(v);
        }
        else if (content.ValueKind == JsonValueKind.Object)
        {
            o.Access = ParsingHelpers.GetList<AccessRequest>(v, AccessRequest.Load);
        }
    }

    // Write Method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(ClientIdentifier)) writer.WriteString(ClientIdentifierProperty, ClientIdentifier);

        if (Access != null)
        {
            writer.WritePropertyName(AccessProperty);
            writer.WriteStartArray();
            if (AccessReference != null)
            {
                foreach (var accessReference in AccessReference)
                {
                    writer.WriteStringValue(accessReference);
                }
            } else if (Access != null) {
                foreach (var accessRequest in Access)
                {
                    accessRequest.Write(writer);
                }
            }
            writer.WriteEndArray();
        }
        writer.WriteEndObject();
    }
    // Load Method
    internal static Auth Load(JsonElement value)
    {
        var auth = new Auth();
        ParsingHelpers.ParseMap(value, auth, handlers);
        return auth;
    }
}

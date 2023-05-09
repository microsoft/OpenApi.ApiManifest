using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class Auth
{
    public string? ClientId { get; set; }
    public Dictionary<string, List<string>>? Permissions { get; set; }

    // Fixed fieldmap for Auth
    private static FixedFieldMap<Auth> handlers = new()
    {
        { "clientId", (o,v) => {o.ClientId = v.GetString();  } },
        { "permissions", (o,v) => {o.Permissions = ParsingHelpers.GetMap(v, ParsingHelpers.GetListOfString);  } },
    };

    // Write Method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(ClientId)) writer.WriteString("clientId", ClientId);

        if (Permissions != null)
        {
            writer.WritePropertyName("permissions");
            writer.WriteStartObject();
            foreach (var permission in Permissions)
            {
                writer.WritePropertyName(permission.Key);
                writer.WriteStartArray();
                foreach (var value in permission.Value)
                {
                    writer.WriteStringValue(value);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
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

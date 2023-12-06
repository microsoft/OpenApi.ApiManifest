using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class AuthorizationRequirements
{
    public string? ClientIdentifier { get; set; }
    public IList<string>? AccessReference { get; set; }
    public IList<AccessRequest>? Access { get; set; }

    private const string ClientIdentifierProperty = "clientIdentifier";
    private const string AccessProperty = "access";

    // Fixed fieldmap for AuthorizationRequirements
    private static readonly FixedFieldMap<AuthorizationRequirements> handlers = new()
    {
        { ClientIdentifierProperty, (o,v) => { o.ClientIdentifier = v.GetString();  } },
        { AccessProperty, (o,v) => { LoadAccessProperty(o, v); }}
    };

    private static void LoadAccessProperty(AuthorizationRequirements o, JsonElement v)
    {
        JsonElement content = v.EnumerateArray().FirstOrDefault();
        if (content.ValueKind == JsonValueKind.String)
        {
            o.AccessReference = ParsingHelpers.GetListOfString(v);
        }
        else if (content.ValueKind == JsonValueKind.Object)
        {
            o.Access = ParsingHelpers.GetList(v, AccessRequest.Load);
        }
    }

    // Write Method
    public void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();

        if (!string.IsNullOrWhiteSpace(ClientIdentifier)) writer.WriteString(ClientIdentifierProperty, ClientIdentifier);

        if (AccessReference is not null)
        {
            writer.WritePropertyName(AccessProperty);
            writer.WriteStartArray();
            foreach (string accessReference in AccessReference)
            {
                writer.WriteStringValue(accessReference);
            }
            writer.WriteEndArray();
        }
        else if (Access is not null)
        {
            writer.WritePropertyName(AccessProperty);
            writer.WriteStartArray();
            foreach (AccessRequest accessRequest in Access)
            {
                accessRequest.Write(writer);
            }
            writer.WriteEndArray();
        }
        writer.WriteEndObject();
    }
    // Load Method
    internal static AuthorizationRequirements Load(JsonElement value)
    {
        var auth = new AuthorizationRequirements();
        ParsingHelpers.ParseMap(value, auth, handlers);
        return auth;
    }
}


using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class OpenAIPluginManifest
{
    public string? SchemaVersion { get; set; }
    public string? NameForHuman { get; set; }
    public string? NameForModel { get; set; }
    public string? DescriptionForHuman { get; set; }
    public string? DescriptionForModel { get; set; }
    public BaseManifestAuth? Auth { get; set; }
    public Api? Api { get; set; }
    public string? LogoUrl { get; set; }
    public string? ContactEmail { get; set; }
    public string? LegalInfoUrl { get; set; }

    public OpenAIPluginManifest()
    {
        SchemaVersion = "v1";
    }

    public static OpenAIPluginManifest Load(JsonElement value)
    {
        var manifest = new OpenAIPluginManifest();
        ParsingHelpers.ParseMap(value, manifest, handlers);
        return manifest;
    }

    // Create handlers FixedFieldMap for OpenAIPluginManifest
    private static readonly FixedFieldMap<OpenAIPluginManifest> handlers = new()
    {
        { "schema_version", (o,v) => {o.SchemaVersion = v.GetString();  } },
        { "name_for_human", (o,v) => {o.NameForHuman = v.GetString();  } },
        { "name_for_model", (o,v) => {o.NameForModel = v.GetString();  } },
        { "description_for_human", (o,v) => {o.DescriptionForHuman = v.GetString();  } },
        { "description_for_model", (o,v) => {o.DescriptionForModel = v.GetString();  } },
        { "auth", (o,v) => {o.Auth = BaseManifestAuth.Load(v);  } },
        { "api", (o,v) => {o.Api = Api.Load(v);  } },
        { "logo_url", (o,v) => {o.LogoUrl = v.GetString();  } },
        { "contact_email", (o,v) => {o.ContactEmail = v.GetString();  } },
        { "legal_info_url", (o,v) => {o.LegalInfoUrl = v.GetString();  } },
    };

    //Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("schema_version", SchemaVersion);
        writer.WriteString("name_for_human", NameForHuman);
        writer.WriteString("name_for_model", NameForModel);
        writer.WriteString("description_for_human", DescriptionForHuman);
        writer.WriteString("description_for_model", DescriptionForModel);
        if (Auth != null)
        {
            writer.WritePropertyName("auth");
            Auth.Write(writer);
        }
        if (Api != null)
        {
            writer.WritePropertyName("api");
            Api?.Write(writer);
        }
        if (LogoUrl != null) writer.WriteString("logo_url", LogoUrl);
        if (ContactEmail != null) writer.WriteString("contact_email", ContactEmail);
        if (LegalInfoUrl != null) writer.WriteString("legal_info_url", LegalInfoUrl);
        writer.WriteEndObject();
    }
}



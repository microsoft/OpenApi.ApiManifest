
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public abstract class BaseManifestAuth
{
    public string? Type { get; set; }
    public string? Instructions { get; set; }

    public static BaseManifestAuth? Load(JsonElement value)
    {
        BaseManifestAuth? auth = null;

        switch(value.GetProperty("type").GetString()) {
            case "none":
                auth = new ManifestNoAuth();
                ParsingHelpers.ParseMap<ManifestNoAuth>(value, (ManifestNoAuth)auth, ManifestNoAuth.handlers);
                break;
            case "user_http":
                auth = new ManifestUserHttpAuth();
                ParsingHelpers.ParseMap<ManifestUserHttpAuth>(value, (ManifestUserHttpAuth)auth, ManifestUserHttpAuth.handlers);
                break;
            case "service_http":
                auth = new ManifestServiceHttpAuth();
                ParsingHelpers.ParseMap<ManifestServiceHttpAuth>(value, (ManifestServiceHttpAuth)auth, ManifestServiceHttpAuth.handlers);
                break;
            case "oauth":
                auth = new ManifestOAuthAuth();
                ParsingHelpers.ParseMap<ManifestOAuthAuth>(value, (ManifestOAuthAuth)auth, ManifestOAuthAuth.handlers);
                break;
        }
        
        return auth;
    }

    // Create handlers FixedFieldMap for ManifestAuth

    public virtual void Write(Utf8JsonWriter writer) {}

}

public class ManifestNoAuth : BaseManifestAuth
{
    public ManifestNoAuth()
    {
        Type = "none";
    }

    internal static FixedFieldMap<ManifestNoAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
    };

    public override void Write(Utf8JsonWriter writer) {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        if(Instructions != null) writer.WriteString("instructions", Instructions);
        writer.WriteEndObject();
    }
}

public class ManifestOAuthAuth : BaseManifestAuth
{
    public string? ClientUrl { get; set; }
    public string? Scope { get; set; }
    public string? AuthorizationUrl { get; set; }
    public string? AuthorizationContentType { get; set; }
    public Dictionary<string, string>? VerificationTokens { get; set; }

    public ManifestOAuthAuth()
    {
        Type = "oauth";
    }
    internal static FixedFieldMap<ManifestOAuthAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
        { "client_url", (o,v) => {o.ClientUrl = v.GetString();  } },
        { "scope", (o,v) => {o.Scope = v.GetString();  } },
        { "authorization_url", (o,v) => {o.AuthorizationUrl = v.GetString();  } },
        { "authorization_content_type", (o,v) => {o.AuthorizationContentType = v.GetString();  } },
        { "verification_tokens", (o,v) => { o.VerificationTokens = ParsingHelpers.GetMap<string>(v,(e) => e.GetString() );  } },
    };

    public override void Write(Utf8JsonWriter writer) {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        
        if(Instructions != null) writer.WriteString("instructions", Instructions);
        if(ClientUrl != null) writer.WriteString("client_url", ClientUrl);
        if(Scope != null) writer.WriteString("scope", Scope);
        if(AuthorizationUrl != null) writer.WriteString("authorization_url", AuthorizationUrl);
        if(AuthorizationContentType != null) writer.WriteString("authorization_content_type", AuthorizationContentType);
        writer.WriteEndObject();
    }
}

public class ManifestUserHttpAuth : BaseManifestAuth
{
    public ManifestUserHttpAuth()
    {
        Type = "user_http";
    }
    internal static FixedFieldMap<ManifestUserHttpAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
    };
    public override void Write(Utf8JsonWriter writer) {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        writer.WriteString("instructions", Instructions);
        writer.WriteEndObject();
    }
}

public class ManifestServiceHttpAuth : BaseManifestAuth
{
    public ManifestServiceHttpAuth()
    {
        Type = "service_http";
    }
    internal static FixedFieldMap<ManifestServiceHttpAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
    };
    public override void Write(Utf8JsonWriter writer) {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        writer.WriteString("instructions", Instructions);
        writer.WriteEndObject();
    }
}





// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

public class ManifestOAuthAuth : BaseManifestAuth
{
    private const string ClientUrlPropertyName = "client_url";
    private const string ScopePropertyName = "scope";
    private const string AuthorizationUrlPropertyName = "authorization_url";
    private const string AuthorizationContentTypePropertyName = "authorization_content_type";
    private const string VerificationTokensPropertyName = "verification_tokens";

    public string? ClientUrl { get; set; }
    public string? Scope { get; set; }
    public string? AuthorizationUrl { get; set; }
    public string? AuthorizationContentType { get; set; }
    public VerificationTokens VerificationTokens { get; set; } = new VerificationTokens();

    public ManifestOAuthAuth()
    {
        Type = "oauth";
    }

    private static readonly FixedFieldMap<ManifestOAuthAuth> handlers = new()
    {
        { ClientUrlPropertyName, (o,v) => {o.ClientUrl = v.GetString();  } },
        { ScopePropertyName, (o,v) => {o.Scope = v.GetString();  } },
        { AuthorizationUrlPropertyName, (o,v) => {o.AuthorizationUrl = v.GetString();  } },
        { AuthorizationContentTypePropertyName, (o,v) => {o.AuthorizationContentType = v.GetString();  } },
        { VerificationTokensPropertyName, (o,v) => { o.VerificationTokens = VerificationTokens.Load(v);  } },
    };

    public static ManifestOAuthAuth Load(JsonElement value)
    {
        var auth = new ManifestOAuthAuth();
        auth.LoadProperties(value);
        ParsingHelpers.ParseMap(value, auth, handlers);
        return auth;
    }

    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        WriteProperties(writer);
        if (!string.IsNullOrWhiteSpace(ClientUrl)) writer.WriteString(ClientUrlPropertyName, ClientUrl);
        if (!string.IsNullOrWhiteSpace(Scope)) writer.WriteString(ScopePropertyName, Scope);
        if (!string.IsNullOrWhiteSpace(AuthorizationUrl)) writer.WriteString(AuthorizationUrlPropertyName, AuthorizationUrl);
        if (!string.IsNullOrWhiteSpace(AuthorizationContentType)) writer.WriteString(AuthorizationContentTypePropertyName, AuthorizationContentType);
        if (VerificationTokens.Any())
        {
            writer.WritePropertyName(VerificationTokensPropertyName);
            VerificationTokens.Write(writer);
        }
        writer.WriteEndObject();
    }
}

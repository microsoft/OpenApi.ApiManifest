// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI.Auth;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class ManifestOAuthAuth : BaseManifestAuth
{
    public string? ClientUrl { get; set; }
    public string? Scope { get; set; }
    public string? AuthorizationUrl { get; set; }
    public string? AuthorizationContentType { get; set; }
    public VerificationTokens VerificationTokens { get; set; } = new VerificationTokens();

    public ManifestOAuthAuth()
    {
        Type = "oauth";
    }
    internal static readonly FixedFieldMap<ManifestOAuthAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
        { "client_url", (o,v) => {o.ClientUrl = v.GetString();  } },
        { "scope", (o,v) => {o.Scope = v.GetString();  } },
        { "authorization_url", (o,v) => {o.AuthorizationUrl = v.GetString();  } },
        { "authorization_content_type", (o,v) => {o.AuthorizationContentType = v.GetString();  } },
        { "verification_tokens", (o,v) => { o.VerificationTokens = VerificationTokens.Load(v);  } },
    };

    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", Type);

        if (Instructions != null) writer.WriteString("instructions", Instructions);
        if (ClientUrl != null) writer.WriteString("client_url", ClientUrl);
        if (Scope != null) writer.WriteString("scope", Scope);
        if (AuthorizationUrl != null) writer.WriteString("authorization_url", AuthorizationUrl);
        if (AuthorizationContentType != null) writer.WriteString("authorization_content_type", AuthorizationContentType);
        if (VerificationTokens.Any())
        {
            writer.WritePropertyName("verification_tokens");
            VerificationTokens.Write(writer);
        }
        writer.WriteEndObject();
    }
}

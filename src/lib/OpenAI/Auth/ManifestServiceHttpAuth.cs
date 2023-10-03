// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI.Auth;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class ManifestServiceHttpAuth : BaseManifestAuth
{
    public string? AuthorizationType { get; set; }
    public VerificationTokens VerificationTokens { get; set; }
    public ManifestServiceHttpAuth(VerificationTokens verificationTokens)
    {
        if (verificationTokens == null || !verificationTokens.Any())
        {
            // Reference: https://platform.openai.com/docs/plugins/authentication/service-level
            throw new ArgumentException($"{nameof(verificationTokens)} must be have at least one verification token.");
        }
        Type = "service_http";
        AuthorizationType = "bearer";
        VerificationTokens = verificationTokens;
    }

    internal static readonly FixedFieldMap<ManifestServiceHttpAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "authorization_type", (o,v) => {o.AuthorizationType = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
        { "verification_tokens", (o, v) => { o.VerificationTokens = VerificationTokens.Load(v); } }
    };

    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        writer.WriteString("authorization_type", AuthorizationType);
        if (Instructions != null) writer.WriteString("instructions", Instructions);
        writer.WritePropertyName("verification_tokens");
        VerificationTokens.Write(writer);
        writer.WriteEndObject();
    }
}

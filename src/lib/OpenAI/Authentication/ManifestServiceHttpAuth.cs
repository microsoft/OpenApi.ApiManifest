// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

public class ManifestServiceHttpAuth : BaseManifestAuth
{
    private const string AuthorizationTypeProperty = "authorization_type";
    private const string VerificationTokensProperty = "verification_tokens";
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

    private static readonly FixedFieldMap<ManifestServiceHttpAuth> handlers = new()
    {
        { AuthorizationTypeProperty, (o,v) => {o.AuthorizationType = v.GetString();  } }
    };

    public static ManifestServiceHttpAuth Load(JsonElement value)
    {
        var auth = new ManifestServiceHttpAuth(VerificationTokens.Load(value.GetProperty(VerificationTokensProperty)));
        auth.LoadProperties(value);
        ParsingHelpers.ParseMap(value, auth, handlers);
        return auth;
    }

    public override void Write(Utf8JsonWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStartObject();
        WriteProperties(writer);
        writer.WriteString(AuthorizationTypeProperty, AuthorizationType);
        writer.WritePropertyName(VerificationTokensProperty);
        VerificationTokens.Write(writer);
        writer.WriteEndObject();
    }
}

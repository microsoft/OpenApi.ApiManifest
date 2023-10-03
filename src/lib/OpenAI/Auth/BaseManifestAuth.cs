// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI.Auth;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public abstract class BaseManifestAuth
{
    public string? Type { get; set; }
    public string? Instructions { get; set; }

    public static BaseManifestAuth? Load(JsonElement value)
    {
        BaseManifestAuth? auth = null;

        switch (value.GetProperty("type").GetString())
        {
            case "none":
                auth = new ManifestNoAuth();
                ParsingHelpers.ParseMap(value, (ManifestNoAuth)auth, ManifestNoAuth.handlers);
                break;
            case "user_http":
                var authorizationType = value.GetProperty("authorization_type").GetString();
                auth = new ManifestUserHttpAuth(authorizationType);
                ParsingHelpers.ParseMap(value, (ManifestUserHttpAuth)auth, ManifestUserHttpAuth.handlers);
                break;
            case "service_http":
                var verificationTokens = value.GetProperty("verification_tokens");
                auth = new ManifestServiceHttpAuth(VerificationTokens.Load(verificationTokens));
                ParsingHelpers.ParseMap(value, (ManifestServiceHttpAuth)auth, ManifestServiceHttpAuth.handlers);
                break;
            case "oauth":
                auth = new ManifestOAuthAuth();
                ParsingHelpers.ParseMap(value, (ManifestOAuthAuth)auth, ManifestOAuthAuth.handlers);
                break;
        }

        return auth;
    }

    // Create handlers FixedFieldMap for ManifestAuth

    public virtual void Write(Utf8JsonWriter writer) { }

}

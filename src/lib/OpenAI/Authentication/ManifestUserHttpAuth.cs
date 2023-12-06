// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

public class ManifestUserHttpAuth : BaseManifestAuth
{
    private const string AuthorizationTypeProperty = "authorization_type";
    public string? AuthorizationType { get; set; }
    public ManifestUserHttpAuth(string? authorizationType)
    {
        if (string.IsNullOrWhiteSpace(authorizationType) ||
            (!string.Equals(authorizationType, "basic", StringComparison.OrdinalIgnoreCase) && !string.Equals(authorizationType, "bearer", StringComparison.OrdinalIgnoreCase)))
        {
            // Reference: https://platform.openai.com/docs/plugins/authentication/user-level
            throw new ArgumentException($"{nameof(authorizationType)} must be either 'basic' or 'bearer'.");
        }
        Type = "user_http";
        AuthorizationType = authorizationType;
    }

    public static ManifestUserHttpAuth Load(JsonElement value)
    {
        var auth = new ManifestUserHttpAuth(value.GetProperty(AuthorizationTypeProperty).GetString());
        auth.LoadProperties(value);
        return auth;
    }

    public override void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();
        WriteProperties(writer);
        writer.WriteString(AuthorizationTypeProperty, AuthorizationType);
        writer.WriteEndObject();
    }
}

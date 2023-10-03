// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class ManifestUserHttpAuth : BaseManifestAuth
{
    public string? AuthorizationType { get; set; }
    public ManifestUserHttpAuth(string? authorizationType)
    {
        if (string.IsNullOrWhiteSpace(authorizationType) || (authorizationType != "basic" && authorizationType != "bearer"))
        {
            // Reference: https://platform.openai.com/docs/plugins/authentication/user-level
            throw new ArgumentException($"{nameof(authorizationType)} must be either 'basic' or 'bearer'.");
        }
        Type = "user_http";
        AuthorizationType = authorizationType;
    }

    internal static readonly FixedFieldMap<ManifestUserHttpAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "authorization_type", (o,v) => {o.AuthorizationType = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
    };
    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        writer.WriteString("authorization_type", AuthorizationType);
        if (Instructions != null) writer.WriteString("instructions", Instructions);
        writer.WriteEndObject();
    }
}

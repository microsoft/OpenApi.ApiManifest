// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

public class ManifestNoAuth : BaseManifestAuth
{
    public ManifestNoAuth()
    {
        Type = "none";
    }

    public static ManifestNoAuth Load(JsonElement value)
    {
        var auth = new ManifestNoAuth();
        auth.LoadProperties(value);
        return auth;
    }

    public override void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();
        WriteProperties(writer);
        writer.WriteEndObject();
    }
}

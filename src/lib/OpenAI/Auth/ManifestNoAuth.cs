// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class ManifestNoAuth : BaseManifestAuth
{
    public ManifestNoAuth()
    {
        Type = "none";
    }

    internal static readonly FixedFieldMap<ManifestNoAuth> handlers = new()
    {
        { "type", (o,v) => {o.Type = v.GetString();  } },
        { "instructions", (o,v) => {o.Instructions = v.GetString();  } },
    };

    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", Type);
        if (Instructions != null) writer.WriteString("instructions", Instructions);
        writer.WriteEndObject();
    }
}

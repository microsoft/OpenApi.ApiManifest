
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class Api
{
    private const string TypeProperty = "type";
    private const string UrlProperty = "url";
    private const string IsUserAuthenticatedProperty = "is_user_authenticated";
    public string? Type { get; set; }
    public string? Url { get; set; }
    public bool? IsUserAuthenticated { get; set; }

    public static Api Load(JsonElement value)
    {
        var api = new Api();
        ParsingHelpers.ParseMap(value, api, handlers);
        return api;
    }

    // Create handlers FixedFieldMap for Api
    private static readonly FixedFieldMap<Api> handlers = new()
    {
        { TypeProperty, (o,v) => {o.Type = v.GetString();  } },
        { UrlProperty, (o,v) => {o.Url = v.GetString();  } },
        { IsUserAuthenticatedProperty, (o,v) => {o.IsUserAuthenticated = v.GetBoolean(); }},
    };

    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString(TypeProperty, Type);
        writer.WriteString(UrlProperty, Url);
        writer.WriteBoolean(IsUserAuthenticatedProperty, IsUserAuthenticated ?? false);
        writer.WriteEndObject();
    }
}



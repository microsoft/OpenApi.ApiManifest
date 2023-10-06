
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class OpenAIPluginManifest
{
    private const string SchemaVersionProperty = "schema_version";
    private const string NameForHumanProperty = "name_for_human";
    private const string NameForModelProperty = "name_for_model";
    private const string DescriptionForHumanProperty = "description_for_human";
    private const string DescriptionForModelProperty = "description_for_model";
    private const string AuthProperty = "auth";
    private const string ApiProperty = "api";
    private const string LogoUrlProperty = "logo_url";
    private const string ContactEmailProperty = "contact_email";
    private const string LegalInfoUrlProperty = "legal_info_url";

    public string? SchemaVersion { get; set; }
    public string? NameForHuman { get; set; }
    public string? NameForModel { get; set; }
    public string? DescriptionForHuman { get; set; }
    public string? DescriptionForModel { get; set; }
    public BaseManifestAuth? Auth { get; set; }
    public Api? Api { get; set; }
    public string? LogoUrl { get; set; }
    public string? ContactEmail { get; set; }
    public string? LegalInfoUrl { get; set; }

    public OpenAIPluginManifest()
    {
        SchemaVersion = "v1";
    }

    public static OpenAIPluginManifest Load(JsonElement value)
    {
        var manifest = new OpenAIPluginManifest();
        ParsingHelpers.ParseMap(value, manifest, handlers);
        return manifest;
    }

    // Create handlers FixedFieldMap for OpenAIPluginManifest
    private static readonly FixedFieldMap<OpenAIPluginManifest> handlers = new()
    {
        { SchemaVersionProperty, (o,v) => {o.SchemaVersion = v.GetString();  } },
        { NameForHumanProperty, (o,v) => {o.NameForHuman = v.GetString();  } },
        { NameForModelProperty, (o,v) => {o.NameForModel = v.GetString();  } },
        { DescriptionForHumanProperty, (o,v) => {o.DescriptionForHuman = v.GetString();  } },
        { DescriptionForModelProperty, (o,v) => {o.DescriptionForModel = v.GetString();  } },
        { AuthProperty, (o,v) => {o.Auth = ManifestAuthFactory.CreateManifestAuth(v);  } },
        { ApiProperty, (o,v) => {o.Api = Api.Load(v);  } },
        { LogoUrlProperty, (o,v) => {o.LogoUrl = v.GetString();  } },
        { ContactEmailProperty, (o,v) => {o.ContactEmail = v.GetString();  } },
        { LegalInfoUrlProperty, (o,v) => {o.LegalInfoUrl = v.GetString();  } },
    };

    //Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString(SchemaVersionProperty, SchemaVersion);
        writer.WriteString(NameForHumanProperty, NameForHuman);
        writer.WriteString(NameForModelProperty, NameForModel);
        writer.WriteString(DescriptionForHumanProperty, DescriptionForHuman);
        writer.WriteString(DescriptionForModelProperty, DescriptionForModel);
        if (Auth != null)
        {
            writer.WritePropertyName(AuthProperty);
            Auth.Write(writer);
        }
        if (Api != null)
        {
            writer.WritePropertyName(ApiProperty);
            Api?.Write(writer);
        }
        if (LogoUrl != null) writer.WriteString(LogoUrlProperty, LogoUrl);
        if (ContactEmail != null) writer.WriteString(ContactEmailProperty, ContactEmail);
        if (LegalInfoUrl != null) writer.WriteString(LegalInfoUrlProperty, LegalInfoUrl);
        writer.WriteEndObject();
    }
}



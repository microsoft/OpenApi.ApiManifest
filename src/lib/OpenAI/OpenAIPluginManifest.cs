
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
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

    /// <summary>
    /// REQUIRED. The version of the manifest schema.
    /// </summary>
    public string? SchemaVersion { get; private set; } = "v1";
    /// <summary>
    /// REQUIRED. The name of the plugin that will be shown to users.
    /// </summary>
    public string? NameForHuman { get; set; }
    /// <summary>
    /// REQUIRED. The name the model will use to target the plugin.
    /// </summary>
    public string? NameForModel { get; set; }
    /// <summary>
    /// REQUIRED. A description of the plugin that will be shown to users.
    /// </summary>
    public string? DescriptionForHuman { get; set; }
    /// <summary>
    /// REQUIRED. Description better tailored to the model, such as token context length considerations or keyword usage for improved plugin prompting.
    /// </summary>
    public string? DescriptionForModel { get; set; }
    /// <summary>
    /// REQUIRED. The authentication schema type for the plugin. This can be one of the following types: <see cref="ManifestNoAuth"/>, <see cref="ManifestOAuthAuth"/>, <see cref="ManifestUserHttpAuth"/>, and <see cref="ManifestServiceHttpAuth"/>.
    /// </summary>
    public BaseManifestAuth? Auth { get; set; }
    /// <summary>
    /// REQUIRED. The API specification for the plugin.
    /// </summary>
    public Api? Api { get; set; }
    /// <summary>
    /// REQUIRED. A URL to a logo for the plugin. This logo will be shown to users. Suggested size: 512 x 512. Transparent backgrounds are supported. Must be an image, no GIFs are allowed.
    /// </summary>
    public string? LogoUrl { get; set; }
    /// <summary>
    /// REQUIRED. An email address for safety/moderation, support, and deactivation.
    /// </summary>
    public string? ContactEmail { get; set; }
    /// <summary>
    /// REQUIRED. A URL to a page with legal information about the plugin.
    /// </summary>
    public string? LegalInfoUrl { get; set; }

    public OpenAIPluginManifest(string nameForModel, string nameForHuman, string logoUrl, string contactEmail, string legalInfoUrl)
    {
        NameForHuman = nameForHuman;
        NameForModel = nameForModel;
        LogoUrl = logoUrl;
        ContactEmail = contactEmail;
        LegalInfoUrl = legalInfoUrl;
    }

    internal OpenAIPluginManifest(JsonElement value)
    {
        ParsingHelpers.ParseMap(value, this, handlers);
        Validate(this);
    }

    public static OpenAIPluginManifest Load(JsonElement value)
    {
        return new OpenAIPluginManifest(value);
    }

    //Write method
    public void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        Validate(this);
        writer.WriteStartObject();
        writer.WriteString(SchemaVersionProperty, SchemaVersion);
        writer.WriteString(NameForHumanProperty, NameForHuman);
        writer.WriteString(NameForModelProperty, NameForModel);
        writer.WriteString(DescriptionForHumanProperty, DescriptionForHuman);
        writer.WriteString(DescriptionForModelProperty, DescriptionForModel);
        writer.WritePropertyName(AuthProperty);
        Auth?.Write(writer);
        writer.WritePropertyName(ApiProperty);
        Api?.Write(writer);
        writer.WriteString(LogoUrlProperty, LogoUrl);
        writer.WriteString(ContactEmailProperty, ContactEmail);
        writer.WriteString(LegalInfoUrlProperty, LegalInfoUrl);
        writer.WriteEndObject();
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

    /// <summary>
    /// Validate the provided <see cref="OpenAIPluginManifest"/> based on the Open AI Plugin manifest schema at https://platform.openai.com/docs/plugins/getting-started/plugin-manifest.
    /// </summary>
    /// <param name="openAIPluginManifest">The <see cref="OpenAIPluginManifest"/> to validate.</param>
    private void Validate(OpenAIPluginManifest openAIPluginManifest)
    {
        ValidationHelpers.ValidateNullOrWhitespace(nameof(NameForHuman), openAIPluginManifest.NameForHuman, nameof(OpenAIPluginManifest));
        ValidationHelpers.ValidateLength(nameof(NameForHuman), openAIPluginManifest.NameForHuman, 20);

        ValidationHelpers.ValidateNullOrWhitespace(nameof(NameForModel), openAIPluginManifest.NameForModel, nameof(OpenAIPluginManifest));
        ValidationHelpers.ValidateLength(nameof(NameForModel), openAIPluginManifest.NameForModel, 50);

        ValidationHelpers.ValidateNullOrWhitespace(nameof(DescriptionForHuman), openAIPluginManifest.DescriptionForHuman, nameof(OpenAIPluginManifest));
        ValidationHelpers.ValidateLength(nameof(DescriptionForHuman), openAIPluginManifest.DescriptionForHuman, 100);

        ValidationHelpers.ValidateNullOrWhitespace(nameof(DescriptionForModel), openAIPluginManifest.DescriptionForModel, nameof(OpenAIPluginManifest));
        ValidationHelpers.ValidateLength(nameof(DescriptionForModel), openAIPluginManifest.DescriptionForModel, 8000);

        ValidationHelpers.ValidateNullOrWhitespace(nameof(SchemaVersion), openAIPluginManifest.SchemaVersion, nameof(OpenAIPluginManifest));
        ValidationHelpers.ThrowIfNull(openAIPluginManifest.Auth, "Auth");
        ValidationHelpers.ThrowIfNull(openAIPluginManifest.Api, "Api");
        ValidationHelpers.ValidateNullOrWhitespace(nameof(LogoUrl), openAIPluginManifest.LogoUrl, nameof(OpenAIPluginManifest));
        ValidationHelpers.ValidateEmail(nameof(ContactEmail), openAIPluginManifest.ContactEmail, nameof(OpenAIPluginManifest));
        ValidationHelpers.ValidateNullOrWhitespace(nameof(LegalInfoUrl), openAIPluginManifest.LegalInfoUrl, nameof(OpenAIPluginManifest));
    }
}



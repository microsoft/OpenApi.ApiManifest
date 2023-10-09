// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public static class OpenApiPluginFactory
{

    public static OpenAIPluginManifest CreateOpenAIPluginManifest(string nameForModel, string nameForHuman, string logoUrl, string contactEmail, string legalInfoUrl, string schemaVersion = "v1")
    {
        return new OpenAIPluginManifest(nameForModel: nameForModel, nameForHuman: nameForHuman, logoUrl: logoUrl, contactEmail: contactEmail, legalInfoUrl: legalInfoUrl, schemaVersion: schemaVersion);
    }
}
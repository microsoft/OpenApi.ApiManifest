
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public static class OpenApiPluginFactory
{

    public static OpenAIPluginManifest CreateOpenAIPluginManifest(string nameForModel, string nameForHuman, string descriptionForHuman, string descriptionForModel, BaseManifestAuth auth, Api api, string logoUrl, string contactEmail, string legalInfoUrl, string schemaVersion = "v1")
    {
        return new OpenAIPluginManifest(nameForModel, nameForHuman, descriptionForHuman, descriptionForModel, auth, api, logoUrl, contactEmail, legalInfoUrl, schemaVersion);
    }
}
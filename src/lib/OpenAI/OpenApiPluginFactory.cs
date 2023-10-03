
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.OpenApi.ApiManifest.OpenAI;

public class OpenApiPluginFactory
{

    public static OpenAIPluginManifest CreateOpenAIPluginManifest()
    {
        var manifest = new OpenAIPluginManifest
        {
            SchemaVersion = "v1"
        };
        return manifest;
    }
}
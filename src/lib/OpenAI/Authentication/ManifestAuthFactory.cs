// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication
{
    internal static class ManifestAuthFactory
    {
        public static BaseManifestAuth CreateManifestAuth(JsonElement value)
        {
            var authType = value.GetProperty("type").GetString()?.ToLowerInvariant();
            return authType switch
            {
                "none" => ManifestNoAuth.Load(value),
                "user_http" => ManifestUserHttpAuth.Load(value),
                "service_http" => ManifestServiceHttpAuth.Load(value),
                "oauth" => ManifestOAuthAuth.Load(value),
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unknown auth type: {authType}")
            };
        }
    }
}

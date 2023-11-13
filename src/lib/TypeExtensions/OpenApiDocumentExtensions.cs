// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using Microsoft.OpenApi.Models;

namespace Microsoft.OpenApi.ApiManifest.TypeExtensions
{
    public static class OpenApiDocumentExtensions
    {
        /// <summary>
        /// Converts an <see cref="OpenApiDocument"/> to an <see cref="ApiManifestDocument"/>.
        /// </summary>
        /// <param name="document">The OpenAPI document to convert.</param>
        /// <param name="apiDescriptionUrl">The URL of the API description.</param>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="apiDependencyName">The name of the API dependency.</param>
        /// <returns>An <see cref="ApiManifestDocument"/>.</returns>
        internal static ApiManifestDocument ToApiManifest(this OpenApiDocument document, string? apiDescriptionUrl, string applicationName, string? apiDependencyName = default)
        {
            ArgumentNullException.ThrowIfNull(document);
            ValidationHelpers.ValidateNullOrWhitespace(nameof(apiDescriptionUrl), apiDescriptionUrl, nameof(ApiManifestDocument));
            ValidationHelpers.ValidateNullOrWhitespace(nameof(applicationName), applicationName, nameof(ApiManifestDocument));

            var apiName = apiDependencyName ?? document.Info.Title.Trim().Replace(' ', '-'); // Normilize OpenAPI document title to API name by trimming and replacing spaces with dashes.
            var publisherName = document.Info.Contact?.Name ?? "publisher-name";
            var publisherEmail = document.Info.Contact?.Email ?? "publisher-email@example.com";

            string? apiDeploymentBaseUrl = default;
            var server = document.Servers.FirstOrDefault();
            if (server is not null)
                apiDeploymentBaseUrl = !server.Url.EndsWith("/", StringComparison.Ordinal) ? $"{server.Url}/" : server.Url;

            var apiManifest = new ApiManifestDocument(applicationName)
            {
                Publisher = new(publisherName, publisherEmail),
                ApiDependencies = new() {
                    {
                        apiName, new() {
                            ApiDescriptionUrl = apiDescriptionUrl,
                            ApiDescriptionVersion = document.Info.Version,
                            ApiDeploymentBaseUrl = apiDeploymentBaseUrl,
                        }
                    }
                }
            };

            foreach (var path in document.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var requestInfo = new RequestInfo
                    {
                        Method = operation.Key.ToString(),
                        UriTemplate = apiDeploymentBaseUrl != default ? path.Key.TrimStart('/') : path.Key
                    };
                    apiManifest.ApiDependencies[apiName].Requests.Add(requestInfo);
                }
            }
            return apiManifest;
        }
    }
}

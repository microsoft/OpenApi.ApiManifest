// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using Microsoft.OpenApi.Models;

namespace Microsoft.OpenApi.ApiManifest.TypeExtensions
{
    public static class OpenApiDocumentExtensions
    {
        private const string DefaultPublisherName = "publisher-name";
        private const string DefaultPublisherEmail = "publisher-email@example.com";

        /// <summary>
        /// Converts an <see cref="OpenApiDocument"/> to an <see cref="ApiManifestDocument"/>.
        /// </summary>
        /// <param name="document">The OpenAPI document to convert.</param>
        /// <param name="apiDescriptionUrl">The URL of the API description.</param>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="apiDependencyName">The name of the API dependency.</param>
        /// <returns>An <see cref="ApiManifestDocument"/>.</returns>
        public static ApiManifestDocument ToApiManifest(this OpenApiDocument document, string? apiDescriptionUrl, string applicationName, string? apiDependencyName = default)
        {
            ArgumentNullException.ThrowIfNull(document);
            ValidationHelpers.ValidateNullOrWhitespace(nameof(apiDescriptionUrl), apiDescriptionUrl, nameof(ApiManifestDocument));
            ValidationHelpers.ValidateNullOrWhitespace(nameof(applicationName), applicationName, nameof(ApiManifestDocument));

            apiDependencyName = NormalizeApiName(apiDependencyName ?? document.Info.Title);
            var publisherName = document.Info.Contact?.Name ?? DefaultPublisherName;
            var publisherEmail = document.Info.Contact?.Email ?? DefaultPublisherEmail;

            string? apiDeploymentBaseUrl = GetApiDeploymentBaseUrl(document.Servers.FirstOrDefault());

            var apiManifest = new ApiManifestDocument(applicationName)
            {
                Publisher = new(publisherName, publisherEmail),
                ApiDependencies = new() {
                    {
                        apiDependencyName, new() {
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
                    apiManifest.ApiDependencies[apiDependencyName].Requests.Add(requestInfo);
                }
            }
            return apiManifest;
        }

        private static string NormalizeApiName(string apiName)
        {
            // Normalize OpenAPI document title to API name by trimming and replacing spaces with dashes.
            return apiName.Trim().Replace(' ', '-');
        }

        private static string? GetApiDeploymentBaseUrl(OpenApiServer? server)
        {
            if (server is null)
                return null;

            // Ensure the base URL ends with a slash.
            return !server.Url.EndsWith("/", StringComparison.Ordinal) ? $"{server.Url}/" : server.Url;
        }
    }
}

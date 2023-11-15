// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

namespace Microsoft.OpenApi.ApiManifest.TypeExtensions
{
    public static partial class OpenApiDocumentExtensions
    {
        [GeneratedRegex("[^a-zA-Z0-9]", RegexOptions.Compiled, 5000)]
        private static partial Regex SpecialCharactersInApiNameRegex();
        internal const string DefaultPublisherName = "publisher-name";
        internal const string DefaultPublisherEmail = "publisher-email@example.com";

        /// <summary>
        /// Converts an <see cref="OpenApiDocument"/> to an <see cref="ApiManifestDocument"/>.
        /// </summary>
        /// <param name="document">The OpenAPI document to convert.</param>
        /// <param name="apiDescriptionUrl">The URL of the API description.</param>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="apiDependencyName">The name of the API dependency. If not specified, it defaults to the title from the OpenAPI document.</param>
        /// <param name="publisherName"> The publisher name of the API manifest. If not supplied, it defaults to the contact name from the OpenAPI document, if available. In the absence of both, 'publisher-name' is used as a fallback.</param>
        /// <param name="publisherEmail">The publisher email of the API manifest. If not supplied, it defaults to the contact email from the OpenAPI document, if available.In the absence of both, 'publisher-email@example.com' is used as a fallback.</param>
        /// <returns>An <see cref="ApiManifestDocument"/>.</returns>
        public static ApiManifestDocument ToApiManifest(this OpenApiDocument document, string? apiDescriptionUrl, string applicationName, string? apiDependencyName = default, string? publisherName = default, string? publisherEmail = default)
        {
            ArgumentNullException.ThrowIfNull(document);
            ValidationHelpers.ValidateNullOrWhitespace(nameof(apiDescriptionUrl), apiDescriptionUrl, nameof(ApiManifestDocument));
            ValidationHelpers.ValidateNullOrWhitespace(nameof(applicationName), applicationName, nameof(ApiManifestDocument));

            if (string.IsNullOrEmpty(publisherName))
                publisherName = document.Info.Contact?.Name ?? DefaultPublisherName;

            if (string.IsNullOrEmpty(publisherEmail))
                publisherEmail = document.Info.Contact?.Email ?? DefaultPublisherEmail;

            apiDependencyName = NormalizeApiName(string.IsNullOrEmpty(apiDependencyName) ? document.Info.Title : apiDependencyName);
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
            // Normalize OpenAPI document title to API dependency name by removing all special characters from the provided api name.
            return SpecialCharactersInApiNameRegex().Replace(apiName, string.Empty);
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

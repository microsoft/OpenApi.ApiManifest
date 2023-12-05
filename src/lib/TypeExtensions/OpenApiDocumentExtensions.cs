// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

namespace Microsoft.OpenApi.ApiManifest.TypeExtensions
{
    public static partial class OpenApiDocumentExtensions
    {
        private static readonly Regex s_specialCharactersInApiNameRegex = new("[^a-zA-Z0-9]", RegexOptions.Compiled, TimeSpan.FromSeconds(5));
        internal const string DefaultPublisherName = "publisher-name";
        internal const string DefaultPublisherEmail = "publisher-email@example.com";

        /// <summary>
        /// Converts an <see cref="OpenApiDocument"/> to an <see cref="ApiManifestDocument"/>.
        /// </summary>
        /// <param name="document">The OpenAPI document to convert.</param>
        /// <param name="apiDescriptionUrl">The URL of the API description.</param>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="apiDependencyName">The name of the API dependency. If not specified, it defaults to the title from the OpenAPI document.</param>
        /// <param name="publisherName">
        /// The publisher's name for the API manifest. 
        /// If not provided, it defaults to the contact name from the OpenAPI document (if available).
        /// If the contact name is also not available, it defaults to 'publisher-name'.
        /// </param>
        /// <param name="publisherEmail">
        /// The publisher's email for the API manifest. 
        /// If not provided, it defaults to the contact email from the OpenAPI document (if available).
        /// If the contact email is also not available, it defaults to 'publisher-email@example.com'.
        /// </param>
        /// <returns>An <see cref="ApiManifestDocument"/>.</returns>
        public static ApiManifestDocument ToApiManifest(this OpenApiDocument document, string? apiDescriptionUrl, string applicationName, string? apiDependencyName = default, string? publisherName = default, string? publisherEmail = default)
        {
            ValidationHelpers.ThrowIfNull(document, nameof(document));
            ValidationHelpers.ValidateNullOrWhitespace(nameof(apiDescriptionUrl), apiDescriptionUrl, nameof(ApiManifestDocument));
            ValidationHelpers.ValidateNullOrWhitespace(nameof(applicationName), applicationName, nameof(ApiManifestDocument));

            if (string.IsNullOrEmpty(publisherName))
                publisherName = document.Info.Contact?.Name is string cName && !string.IsNullOrEmpty(cName) ? cName : DefaultPublisherName;

            if (string.IsNullOrEmpty(publisherEmail))
                publisherEmail = document.Info.Contact?.Email is string cEmail && !string.IsNullOrEmpty(cEmail) ? cEmail : DefaultPublisherEmail;

            apiDependencyName = NormalizeApiName(string.IsNullOrEmpty(apiDependencyName) ? document.Info.Title : apiDependencyName!);
            string? apiDeploymentBaseUrl = GetApiDeploymentBaseUrl(document.Servers.FirstOrDefault());

            var apiManifest = new ApiManifestDocument(applicationName)
            {
                Publisher = new(publisherName!, publisherEmail!),
                ApiDependencies = new() {
                    {
                        apiDependencyName, new() {
                            ApiDescriptionUrl = apiDescriptionUrl,
                            ApiDescriptionVersion = document.Info.Version,
                            ApiDeploymentBaseUrl = apiDeploymentBaseUrl
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
            return s_specialCharactersInApiNameRegex.Replace(apiName, string.Empty);
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

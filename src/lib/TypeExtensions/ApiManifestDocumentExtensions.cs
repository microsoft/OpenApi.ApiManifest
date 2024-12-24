using Microsoft.OpenApi.ApiManifest.Exceptions;
using Microsoft.OpenApi.ApiManifest.Helpers;
using Microsoft.OpenApi.ApiManifest.OpenAI;
using Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace Microsoft.OpenApi.ApiManifest.TypeExtensions
{
    public static class ApiManifestDocumentExtensions
    {
        /// <summary>
        /// Converts an instance of <see cref="ApiManifestDocument"/> to an instance of <see cref="OpenAIPluginManifest"/>.
        /// </summary>
        /// <param name="apiManifestDocument">A valid instance of <see cref="ApiManifestDocument"/> to generate an OpenAI Plugin manifest from.</param>
        /// <param name="logoUrl">The URL to a logo for the plugin.</param>
        /// <param name="legalInfoUrl">The URL to a page with legal information about the plugin.</param>
        /// <param name="apiDependencyName">The name of apiDependency to use from the provided <see cref="ApiManifestDocument.ApiDependencies"/>. The method defaults to the first apiDependency in  <see cref="ApiManifestDocument.ApiDependencies"/> if no value is provided.</param>
        /// <param name="openApiPath">The path to where the OpenAPI file that's packaged with the plugin manifest is stored. The method defaults to the ApiDependency.ApiDescriptionUrl if none is provided.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A <see cref="Task{OpenAIPluginManifest}"/></returns>
        public static async Task<OpenAIPluginManifest> ToOpenAIPluginManifestAsync(this ApiManifestDocument apiManifestDocument, string logoUrl, string legalInfoUrl, string? apiDependencyName = default, string? openApiPath = default, CancellationToken cancellationToken = default)
        {
            ValidationHelpers.ThrowIfNull(apiManifestDocument, nameof(apiManifestDocument));
            if (!TryGetApiDependency(apiManifestDocument.ApiDependencies, apiDependencyName, out ApiDependency? apiDependency))
            {
                throw new ApiManifestException(string.Format(CultureInfo.InvariantCulture, ErrorMessage.ApiDependencyNotFound, nameof(OpenAIPluginManifest)));
            }
            else if (string.IsNullOrWhiteSpace(apiDependency?.ApiDescriptionUrl))
            {
                throw new ApiManifestException(string.Format(CultureInfo.InvariantCulture, ErrorMessage.ApiDescriptionUrlNotFound, nameof(OpenAIPluginManifest)));
            }
            else
            {
                var result = await ParsingHelpers.ParseOpenApiAsync(new Uri(apiDependency!.ApiDescriptionUrl), false, cancellationToken).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(openApiPath))
                    openApiPath = apiDependency.ApiDescriptionUrl;
                return apiManifestDocument.ToOpenAIPluginManifest(result.Document, logoUrl, legalInfoUrl, openApiPath!);
            }
        }

        /// <summary>
        /// Converts an instance of <see cref="ApiManifestDocument"/> to an instance of <see cref="OpenAIPluginManifest"/>.
        /// </summary>
        /// <param name="apiManifestDocument">A valid instance of <see cref="ApiManifestDocument"/> with at least one API dependency.</param>
        /// <param name="openApiDocument">The OpenAPI document to use for the OpenAIPluginManifest.</param>
        /// <param name="logoUrl">The URL to a logo for the plugin.</param>
        /// <param name="legalInfoUrl">The URL to a page with legal information about the plugin.</param>
        /// <param name="openApiPath">The path to where the OpenAPI file that's packaged with the plugin manifest is stored.</param>
        /// <returns>A <see cref="OpenAIPluginManifest"/></returns>
        public static OpenAIPluginManifest ToOpenAIPluginManifest(this ApiManifestDocument apiManifestDocument, OpenApiDocument openApiDocument, string logoUrl, string legalInfoUrl, string openApiPath)
        {
            ValidationHelpers.ThrowIfNull(apiManifestDocument, nameof(apiManifestDocument));
            ValidationHelpers.ThrowIfNull(openApiDocument, nameof(openApiDocument));
            // Validates the ApiManifestDocument before generating the OpenAI manifest. This includes the publisher object.
            apiManifestDocument.Validate();
            string contactEmail = apiManifestDocument.Publisher?.ContactEmail!;

            var openApiManifest = OpenApiPluginFactory.CreateOpenAIPluginManifest(openApiDocument.Info.Title, openApiDocument.Info.Title, logoUrl, contactEmail, legalInfoUrl);
            openApiManifest.Api = new Api("openapi", openApiPath);
            openApiManifest.Auth = new ManifestNoAuth();
            openApiManifest.DescriptionForHuman = openApiDocument.Info.Description ?? $"Description for {openApiManifest.NameForHuman}.";
            openApiManifest.DescriptionForModel = openApiManifest.DescriptionForHuman;

            return openApiManifest;
        }

        /// <summary>
        /// Tries to get an <see cref="ApiDependency"/> from the provided <see cref="ApiDependencies"/>.
        /// </summary>
        /// <param name="apiDependencies">The <see cref="ApiDependencies"/> to search for the apiDependency.</param>
        /// <param name="apiDependencyName">The name of apiDependency to use from the provided <see cref="ApiManifestDocument.ApiDependencies"/>. The method defaults to the first apiDependency in <see cref="ApiManifestDocument.ApiDependencies"/> if no value is provided.</param>
        /// <param name="apiDependency">The <see cref="ApiDependency"/> that was found.</param>
        /// <returns>Returns true if the apiDependency is found and not null, otherwise false.</returns>
        private static bool TryGetApiDependency(ApiDependencies apiDependencies, string? apiDependencyName, out ApiDependency? apiDependency)
        {
            if (string.IsNullOrEmpty(apiDependencyName))
                apiDependency = apiDependencies.FirstOrDefault().Value;
            else
                _ = apiDependencies.TryGetValue(apiDependencyName!, out apiDependency);
            return apiDependency != null;
        }
    }
}

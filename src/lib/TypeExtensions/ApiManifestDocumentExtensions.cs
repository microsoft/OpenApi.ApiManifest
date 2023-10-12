using Microsoft.OpenApi.ApiManifest.Helpers;
using Microsoft.OpenApi.ApiManifest.OpenAI;
using Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;
using Microsoft.OpenApi.Models;

namespace Microsoft.OpenApi.ApiManifest.TypeExtensions
{
    public static class ApiManifestDocumentExtensions
    {
        /// <summary>
        /// Generates an OpenAIPluginManifest from the provided ApiManifestDocument.
        /// </summary>
        /// <param name="apiManifestDocument">A valid instance of <see cref="ApiManifestDocument"/> to generate an OpenAI Plugin manifest from.</param>
        /// <param name="logoUrl"> The URL to a logo for the plugin.</param>
        /// <param name="legalInfoUrl">The URL to a page with legal information about the plugin.</param>
        /// <param name="apiDependencyName">The name of apiDependency to use from the provided <see cref="ApiManifestDocument.ApiDependencies"/>. The method defaults to the first apiDependency in  <see cref="ApiManifestDocument.ApiDependencies"/> if no value is provided.</param>
        /// <param name="openApiFilePath">The relative path to where the OpenAPI file that's packaged with the plugin manifest if stored. The method default './openapi.json' if none is provided.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A <see cref="Task{OpenAIPluginManifest}"/></returns>
        public static async Task<OpenAIPluginManifest> ToOpenAIPluginManifestAsync(this ApiManifestDocument apiManifestDocument, string logoUrl, string legalInfoUrl, string? apiDependencyName = default, string openApiFilePath = "./openapi.json", CancellationToken cancellationToken = default)
        {
            if (!TryGetApiDependency(apiManifestDocument.ApiDependencies, apiDependencyName, out ApiDependency? apiDependency))
            {
                throw new ArgumentException("Failed to get a valid apiDependency from the provided apiManifestDocument", nameof(apiManifestDocument.ApiDependencies));
            }
            else if (string.IsNullOrWhiteSpace(apiDependency?.ApiDescriptionUrl))
            {
                throw new ArgumentNullException(nameof(apiDependency.ApiDescriptionUrl), "ApiDescriptionUrl is missing in the provided apiManifestDocument. The property is required generate a complete OpenAI Plugin manifest.");
            }
            else
            {
                var result = await ParsingHelpers.ParseOpenApiAsync(apiDependency.ApiDescriptionUrl, false, cancellationToken);
                OpenApiDocument document = result.OpenApiDocument;

                return apiManifestDocument.ToOpenAIPluginManifest(openApiDocument: document, logoUrl: logoUrl, legalInfoUrl: legalInfoUrl, openApiFilePath: openApiFilePath);
            }
        }

        internal static OpenAIPluginManifest ToOpenAIPluginManifest(this ApiManifestDocument apiManifestDocument, OpenApiDocument openApiDocument, string logoUrl, string legalInfoUrl, string openApiFilePath)
        {
            // Validate the ApiManifestDocument before generating the OpenAI manifest.
            apiManifestDocument.Validate();
            string contactEmail = string.IsNullOrWhiteSpace(apiManifestDocument.Publisher?.ContactEmail) ? string.Empty : apiManifestDocument.Publisher.ContactEmail;

            var openApiManifest = OpenApiPluginFactory.CreateOpenAIPluginManifest(
                schemaVersion: openApiDocument.Info.Version,
                nameForHuman: openApiDocument.Info.Title,
                nameForModel: openApiDocument.Info.Title,
                logoUrl: logoUrl,
                contactEmail: contactEmail,
                legalInfoUrl: legalInfoUrl);

            openApiManifest.Api = new Api("openapi", openApiFilePath);
            openApiManifest.Auth = new ManifestNoAuth();
            openApiManifest.DescriptionForHuman = openApiDocument.Info.Description ?? $"Description for {openApiManifest.NameForHuman}.";
            openApiManifest.DescriptionForModel = openApiManifest.DescriptionForHuman;

            return openApiManifest;
        }

        private static bool TryGetApiDependency(ApiDependencies apiDependencies, string? apiDependencyName, out ApiDependency? apiDependency)
        {
            if (apiDependencyName == default)
                apiDependency = apiDependencies.FirstOrDefault().Value;
            else
                _ = apiDependencies.TryGetValue(apiDependencyName, out apiDependency);
            return apiDependency != null;
        }
    }
}

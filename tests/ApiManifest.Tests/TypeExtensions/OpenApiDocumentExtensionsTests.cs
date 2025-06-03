using Microsoft.OpenApi.ApiManifest.TypeExtensions;
using Microsoft.OpenApi;

namespace Microsoft.OpenApi.ApiManifest.Tests.TypeExtensions
{
    public class OpenApiDocumentExtensionsTests
    {
        private readonly OpenApiDocument exampleDocument;
        public OpenApiDocumentExtensionsTests()
        {
            exampleDocument = CreateDocument();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ToApiManifestWithNullApiDescriptionUrlThrowsArgumentException(string? apiDescriptionUrl)
        {
            // Arrange
            var document = new OpenApiDocument();

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => document.ToApiManifest(apiDescriptionUrl, "application-name"));

            // Assert
            Assert.Equal("apiDescriptionUrl", exception.ParamName);
        }

        [Fact]
        public void ToApiManifestWithNullApplicationNameThrowsArgumentException()
        {
            // Arrange
            var document = new OpenApiDocument();
            var apiDescriptionUrl = "https://example.com/api-description.yaml";

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => document.ToApiManifest(apiDescriptionUrl, string.Empty));

            // Assert
            Assert.Equal("applicationName", exception.ParamName);
        }

        [Fact]
        public void ToApiManifestWithValidDocumentReturnsApiManifestDocument()
        {
            // Arrange
            var apiDescriptionUrl = "https://example.com/api-description.yaml";
            var applicationName = "application-name";

            // Act
            var apiManifest = exampleDocument.ToApiManifest(apiDescriptionUrl, applicationName);

            // Assert
            Assert.NotNull(apiManifest);
            Assert.Equal(applicationName, apiManifest.ApplicationName);
            Assert.NotNull(apiManifest.Publisher);
            Assert.Equal(exampleDocument.Info.Contact?.Name, apiManifest.Publisher?.Name);
            Assert.Equal(exampleDocument.Info.Contact?.Email, apiManifest.Publisher?.ContactEmail);
            Assert.NotNull(apiManifest.ApiDependencies);
            _ = Assert.Single(apiManifest.ApiDependencies);
            Assert.Equal("GraphAPI", apiManifest.ApiDependencies.First().Key);
            Assert.Equal(apiDescriptionUrl, apiManifest.ApiDependencies.First().Value.ApiDescriptionUrl);
            Assert.Equal(exampleDocument.Info.Version, apiManifest.ApiDependencies.First().Value.ApiDescriptionVersion);
            Assert.NotNull(exampleDocument.Servers);
            Assert.NotEmpty(exampleDocument.Servers);
            Assert.Equal(exampleDocument.Servers[0].Url, apiManifest.ApiDependencies.First().Value.ApiDeploymentBaseUrl);
            Assert.Equal(exampleDocument.Paths.Count, apiManifest.ApiDependencies.First().Value.Requests.Count);
        }

        [Fact]
        public void ToApiManifestWithValidDocumentAndApiDependencyNameReturnsApiManifestDocument()
        {
            // Arrange
            var apiDescriptionUrl = "https://example.com/api-description.yaml";
            var applicationName = "application-name";
            var apiDependencyName = "graph";

            // Act
            var apiManifest = exampleDocument.ToApiManifest(apiDescriptionUrl, applicationName, apiDependencyName);

            // Assert
            Assert.NotNull(apiManifest);
            Assert.Equal(applicationName, apiManifest.ApplicationName);
            Assert.NotNull(apiManifest.Publisher);
            Assert.Equal(exampleDocument.Info.Contact?.Name, apiManifest.Publisher?.Name);
            Assert.Equal(exampleDocument.Info.Contact?.Email, apiManifest.Publisher?.ContactEmail);
            Assert.NotNull(apiManifest.ApiDependencies);
            _ = Assert.Single(apiManifest.ApiDependencies);
            Assert.Equal(apiDependencyName, apiManifest.ApiDependencies.First().Key);
            Assert.Equal(apiDescriptionUrl, apiManifest.ApiDependencies.First().Value.ApiDescriptionUrl);
            Assert.Equal(exampleDocument.Info.Version, apiManifest.ApiDependencies.First().Value.ApiDescriptionVersion);
            Assert.NotNull(exampleDocument.Servers);
            Assert.NotEmpty(exampleDocument.Servers);
            Assert.Equal(exampleDocument.Servers[0].Url, apiManifest.ApiDependencies.First().Value.ApiDeploymentBaseUrl);
            Assert.Equal(exampleDocument.Paths.Count, apiManifest.ApiDependencies.First().Value.Requests.Count);
        }

        [Fact]
        public void ToApiManifestWithValidDocumentAndApiDependencyNameAndApiDeploymentBaseUrlReturnsApiManifestDocument()
        {
            // Arrange
            var apiDescriptionUrl = "https://example.com/api-description.yaml";
            var applicationName = "application-name";
            var apiDependencyName = "graph";
            var apiDeploymentBaseUrl = "https://example.com/api/";

            // Act
            var apiManifest = exampleDocument.ToApiManifest(apiDescriptionUrl, applicationName, apiDependencyName);

            // Assert
            Assert.NotNull(apiManifest);
            Assert.Equal(applicationName, apiManifest.ApplicationName);
            Assert.NotNull(apiManifest.Publisher);
            Assert.Equal(exampleDocument.Info.Contact?.Name, apiManifest.Publisher?.Name);
            Assert.Equal(exampleDocument.Info.Contact?.Email, apiManifest.Publisher?.ContactEmail);
            Assert.NotNull(apiManifest.ApiDependencies);
            _ = Assert.Single(apiManifest.ApiDependencies);
            Assert.Equal(apiDependencyName, apiManifest.ApiDependencies.First().Key);
            Assert.Equal(apiDescriptionUrl, apiManifest.ApiDependencies.First().Value.ApiDescriptionUrl);
            Assert.Equal(exampleDocument.Info.Version, apiManifest.ApiDependencies.First().Value.ApiDescriptionVersion);
            Assert.Equal(apiDeploymentBaseUrl, apiManifest.ApiDependencies.First().Value.ApiDeploymentBaseUrl);
            Assert.Equal(exampleDocument.Paths.Count, apiManifest.ApiDependencies.First().Value.Requests.Count);
        }

        [Fact]
        public void ToApiManifestWhenOpenApiContactInfoIsNullAndNoPublisherInfoIIsProvidedReturnsApiManifestDocument()
        {
            // Arrange
            var apiDescriptionUrl = "https://example.com/api-description.yaml";
            var applicationName = "application-name";
            var apiDependencyName = "graph";
            var apiDeploymentBaseUrl = "https://example.com/api/";
            var localExampleDocument = CreateDocument();
            localExampleDocument.Info.Contact = null;

            // Act
            var apiManifest = localExampleDocument.ToApiManifest(apiDescriptionUrl, applicationName, apiDependencyName);

            // Assert
            Assert.NotNull(apiManifest);
            Assert.Equal(applicationName, apiManifest.ApplicationName);
            Assert.NotNull(apiManifest.Publisher);
            Assert.Equal(OpenApiDocumentExtensions.DefaultPublisherName, apiManifest.Publisher?.Name);
            Assert.Equal(OpenApiDocumentExtensions.DefaultPublisherEmail, apiManifest.Publisher?.ContactEmail);
            Assert.NotNull(apiManifest.ApiDependencies);
            _ = Assert.Single(apiManifest.ApiDependencies);
            Assert.Equal(apiDependencyName, apiManifest.ApiDependencies.First().Key);
            Assert.Equal(apiDescriptionUrl, apiManifest.ApiDependencies.First().Value.ApiDescriptionUrl);
            Assert.Equal(localExampleDocument.Info.Version, apiManifest.ApiDependencies.First().Value.ApiDescriptionVersion);
            Assert.Equal(apiDeploymentBaseUrl, apiManifest.ApiDependencies.First().Value.ApiDeploymentBaseUrl);
            Assert.Equal(localExampleDocument.Paths.Count, apiManifest.ApiDependencies.First().Value.Requests.Count);
        }

        [Fact]
        public void ToApiManifestWithAllParametersReturnsApiManifestDocument()
        {
            // Arrange
            var apiDescriptionUrl = "https://example.com/api-description.yaml";
            var applicationName = "application-name";
            var apiDependencyName = "graph";
            var apiDeploymentBaseUrl = "https://example.com/api/";
            var publisherName = "FooBar";
            var publisherEmail = "FooBar@contoso.com";

            // Act
            var apiManifest = exampleDocument.ToApiManifest(apiDescriptionUrl, applicationName, apiDependencyName, publisherName, publisherEmail);

            // Assert
            Assert.NotNull(apiManifest);
            Assert.Equal(applicationName, apiManifest.ApplicationName);
            Assert.NotNull(apiManifest.Publisher);
            Assert.Equal(publisherName, apiManifest.Publisher?.Name);
            Assert.Equal(publisherEmail, apiManifest.Publisher?.ContactEmail);
            Assert.NotNull(apiManifest.ApiDependencies);
            _ = Assert.Single(apiManifest.ApiDependencies);
            Assert.Equal(apiDependencyName, apiManifest.ApiDependencies.First().Key);
            Assert.Equal(apiDescriptionUrl, apiManifest.ApiDependencies.First().Value.ApiDescriptionUrl);
            Assert.Equal(exampleDocument.Info.Version, apiManifest.ApiDependencies.First().Value.ApiDescriptionVersion);
            Assert.Equal(apiDeploymentBaseUrl, apiManifest.ApiDependencies.First().Value.ApiDeploymentBaseUrl);
            Assert.Equal(exampleDocument.Paths.Count, apiManifest.ApiDependencies.First().Value.Requests.Count);
        }

        private static OpenApiDocument CreateDocument()
        {
            return new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = " Graph + API ",
                    Version = "v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "publisher-name",
                        Email = "foo@bar.com"
                    }
                },
                Servers = new List<OpenApiServer>
                {
                    new OpenApiServer
                    {
                        Url = "https://example.com/api/"
                    }
                },
                Paths = new OpenApiPaths
                {
                    ["/users"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<HttpMethod, OpenApiOperation>
                        {
                            [HttpMethod.Get] = new OpenApiOperation()
                        }
                    },
                    ["/groups"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<HttpMethod, OpenApiOperation>
                        {
                            [HttpMethod.Get] = new OpenApiOperation()
                        }
                    }
                }
            };
        }
    }
}

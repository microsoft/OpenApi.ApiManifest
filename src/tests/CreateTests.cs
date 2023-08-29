using System.Text.Json.Nodes;

namespace Tests.ApiManifest;

public class CreateTests
{

    [Fact]
    public void CreateApiManifestDocumentWithRequiredFields()
    {
        var apiManifest = new ApiManifestDocument("application-name");
        Assert.NotNull(apiManifest);
        Assert.Equal("application-name", apiManifest.ApplicationName);
        Assert.Null(apiManifest.Publisher);
        Assert.Empty(apiManifest.ApiDependencies);
        Assert.Empty(apiManifest.Extensions);
    }

    [Fact]
    public void CreatePublisher()
    {
        var publisher = new Publisher(name: "Contoso", contactEmail: "foo@bar.com");
        Assert.Equal("Contoso", publisher.Name);
        Assert.Equal("foo@bar.com", publisher.ContactEmail);

    }

    // Create test to instantiate ApiManifest with auth
    [Fact]
    public void CreateApiManifestWithAuthorizationRequirements()
    {
        var apiManifest = new ApiManifestDocument("application-name")
        {
            Publisher = new(name: "Contoso", contactEmail: "foo@bar.com"),
            ApiDependencies = new() {
                { "Contoso.Api", new() {
                    AuthorizationRequirements = new() {
                        ClientIdentifier = "2143234-234324-234234234-234",
                        Access = new() {
                            new() { Type = "oauth2",
                                    Content = new JsonObject() {
                                                { "scopes", new JsonArray() { "user.read", "user.write" } }
                                            }
                                }
                            }
                        }
                    }
                }
            }
        };
        Assert.NotNull(apiManifest.ApiDependencies["Contoso.Api"].AuthorizationRequirements);
        Assert.Equal("2143234-234324-234234234-234", apiManifest?.ApiDependencies["Contoso.Api"]?.AuthorizationRequirements?.ClientIdentifier);
        Assert.Equal("oauth2", apiManifest?.ApiDependencies["Contoso.Api"]?.AuthorizationRequirements?.Access[0].Type);
    }

}
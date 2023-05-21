using System.Text.Json.Nodes;

namespace Tests.ApiManifest;

public class CreateTests {

    [Fact]
    public void CreateEmptyApiManifestDocument() {
        var apiManifest = new ApiManifestDocument();
        Assert.NotNull(apiManifest);
    }

    [Fact]
    public void CreatePublisher() {
        var publisher = new Publisher() {
            Name = "Contoso",
            ContactEmail = "foo@bar.com"
        };
        Assert.Equal("Contoso", publisher.Name);
        Assert.Equal("foo@bar.com", publisher.ContactEmail);
        
    }

    // Create test to instantiate ApiManifest with auth
    [Fact]
    public void CreateApiManifestWithAuth() {
        var apiManifest = new ApiManifestDocument()
        {
            Publisher = new()
            {
                Name = "Contoso",
                ContactEmail = "foo@bar.com"
            },
            ApiDependencies = new() {
                { "Contoso.Api", new() {
                    Auth = new() {
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
        Assert.NotNull(apiManifest.ApiDependencies["Contoso.Api"].Auth);
        Assert.Equal("2143234-234324-234234234-234", apiManifest.ApiDependencies["Contoso.Api"].Auth.ClientIdentifier);
        Assert.Equal("oauth2", apiManifest.ApiDependencies["Contoso.Api"].Auth.Access[0].Type);
    }

}
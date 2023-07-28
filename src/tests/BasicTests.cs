using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Tests.ApiManifest;

public class BasicTests
{
    ApiManifestDocument exampleApiManifest;
    public BasicTests()
    {
        exampleApiManifest = CreateDocument();
    }

    // Create test to instantiate a simple ApiManifestDocument
    [Fact]
    public void InitializeDocument()
    {
        Assert.NotNull(exampleApiManifest);
    }

    // Serialize the ApiManifestDocument to a string
    [Fact]
    public void SerializeDocument()
    {
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream);
        exampleApiManifest.Write(writer);
        writer.Flush();
        // Read string from stream
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        Debug.WriteLine(json);
        var doc = JsonDocument.Parse(json);       
        Assert.NotNull(doc);
    }

    // Deserialize the ApiManifestDocument from a string
    [Fact]
    public void DeserializeDocument()
    {
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream);
        exampleApiManifest.Write(writer);
        writer.Flush();
        // Read string from stream
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var doc = JsonDocument.Parse(json);
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);
        Assert.Equivalent(exampleApiManifest.Publisher, apiManifest.Publisher );
        Assert.Equivalent(exampleApiManifest.ApiDependencies["example"].Requests, apiManifest.ApiDependencies["example"].Requests );
        Assert.Equivalent(exampleApiManifest.ApiDependencies["example"].ApiDescripionUrl, apiManifest.ApiDependencies["example"].ApiDescripionUrl );
        var expectedAuth = exampleApiManifest.ApiDependencies["example"].Auth;
        var actualAuth = apiManifest.ApiDependencies["example"].Auth;
        Assert.Equivalent(expectedAuth?.ClientIdentifier, actualAuth?.ClientIdentifier );
        Assert.Equivalent(expectedAuth?.Access[0].Content.ToJsonString(), actualAuth.Access[0].Content.ToJsonString() );
    }


    // Create an empty document
    [Fact]
    public void CreateEmptyDocument()
    {
        var doc = new ApiManifestDocument();
        Assert.NotNull(doc);
        Assert.NotNull(doc.ApiDependencies);
        Assert.Empty(doc.ApiDependencies);
    }

    // Create a document with a publisher that is missing contactEmail
    [Fact]
    public void CreateDocumentWithMissingContactEmail()
    {
        Assert.Throws<ArgumentNullException>(()=> {
            var doc = new ApiManifestDocument() {
                Publisher = new("") {
                    Name = "Microsoft"
                }
            };
        }  
        );
    }

    private static ApiManifestDocument CreateDocument()
    {
        return new ApiManifestDocument() {
            Publisher = new("example@example.org") {
                Name = "Microsoft"
            },
            ApiDependencies = new() {
                { "example", new()
                    {
                        ApiDescripionUrl = "https://example.org",
                        Auth = new()
                        {
                            ClientIdentifier = "1234",
                            Access = new() {
                                new () { Type= "application", Content = new JsonObject() {
                                        { "scopes", new JsonArray() {"User.Read.All"} }}
                                     } ,
                                new () { Type= "delegated", Content = new JsonObject() {
                                        { "scopes", new JsonArray() {"User.Read", "Mail.Read"} }}
                                     }
                            }
                        },
                        Requests = new() {
                            new() { Method = "GET", UriTemplate = "/api/v1/endpoint" },
                            new () { Method = "POST", UriTemplate = "/api/v1/endpoint"}
                        }
                    }
                }
            }
        };
    }

}
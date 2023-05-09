using System.Diagnostics;
using System.Text.Json;

namespace tests;

public class UnitTest1
{
    ApiManifestDocument exampleApiManifest;
    public UnitTest1()
    {
        exampleApiManifest = CreateDocument();
    }

    [Fact]
    public void Test1()
    {

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
        Assert.Equivalent(exampleApiManifest, apiManifest );
    }

    private static ApiManifestDocument CreateDocument()
    {
        var apiManifest = new ApiManifestDocument();
        apiManifest.Publisher = new Publisher();
        apiManifest.Publisher.Name = "Microsoft";
        apiManifest.Publisher.ContactEmail = "example@example.org";
        apiManifest.ApiDependencies.Add(new ApiDependency()
        {
            ApiDescripionUrl = "https://example.org",
            Auth = new Auth()
            {
                ClientId = "1234",
                Permissions = new() {
                    {"application", new() {"read"}},
                    {"delegated", new() {"read", "write"}}
                }
            },
            Requests = new List<Request>() {
                new() {
                    Method = "GET",
                    UriTemplate = "/api/v1/endpoint"
                },
                new () {
                    Method = "POST",
                    UriTemplate = "/api/v1/endpoint"
                }
            }
        });
        return apiManifest;
    }


}
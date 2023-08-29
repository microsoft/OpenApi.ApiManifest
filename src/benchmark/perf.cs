using BenchmarkDotNet.Attributes;
using Microsoft.OpenApi.ApiManifest;
using System.Text.Json;

[MemoryDiagnoser] // we need to enable it in explicit way
public class Perf
{

    private MemoryStream? apiManifestStream;
    private JsonSerializerOptions? autoSerializationOptions;

    [GlobalSetup]
    public void GlobalSetup()
    {
        autoSerializationOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        apiManifestStream = new MemoryStream();
        var json = @"
        {
    ""publisher"": {
        ""name"": ""Alice"",
        ""contactEmail"": ""alice@example.org""
    },
    ""apiDependencies"": [
        {
            ""apiDescripionUrl"": ""https://example.org/openapi.json"",
            ""auth"": {
                ""clientId"": ""some-uuid-here"",
                ""permissions"": {
                    ""delegated"": [
                        ""resourceA.ReadWrite"",
                        ""resourceB.ReadWrite""
                    ],
                    ""application"": [
                        ""resourceB.Read""
                    ]
                }
            },
            ""requests"": [
                {
                    ""method"": ""GET"",
                    ""uriTemplate"": ""https://example.org/api/resourceA""
                },
                {
                    ""method"": ""GET"",
                    ""uriTemplate"": ""https://example.org/api/resourceB""
                }
            ]
        }
    ]
}
        ";
        StreamWriter writer = new StreamWriter(apiManifestStream);
        writer.Write(json);
        writer.Flush();

    }

    [Benchmark]
    public void DeserializeApiManifest()
    {
        // Read string from stream
        apiManifestStream!.Position = 0;
        JsonDocument doc = JsonDocument.Parse(apiManifestStream);
        _ = ApiManifestDocument.Load(doc.RootElement);
    }


    [Benchmark]
    public void AutoDeserializeApiManifest()
    {
        apiManifestStream!.Position = 0;
        _ = JsonSerializer.Deserialize<ApiManifestDocument>(apiManifestStream, autoSerializationOptions);
    }

    [Benchmark]
    public void AutoDeserializeApiManifestFromRootElement()
    {
        apiManifestStream!.Position = 0;
        JsonDocument doc = JsonDocument.Parse(apiManifestStream);
        _ = JsonSerializer.Deserialize<ApiManifestDocument>(doc.RootElement, autoSerializationOptions);
    }

}
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Microsoft.OpenApi.ApiManifest;

[MemoryDiagnoser] // we need to enable it in explicit way
public class Perf {
   
    private MemoryStream? apiManifestStream;
    private JsonSerializerOptions? autoSerializationOptions;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        autoSerializationOptions = new JsonSerializerOptions() {
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
        var writer = new StreamWriter(apiManifestStream);
        writer.Write(json);
        writer.Flush();

    }

    [Benchmark]
    public void DeserializeApiManifest()
    {
       // Read string from stream
        apiManifestStream!.Position = 0;
        var doc = JsonDocument.Parse(apiManifestStream);
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);
    }


    [Benchmark]
    public void AutoDeserializeApiManifest()
    {
        apiManifestStream!.Position = 0;
        var apiManifest = JsonSerializer.Deserialize<ApiManifestDocument>(apiManifestStream, autoSerializationOptions);
    }

    [Benchmark]
    public void AutoDeserializeApiManifestFromRootElement()
    {
        apiManifestStream!.Position = 0;
        var doc = JsonDocument.Parse(apiManifestStream);
        var apiManifest = JsonSerializer.Deserialize<ApiManifestDocument>(doc.RootElement, autoSerializationOptions);
    }

}
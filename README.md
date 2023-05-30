# API Manifest

This project is a parser and DOM(Document object model) for the API Manifest media type.  An early draft of the specification is [available](https://darrelmiller.github.io/api-manifest/draft-miller-api-manifest.html).

An "api manifest" is a way to store the dependencies that an application has on HTTP APIs. It contains characteristics of those dependencies including links to API descriptions, specifics of the types of HTTP API requests made by the application and related authorization information.

You can create an API manifest in code:

```csharp
 var apiManifest = new ApiManifestDocument() {
            Publisher = new() {
                Name = "Microsoft",
                ContactEmail = "example@example.org"
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
```

or you can read it from a stream

```csharp
var doc = JsonDocument.Parse(stream);
var apiManifest = ApiManifestDocument.Load(doc.RootElement);
```

An ApiManifest object can be serialized to a JSON stream as follows:

```csharp
var stream = new MemoryStream();
var writer = new Utf8JsonWriter(stream);
exampleApiManifest.Write(writer);
writer.Flush();
```

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.

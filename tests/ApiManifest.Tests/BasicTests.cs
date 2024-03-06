// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.OpenApi.ApiManifest.Tests;
public class BasicTests
{
    private readonly ApiManifestDocument exampleApiManifest;
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
        Assert.Equal("application-name", doc.RootElement.GetProperty("applicationName").GetString());
        Assert.Equal("Microsoft", doc.RootElement.GetProperty("publisher").GetProperty("name").GetString());
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
        Assert.Equivalent(exampleApiManifest.Publisher, apiManifest.Publisher);
        Assert.Equivalent(exampleApiManifest.ApiDependencies["example"].Requests, apiManifest.ApiDependencies["example"].Requests);
        Assert.Equivalent(exampleApiManifest.ApiDependencies["example"].ApiDescriptionUrl, apiManifest.ApiDependencies["example"].ApiDescriptionUrl);
        Assert.Equivalent(exampleApiManifest.ApiDependencies["example"].ApiDeploymentBaseUrl, apiManifest.ApiDependencies["example"].ApiDeploymentBaseUrl);
        var expectedAuth = exampleApiManifest.ApiDependencies["example"].AuthorizationRequirements;
        var actualAuth = apiManifest.ApiDependencies["example"].AuthorizationRequirements;
        Assert.Equivalent(expectedAuth?.ClientIdentifier, actualAuth?.ClientIdentifier);
        Assert.Equivalent(expectedAuth?.Access?[0]?.Content?.ToJsonString(), actualAuth?.Access?[0]?.Content?.ToJsonString());
        Assert.NotNull(exampleApiManifest.Extensions["api-manifest-extension"]);
        Assert.Equal(exampleApiManifest.Extensions["api-manifest-extension"]?.ToString(), apiManifest.Extensions?["api-manifest-extension"]?.ToString());
        Assert.NotNull(exampleApiManifest.ApiDependencies["example"]?.Extensions?["EXAMPLE-API-DEPENDENCY-EXTENSION"]);
        Assert.Equal(exampleApiManifest.ApiDependencies["example"]?.Extensions?["example-API-dependency-extension"]?.ToString(), apiManifest.ApiDependencies["example"]?.Extensions?["example-api-dependency-extension"]?.ToString());
    }

    [Fact]
    public void AcceptsMultipleDependenciesWithDifferentCasing()
    {
        var document = new ApiManifestDocument("foo");
        document.ApiDependencies.Add("bar", new());
        document.ApiDependencies.Add("BAR", new());
        Assert.Equal(2, document.ApiDependencies.Count);
    }

    // Create an empty document
    [Fact]
    public void CreateDocumentWithRequiredFields()
    {
        var doc = new ApiManifestDocument("application-name");
        Assert.NotNull(doc);
        Assert.Equal("application-name", doc.ApplicationName);
        Assert.NotNull(doc.ApiDependencies);
        Assert.Empty(doc.ApiDependencies);
    }

    // Create a document with a publisher that is missing required fields (name and contactEmail).
    [Fact]
    public void CreateDocumentWithMissingRequiredPublisherFields()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            var doc = new ApiManifestDocument("application-name")
            {
                Publisher = new("", "")
            };
        }
        );
    }

    [Fact]
    public void FailToParseDocumentWithoutApplicationName()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            var serializedValue = """
            {
                "apiDependencies": {
                    "graph": {
                        "apiDescriptionUrl": "https://example.org",
                        "requests": [
                            {
                                "method": "GET",
                                "uriTemplate": "/directoryObjects/{directoryObject-id}"
                            }
                        ]
                    }
                }
            }
            """;
            var doc = JsonDocument.Parse(serializedValue);
            _ = ApiManifestDocument.Load(doc.RootElement);
        }
        );
    }

    [Fact]
    public void FailToParseDocumentWithoutRequests()
    {
        _ = Assert.Throws<ArgumentException>(() =>
        {
            var serializedValue = "{\"applicationName\": \"application-name\", \"apiDependencies\": { \"graph\": {\"apiDescriptionUrl\":\"https://example.org\"}}}";
            var doc = JsonDocument.Parse(serializedValue);
            _ = ApiManifestDocument.Load(doc.RootElement);
        }
        );
    }

    [Fact]
    public void ParsesApiDescriptionUrlField()
    {
        // Given
        var serializedValue = """
        {
        "applicationName": "application-name",
            "apiDependencies": {
                "graph": {
                    "apiDescriptionUrl": "https://example.org",
                    "requests": [
                        {
                            "method": "GET",
                            "uriTemplate": "/directoryObjects/{directoryObject-id}"
                        }
                    ]
                }
            }
        }
        """;
        var doc = JsonDocument.Parse(serializedValue);

        // When
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);

        // Then
        Assert.Equal("https://example.org", apiManifest.ApiDependencies["graph"].ApiDescriptionUrl);
    }
    [Fact]
    public void ParseApiDescriptionVersionField()
    {
        // Given
        var serializedValue = """
        {
        "applicationName": "application-name",
            "apiDependencies": {
                "graph": {
                    "apiDescriptionVersion": "v1.0",
                    "requests": [
                        {
                            "method": "GET",
                            "uriTemplate": "/directoryObjects/{directoryObject-id}"
                        }
                    ]
                }
            }
        }
        """;
        var doc = JsonDocument.Parse(serializedValue);

        // When
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);

        // Then
        Assert.Equal("v1.0", apiManifest.ApiDependencies["graph"].ApiDescriptionVersion);
    }
    [Fact]
    public void ParsesApiDeploymentBaseUrl()
    {
        // Given
        var serializedValue = """
        {
        "applicationName": "application-name",
            "apiDependencies": {
                "graph": {
                    "apiDeploymentBaseUrl": "https://example.org/",
                    "requests": [
                        {
                            "method": "GET",
                            "uriTemplate": "/directoryObjects/{directoryObject-id}"
                        }
                    ]
                }
            }
        }
        """;
        var doc = JsonDocument.Parse(serializedValue);

        // When
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);

        // Then
        Assert.Equal("https://example.org/", apiManifest.ApiDependencies["graph"].ApiDeploymentBaseUrl);
    }

    [Fact]
    public void ParsesApiDeploymentBaseUrlWithDifferentCasing()
    {
        // Given
        var serializedValue = """
        {
        "applicationName": "application-name",
            "apiDependencies": {
                "graph": {
                    "APIDeploymentBaseUrl": "https://example.org/",
                    "requests": [
                        {
                            "method": "GET",
                            "uriTemplate": "/directoryObjects/{directoryObject-id}"
                        }
                    ]
                }
            }
        }
        """;
        var doc = JsonDocument.Parse(serializedValue);

        // When
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);

        // Then
        Assert.Equal("https://example.org/", apiManifest.ApiDependencies["graph"].ApiDeploymentBaseUrl);
    }

    [Fact]
    public void DoesNotFailOnExtraneousProperty()
    {
        // Given
        var serializedValue = """
        {
        "applicationName": "application-name",
            "apiDependencies": {
                "graph": {
                    "APIDeploymentBaseUrl": "https://example.org/",
                    "APISensitivity": "low",
                    "requests": [
                        {
                            "method": "GET",
                            "uriTemplate": "/directoryObjects/{directoryObject-id}"
                        }
                    ]
                }
            }
        }
        """;
        var doc = JsonDocument.Parse(serializedValue);

        // When
        var apiManifest = ApiManifestDocument.Load(doc.RootElement);

        // Then
        Assert.Equal("https://example.org/", apiManifest.ApiDependencies["graph"].ApiDeploymentBaseUrl);
    }

    private static ApiManifestDocument CreateDocument()
    {
        return new ApiManifestDocument("application-name")
        {
            Publisher = new("Microsoft", "example@example.org"),
            ApiDependencies = new() {
                { "example", new()
                {
                    ApiDescriptionUrl = "https://example.org",
                    ApiDeploymentBaseUrl = "https://example.org/v1.0/",
                    AuthorizationRequirements = new()
                    {
                        ClientIdentifier = "1234",
                        Access = new List<AccessRequest>() {
                            new() { Type = "application", Content = new JsonObject() {
                                { "scopes", new JsonArray() { "User.Read.All" } } }
                            },
                            new() { Type = "delegated", Content = new JsonObject() {
                                { "scopes", new JsonArray() { "User.Read", "Mail.Read" } } }
                            }
                        }
                    },
                    Requests = new List<RequestInfo>() {
                        new() { Method = "GET", UriTemplate = "/api/v1/endpoint" },
                        new() { Method = "POST", UriTemplate = "/api/v1/endpoint" }
                    },
                    Extensions = new()
                    {
                        { "example-api-dependency-extension", "dependency-extension-value" }
                    }
                }
                }
            },
            Extensions = new()
            {
                { "api-manifest-extension", "manifest-extension-value" }
            }
        };
    }
}

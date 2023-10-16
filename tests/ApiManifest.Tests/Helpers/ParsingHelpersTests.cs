// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.Tests.Helpers
{
    public class ParsingHelpersTests
    {
        private readonly string exampleKeyValuePair;
        private readonly JsonDocument exampleJsonDoc;

        public ParsingHelpersTests()
        {
            exampleKeyValuePair = "foo=bar;foo1=bar1;foo2=bar3";
            var json = """
            {
                "foo": ["a", "b", "c"],
                "bar": {
                    "foo1": "bar1"
                }
            }
            """;
            exampleJsonDoc = JsonDocument.Parse(json);
        }

        [Fact]
        public void GetList()
        {
            var listOfString = ParsingHelpers.GetList(exampleJsonDoc.RootElement.GetProperty("foo"),
                (v) =>
                {
                    var value = v.GetString();
                    return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
                });
            Assert.Equal(3, listOfString.Count);
            Assert.Equal(listOfString, new List<string> { "a", "b", "c" });
        }

        [Fact]
        public void GetMap()
        {
            var map = ParsingHelpers.GetMap<string>(exampleJsonDoc.RootElement.GetProperty("bar"),
                (v) =>
                {
                    var value = v.GetString();
                    return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
                });
            Assert.Equal(1, map?.Count);
            Assert.Equal("bar1", map?["foo1"]);
        }

        [Fact]
        public void GetOrderedMap()
        {
            var orderedMap = ParsingHelpers.GetOrderedMap<string>(exampleJsonDoc.RootElement.GetProperty("bar"),
                (v) =>
                {
                    var value = v.GetString();
                    return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
                });
            Assert.Equal(1, orderedMap?.Count);
            Assert.Equal("bar1", orderedMap?["foo1"]);
        }

        [Fact]
        public void GetMapOfString()
        {
            var mapOfString = ParsingHelpers.GetMapOfString(exampleJsonDoc.RootElement.GetProperty("bar"));
            Assert.Equal(1, mapOfString?.Count);
            Assert.Equal("bar1", mapOfString?["foo1"]);
        }

        [Fact]
        public void GetListOfString()
        {
            var listOfString = ParsingHelpers.GetListOfString(exampleJsonDoc.RootElement.GetProperty("foo"));
            Assert.Equal(3, listOfString.Count);
            Assert.Equal(listOfString, new List<string> { "a", "b", "c" });
        }

        [Fact]
        public void GetHashSetOfString()
        {
            var hashSetOfString = ParsingHelpers.GetHashSetOfString(exampleJsonDoc.RootElement.GetProperty("foo"));
            Assert.Equal(3, hashSetOfString.Count);
            Assert.Equal(hashSetOfString, new HashSet<string> { "a", "b", "c" });
        }

        [Fact]
        public void GetOrderedHashSetOfString()
        {
            var hashSetOfString = ParsingHelpers.GetOrderedHashSetOfString(exampleJsonDoc.RootElement.GetProperty("foo"));
            Assert.Equal(3, hashSetOfString.Count);
            Assert.Equal(hashSetOfString, new SortedSet<string> { "a", "b", "c" });
        }

        [Fact]
        public void ParseProperties()
        {
            var kvPairs = ParsingHelpers.ParseProperties(exampleKeyValuePair);
            Assert.Equal(3, kvPairs.Count);
            Assert.Equal("bar", kvPairs["foo"]);
            Assert.Equal("bar1", kvPairs["foo1"]);
            Assert.Equal("bar3", kvPairs["foo2"]);
        }

        [Fact]
        public void ParseKeyValuePair()
        {
            var kvPairs = ParsingHelpers.ParseKey(exampleKeyValuePair);
            Assert.Equal(3, kvPairs.Count());
        }

        [Fact]
        public async Task ParseOpenApiAsync()
        {
            var testOpenApiFilePath = Path.Combine(".", "TestFiles", "testOpenApi.yaml");
            var mockHandler = MockHttpResponse(File.ReadAllText(testOpenApiFilePath));

            var openApiUri = new Uri("https://contoso.com/openapi.yaml");
            var stream = await ParsingHelpers.GetStreamAsync(openApiUri, mockHandler, CancellationToken.None);
            var results = await ParsingHelpers.ParseOpenApiAsync(stream, openApiUri, false, CancellationToken.None);
            Assert.Empty(results.OpenApiDiagnostic.Errors);
            Assert.NotNull(results.OpenApiDocument);
        }

        [Fact]
        public void ParseOpenApiWithWrongOpenApiUrl()
        {
            var openApiUri = new Uri("https://contoso.com/NotValid.yaml");
            _ = Assert.ThrowsAsync<InvalidOperationException>(async () => await ParsingHelpers.ParseOpenApiAsync(openApiUri, false, CancellationToken.None));
        }

        [Fact]
        public void ParseOpenApiWithOpenApiUrlWithAnInvalidSchema()
        {
            var openApiUri = new Uri("xyx://contoso.com/openapi.yaml");
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await ParsingHelpers.ParseOpenApiAsync(openApiUri, false, CancellationToken.None));
        }

        private static DelegatingHandler MockHttpResponse(string responseContent)
        {
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseContent) };
            mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var mockHandler = new Mock<DelegatingHandler>();
            _ = mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(mockResponse));
            return mockHandler.Object;
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
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
            using var stream = File.OpenRead(testOpenApiFilePath);
            var results = await ParsingHelpers.ParseOpenApiAsync(stream, new Uri("https://contoso.com/openapi.yaml"), false, CancellationToken.None);
            Assert.Empty(results.OpenApiDiagnostic.Errors);
            Assert.NotNull(results.OpenApiDocument);
        }

        [Fact]
        public async Task ParseOpenApiWithWrongOpenApiUrl()
        {
            var openApiUri = new Uri("https://1CED4309-EFBF-41A8-9E8F-8BBA0CB3EEE5.com/NotValid.yaml");
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await ParsingHelpers.ParseOpenApiAsync(openApiUri, false, CancellationToken.None));
        }

        [Fact]
        public async Task ParseOpenApiWithOpenApiUrlWithAnInvalidSchema()
        {
            var openApiUri = new Uri("xyx://contoso.com/openapi.yaml");
            await Assert.ThrowsAsync<ArgumentException>(async () => await ParsingHelpers.ParseOpenApiAsync(openApiUri, false, CancellationToken.None));
        }
    }
}

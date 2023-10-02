// Write tests for OpenAIPluginManifest

using Microsoft.OpenApi.ApiManifest.OpenAI;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.Tests.OpenAI
{
    public class OpenAIPluginManifestTests
    {
        [Fact]
        public void LoadOpenAIPluginManifest()
        {
            var json = @"{
                ""schema_version"": ""1.0.0"",
                ""name_for_human"": ""OpenAI GPT-3"",
                ""name_for_model"": ""openai-gpt3"",
                ""description_for_human"": ""OpenAI GPT-3 is a language model that generates text based on prompts."" ,
                ""description_for_model"": ""OpenAI GPT-3 is a language model that generates text based on prompts."",
                ""auth"": {
                    ""type"": ""none""
                },
                ""api"": {
                    ""type"": ""openapi"",
                    ""url"": ""https://api.openai.com/v1""
                },
                ""logo_url"": ""https://avatars.githubusercontent.com/foo"",
                ""contact_email"": ""joe@demo.com""
            }";

            var doc = JsonDocument.Parse(json);
            var manifest = OpenAIPluginManifest.Load(doc.RootElement);

            Assert.Equal("1.0.0", manifest.SchemaVersion);
            Assert.Equal("OpenAI GPT-3", manifest.NameForHuman);
            Assert.Equal("openai-gpt3", manifest.NameForModel);
            Assert.Equal("OpenAI GPT-3 is a language model that generates text based on prompts.", manifest.DescriptionForHuman);
            Assert.Equal("OpenAI GPT-3 is a language model that generates text based on prompts.", manifest.DescriptionForModel);
            Assert.Equal("none", manifest.Auth?.Type);
            Assert.Equal("openapi", manifest.Api?.Type);
            Assert.Equal("https://api.openai.com/v1", manifest.Api?.Url);
            Assert.Equal("https://avatars.githubusercontent.com/foo", manifest.LogoUrl);
            Assert.Equal("joe@demo.com", manifest.ContactEmail);
        }

        // Create minimal OpenAIPluginManifest
        [Fact]
        public void WriteOpenAIPluginManifest()
        {
            var manifest = new OpenAIPluginManifest
            {
                SchemaVersion = "1.0.0",
                NameForHuman = "OpenAI GPT-3",
                NameForModel = "openai-gpt3",
                DescriptionForHuman = "OpenAI GPT-3 is a language model that generates text based on prompts.",
                DescriptionForModel = "OpenAI GPT-3 is a language model that generates text based on prompts.",
                Auth = new ManifestNoAuth(),
                Api = new Api
                {
                    Type = "openapi",
                    Url = "https://api.openai.com/v1",
                    IsUserAuthenticated = false
                },
                LogoUrl = "https://avatars.githubusercontent.com/bar",
                ContactEmail = "joe@test.com"
            };

            // serialize using the Write method
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            manifest.Write(writer);
            writer.Flush();
            stream.Position = 0;
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            Assert.Equal(@"{
    ""schema_version"": ""1.0.0"",
    ""name_for_human"": ""OpenAI GPT-3"",
    ""name_for_model"": ""openai-gpt3"",
    ""description_for_human"": ""OpenAI GPT-3 is a language model that generates text based on prompts."",
    ""description_for_model"": ""OpenAI GPT-3 is a language model that generates text based on prompts."",
    ""auth"": {
        ""type"": ""none""
    },
    ""api"": {
        ""type"": ""openapi"",
        ""url"": ""https://api.openai.com/v1"",
        ""is_user_authenticated"": false
    },
    ""logo_url"": ""https://avatars.githubusercontent.com/bar"",
    ""contact_email"": ""joe@test.com""
}", json, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void WriteOAuthTest()
        {
            var manifest = new OpenAIPluginManifest
            {
                SchemaVersion = "1.0.0",
                NameForHuman = "TestOAuth",
                NameForModel = "TestOAuthModel",
                DescriptionForHuman = "SomeHumanDescription",
                DescriptionForModel = "SomeModelDescription",
                Auth = new ManifestOAuthAuth
                {
                    AuthorizationUrl = "https://api.openai.com/oauth/authorize",
                },
                Api = new Api
                {
                    Type = "openapi",
                    Url = "https://api.openai.com/v1",
                    IsUserAuthenticated = false
                },
                LogoUrl = "https://avatars.githubusercontent.com/bar",
                ContactEmail = "joe@test.com"
            };

            // serialize using the Write method
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            manifest.Write(writer);
            writer.Flush();
            stream.Position = 0;
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            Assert.Equal(@"{
    ""schema_version"": ""1.0.0"",
    ""name_for_human"": ""TestOAuth"",
    ""name_for_model"": ""TestOAuthModel"",
    ""description_for_human"": ""SomeHumanDescription"",
    ""description_for_model"": ""SomeModelDescription"",
    ""auth"": {
        ""type"": ""oauth"",
        ""authorization_url"": ""https://api.openai.com/oauth/authorize""
    },
    ""api"": {
        ""type"": ""openapi"",
        ""url"": ""https://api.openai.com/v1"",
        ""is_user_authenticated"": false
    },
    ""logo_url"": ""https://avatars.githubusercontent.com/bar"",
    ""contact_email"": ""joe@test.com""
}", json, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

    }


}
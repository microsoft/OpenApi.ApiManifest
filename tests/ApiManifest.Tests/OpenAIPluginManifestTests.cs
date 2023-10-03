// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.Tests;

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
    public void LoadOpenAIPluginManifestWithOAuth()
    {
        var json = @"{
    ""schema_version"": ""1.0.0"",
    ""name_for_human"": ""TestOAuth"",
    ""name_for_model"": ""TestOAuthModel"",
    ""description_for_human"": ""SomeHumanDescription"",
    ""description_for_model"": ""SomeModelDescription"",
    ""auth"": {
        ""type"": ""oauth"",
        ""authorization_url"": ""https://api.openai.com/oauth/authorize"",
        ""authorization_content_type"": ""application/json"",
        ""client_url"": ""https://api.openai.com/oauth/token"",
        ""scope"": ""all:all"",
        ""verification_tokens"": {
            ""openai"": ""dummy_verification_token""
        }
    },
    ""api"": {
        ""type"": ""openapi"",
        ""url"": ""https://api.openai.com/v1"",
        ""is_user_authenticated"": false
    },
    ""logo_url"": ""https://avatars.githubusercontent.com/bar"",
    ""contact_email"": ""joe@test.com""
}";

        var doc = JsonDocument.Parse(json);
        var manifest = OpenAIPluginManifest.Load(doc.RootElement);

        Assert.Equal("1.0.0", manifest.SchemaVersion);
        Assert.Equal("TestOAuth", manifest.NameForHuman);
        Assert.Equal("TestOAuthModel", manifest.NameForModel);
        Assert.Equal("SomeHumanDescription", manifest.DescriptionForHuman);
        Assert.Equal("SomeModelDescription", manifest.DescriptionForModel);
        Assert.Equal("oauth", manifest.Auth?.Type);
        Assert.Equal("https://api.openai.com/oauth/authorize", ((ManifestOAuthAuth?)manifest.Auth)?.AuthorizationUrl);
        Assert.Equal("application/json", ((ManifestOAuthAuth?)manifest.Auth)?.AuthorizationContentType);
        Assert.Equal("https://api.openai.com/oauth/token", ((ManifestOAuthAuth?)manifest.Auth)?.ClientUrl);
        Assert.Equal("all:all", ((ManifestOAuthAuth?)manifest.Auth)?.Scope);
        Assert.Equal("dummy_verification_token", ((ManifestOAuthAuth?)manifest.Auth)?.VerificationTokens["OPENAI"]);
        Assert.Equal("openapi", manifest.Api?.Type);
        Assert.Equal("https://api.openai.com/v1", manifest.Api?.Url);
        Assert.Equal("https://avatars.githubusercontent.com/bar", manifest.LogoUrl);
        Assert.Equal("joe@test.com", manifest.ContactEmail);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithOAuth()
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
                AuthorizationContentType = "application/json",
                ClientUrl = "https://api.openai.com/oauth/token",
                Scope = "all:all",
                VerificationTokens = new OpenAI.Auth.VerificationTokens
                {
                    { "openai", "dummy_verification_token" }
                }
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
        ""client_url"": ""https://api.openai.com/oauth/token"",
        ""scope"": ""all:all"",
        ""authorization_url"": ""https://api.openai.com/oauth/authorize"",
        ""authorization_content_type"": ""application/json"",
        ""verification_tokens"": {
            ""openai"": ""dummy_verification_token""
        }
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
    public void LoadOpenAIPluginManifestWithUserHttp()
    {
        var json = @"{
    ""schema_version"": ""1.0.0"",
    ""name_for_human"": ""TestOAuth"",
    ""name_for_model"": ""TestOAuthModel"",
    ""description_for_human"": ""SomeHumanDescription"",
    ""description_for_model"": ""SomeModelDescription"",
    ""auth"": {
        ""type"": ""user_http"",
        ""authorization_type"": ""bearer""
    },
    ""api"": {
        ""type"": ""openapi"",
        ""url"": ""https://api.openai.com/v1"",
        ""is_user_authenticated"": false
    },
    ""logo_url"": ""https://avatars.githubusercontent.com/bar"",
    ""contact_email"": ""joe@test.com""
}";

        var doc = JsonDocument.Parse(json);
        var manifest = OpenAIPluginManifest.Load(doc.RootElement);

        Assert.Equal("1.0.0", manifest.SchemaVersion);
        Assert.Equal("TestOAuth", manifest.NameForHuman);
        Assert.Equal("TestOAuthModel", manifest.NameForModel);
        Assert.Equal("SomeHumanDescription", manifest.DescriptionForHuman);
        Assert.Equal("SomeModelDescription", manifest.DescriptionForModel);
        Assert.Equal("user_http", manifest.Auth?.Type);
        Assert.Equal("bearer", ((ManifestUserHttpAuth?)manifest.Auth)?.AuthorizationType);
        Assert.Equal("openapi", manifest.Api?.Type);
        Assert.Equal("https://api.openai.com/v1", manifest.Api?.Url);
        Assert.Equal("https://avatars.githubusercontent.com/bar", manifest.LogoUrl);
        Assert.Equal("joe@test.com", manifest.ContactEmail);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithUserHttp()
    {
        var manifest = new OpenAIPluginManifest
        {
            SchemaVersion = "1.0.0",
            NameForHuman = "TestOAuth",
            NameForModel = "TestOAuthModel",
            DescriptionForHuman = "SomeHumanDescription",
            DescriptionForModel = "SomeModelDescription",
            Auth = new ManifestUserHttpAuth("bearer"),
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
        ""type"": ""user_http"",
        ""authorization_type"": ""bearer""
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
    public void LoadOpenAIPluginManifestWithServiceHttp()
    {
        var json = @"{
    ""schema_version"": ""1.0.0"",
    ""name_for_human"": ""TestOAuth"",
    ""name_for_model"": ""TestOAuthModel"",
    ""description_for_human"": ""SomeHumanDescription"",
    ""description_for_model"": ""SomeModelDescription"",
    ""auth"": {
        ""type"": ""service_http"",
        ""authorization_type"": ""bearer"",
        ""verification_tokens"": {
            ""openai"": ""dummy_verification_token""
        }
    },
    ""api"": {
        ""type"": ""openapi"",
        ""url"": ""https://api.openai.com/v1"",
        ""is_user_authenticated"": false
    },
    ""logo_url"": ""https://avatars.githubusercontent.com/bar"",
    ""contact_email"": ""joe@test.com""
}";

        var doc = JsonDocument.Parse(json);
        var manifest = OpenAIPluginManifest.Load(doc.RootElement);

        Assert.Equal("1.0.0", manifest.SchemaVersion);
        Assert.Equal("TestOAuth", manifest.NameForHuman);
        Assert.Equal("TestOAuthModel", manifest.NameForModel);
        Assert.Equal("SomeHumanDescription", manifest.DescriptionForHuman);
        Assert.Equal("SomeModelDescription", manifest.DescriptionForModel);
        Assert.Equal("service_http", manifest.Auth?.Type);
        Assert.Equal("bearer", ((ManifestServiceHttpAuth?)manifest.Auth)?.AuthorizationType);
        Assert.Equal("dummy_verification_token", ((ManifestServiceHttpAuth?)manifest.Auth)?.VerificationTokens["OPENAI"]);
        Assert.Equal("openapi", manifest.Api?.Type);
        Assert.Equal("https://api.openai.com/v1", manifest.Api?.Url);
        Assert.Equal("https://avatars.githubusercontent.com/bar", manifest.LogoUrl);
        Assert.Equal("joe@test.com", manifest.ContactEmail);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithServiceHttp()
    {
        var manifest = new OpenAIPluginManifest
        {
            SchemaVersion = "1.0.0",
            NameForHuman = "TestOAuth",
            NameForModel = "TestOAuthModel",
            DescriptionForHuman = "SomeHumanDescription",
            DescriptionForModel = "SomeModelDescription",
            Auth = new ManifestServiceHttpAuth(new OpenAI.Auth.VerificationTokens
            {
                { "openai", "dummy_verification_token" }
            }),
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
        ""type"": ""service_http"",
        ""authorization_type"": ""bearer"",
        ""verification_tokens"": {
            ""openai"": ""dummy_verification_token""
        }
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

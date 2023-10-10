// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.OpenAI;
using Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.Tests;

public class OpenAIPluginManifestTests
{
    // With no auth.
    [Fact]
    public void LoadOpenAIPluginManifestWithNoAuth()
    {
        var json = """
        {
            "schema_version": "1.0.0",
            "name_for_human": "OpenAI GPT-3",
            "name_for_model": "openai-gpt3",
            "description_for_human": "OpenAI GPT-3 is a language model that generates text based on prompts." ,
            "description_for_model": "OpenAI GPT-3 is a language model that generates text based on prompts.",
            "auth": {
                "type": "none"
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1"
            },
            "logo_url": "https://avatars.githubusercontent.com/foo",
            "legal_info_url": "https://legalinfo.foobar.com",
            "contact_email": "joe@demo.com"
        }
        """;

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
        Assert.Equal("https://legalinfo.foobar.com", manifest.LegalInfoUrl);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithNoAuth()
    {
        var manifest = CreateManifestPlugIn();

        // serialize using the Write method
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        manifest.Write(writer);
        writer.Flush();
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        Assert.Equal("""
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "none"
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """, json, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

    // With no OAuth.
    [Fact]
    public void LoadOpenAIPluginManifestWithOAuth()
    {
        var json = """
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "oauth",
                "authorization_url": "https://api.openai.com/oauth/authorize",
                "authorization_content_type": "application/json",
                "client_url": "https://api.openai.com/oauth/token",
                "scope": "all:all",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "legal_info_url": "https://legalinfo.foobar.com",
            "contact_email": "joe@test.com"
        }
        """;

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
        Assert.Equal("https://legalinfo.foobar.com", manifest.LegalInfoUrl);
        Assert.Equal("joe@test.com", manifest.ContactEmail);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithOAuth()
    {
        var manifest = CreateManifestPlugIn();
        manifest.Auth = new ManifestOAuthAuth
        {
            AuthorizationUrl = "https://api.openai.com/oauth/authorize",
            AuthorizationContentType = "application/json",
            ClientUrl = "https://api.openai.com/oauth/token",
            Scope = "all:all",
            VerificationTokens = new VerificationTokens
                {
                    { "openai", "dummy_verification_token" }
                }
        };

        // serialize using the Write method
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        manifest.Write(writer);
        writer.Flush();
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        Assert.Equal("""
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "oauth",
                "client_url": "https://api.openai.com/oauth/token",
                "scope": "all:all",
                "authorization_url": "https://api.openai.com/oauth/authorize",
                "authorization_content_type": "application/json",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """, json, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

    // With user HTTP.
    [Fact]
    public void LoadOpenAIPluginManifestWithUserHttp()
    {
        var json = """
        {
        "schema_version": "1.0.0",
        "name_for_human": "TestOAuth",
        "name_for_model": "TestOAuthModel",
        "description_for_human": "SomeHumanDescription",
        "description_for_model": "SomeModelDescription",
        "auth": {
            "type": "user_http",
            "authorization_type": "bearer"
        },
        "api": {
            "type": "openapi",
            "url": "https://api.openai.com/v1",
            "is_user_authenticated": false
        },
        "logo_url": "https://avatars.githubusercontent.com/bar",
        "legal_info_url": "https://legalinfo.foobar.com",
        "contact_email": "joe@test.com"
        }
        """;

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
        Assert.Equal("https://legalinfo.foobar.com", manifest.LegalInfoUrl);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithUserHttp()
    {
        var manifest = CreateManifestPlugIn();
        manifest.Auth = new ManifestUserHttpAuth("bearer");

        // serialize using the Write method
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        manifest.Write(writer);
        writer.Flush();
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        Assert.Equal("""
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "user_http",
                "authorization_type": "bearer"
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """, json, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

    // With service HTTP.
    [Fact]
    public void LoadOpenAIPluginManifestWithServiceHttp()
    {
        var json = """
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "service_http",
                "authorization_type": "bearer",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """;

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
        Assert.Equal("https://legalinfo.foobar.com", manifest.LegalInfoUrl);
    }

    [Fact]
    public void WriteOpenAIPluginManifestWithServiceHttp()
    {
        var manifest = CreateManifestPlugIn();
        manifest.Auth = new ManifestServiceHttpAuth(new VerificationTokens
        {
            { "openai", "dummy_verification_token" }
        });

        // serialize using the Write method
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        manifest.Write(writer);
        writer.Flush();
        stream.Position = 0;
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        Assert.Equal("""
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "service_http",
                "authorization_type": "bearer",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """, json, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

    [Theory]
    [InlineData("foo")]
    [InlineData("foo@")]
    [InlineData("foo@@bar.com")]
    [InlineData("foo @bar.com")]
    public void WriteOpenAIPluginManifestWithInvalidContactEmail(string email)
    {
        var manifest = CreateManifestPlugIn();
        manifest.ContactEmail = email;

        // serialize using the Write method
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        _ = Assert.Throws<ArgumentException>(() =>
        {
            manifest.Write(writer);
        });
    }

    [Fact]
    public void LoadOpenAIPluginManifestWithInvalidAuthType()
    {
        var json = """
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "NOT_VALID_service_http",
                "authorization_type": "bearer",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """;

        var doc = JsonDocument.Parse(json);
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = OpenAIPluginManifest.Load(doc.RootElement);
        });
        Assert.Equal("Unknown auth type: not_valid_service_http (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void LoadOpenAIPluginManifestWithIncompleteManifest()
    {
        var json = """
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "service_http",
                "authorization_type": "bearer",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "url": "https://api.openai.com/v1",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """;

        var doc = JsonDocument.Parse(json);
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = OpenAIPluginManifest.Load(doc.RootElement);
        });
        Assert.Equal("'NameForModel' is a required property of 'OpenAIPluginManifest'. (Parameter 'NameForModel')", exception.Message);
    }

    [Fact]
    public void LoadOpenAIPluginManifestWithNoApiUrl()
    {
        var json = """
        {
            "schema_version": "1.0.0",
            "name_for_human": "TestOAuth",
            "name_for_model": "TestOAuthModel",
            "description_for_human": "SomeHumanDescription",
            "description_for_model": "SomeModelDescription",
            "auth": {
                "type": "service_http",
                "authorization_type": "bearer",
                "verification_tokens": {
                    "openai": "dummy_verification_token"
                }
            },
            "api": {
                "type": "openapi",
                "is_user_authenticated": false
            },
            "logo_url": "https://avatars.githubusercontent.com/bar",
            "contact_email": "joe@test.com",
            "legal_info_url": "https://legalinfo.foobar.com"
        }
        """;

        var doc = JsonDocument.Parse(json);
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = OpenAIPluginManifest.Load(doc.RootElement);
        });
        Assert.Equal("'Url' is a required property of 'Api'. (Parameter 'Url')", exception.Message);
    }

    private static OpenAIPluginManifest CreateManifestPlugIn()
    {
        var manifest = OpenApiPluginFactory.CreateOpenAIPluginManifest(
            schemaVersion: "1.0.0",
            nameForHuman: "TestOAuth",
            nameForModel: "TestOAuthModel",
            logoUrl: "https://avatars.githubusercontent.com/bar",
            legalInfoUrl: "https://legalinfo.foobar.com",
            contactEmail: "joe@test.com");
        manifest.DescriptionForHuman = "SomeHumanDescription";
        manifest.DescriptionForModel = "SomeModelDescription";
        manifest.Auth = new ManifestNoAuth();
        manifest.Api = new Api("openapi", "https://api.openai.com/v1")
        {
            IsUserAuthenticated = false
        };
        return manifest;
    }
}

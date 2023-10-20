using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class ApiManifestDocument
{
    public Publisher? Publisher { get; set; }
    public string? ApplicationName { get; set; }
    public ApiDependencies ApiDependencies { get; set; } = new ApiDependencies();
    public Extensions Extensions { get; set; } = new Extensions();

    private const string PublisherProperty = "publisher";
    private const string ApplicationNameProperty = "applicationName";
    private const string ApiDependenciesProperty = "apiDependencies";
    private const string ExtensionsProperty = "extensions";

    public ApiManifestDocument(string applicationName)
    {
        ApplicationName = applicationName;
        Validate();
    }

    public ApiManifestDocument(JsonElement value)
    {
        ParsingHelpers.ParseMap(value, this, handlers);
        Validate();
    }

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        Validate();
        writer.WriteStartObject();
        writer.WriteString(ApplicationNameProperty, ApplicationName);
        if (Publisher != null)
        {
            writer.WritePropertyName(PublisherProperty);
            Publisher.Write(writer);
        }
        if (ApiDependencies.Any())
        {
            writer.WritePropertyName(ApiDependenciesProperty);
            writer.WriteStartObject();
            foreach (var apiDependency in ApiDependencies)
            {
                writer.WritePropertyName(apiDependency.Key);
                apiDependency.Value.Write(writer);
            }
            writer.WriteEndObject();
        }
        if (Extensions.Any())
        {
            writer.WritePropertyName(ExtensionsProperty);
            Extensions.Write(writer);
        }
        writer.WriteEndObject();
    }

    // Load method
    public static ApiManifestDocument Load(JsonElement value)
    {
        return new ApiManifestDocument(value);
    }

    internal void Validate()
    {
        ValidationHelpers.ValidateNullOrWhitespace(nameof(ApplicationName), ApplicationName, nameof(ApiManifestDocument));
        Publisher?.Validate();
    }

    // Create fixed field map for ApiManifest
    private static readonly FixedFieldMap<ApiManifestDocument> handlers = new()
    {
        { ApplicationNameProperty, (o,v) => {o.ApplicationName = v.GetString(); } },
        { PublisherProperty, (o,v) => {o.Publisher = Publisher.Load(v);  } },
        { ApiDependenciesProperty, (o,v) => {o.ApiDependencies = new ApiDependencies(ParsingHelpers.GetMap(v, ApiDependency.Load));  } },
        { ExtensionsProperty, (o,v) => {o.Extensions = Extensions.Load(v);  } }
    };
}

public class ApiDependencies : Dictionary<string, ApiDependency>
{
    public ApiDependencies(IDictionary<string, ApiDependency> dictionary) : base(dictionary, StringComparer.OrdinalIgnoreCase) { }
    public ApiDependencies() : base(StringComparer.OrdinalIgnoreCase) { }
}
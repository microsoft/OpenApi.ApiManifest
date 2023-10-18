using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class Publisher
{
    public string? Name { get; set; }
    public string? ContactEmail { get; set; }

    private const string NameProperty = "name";
    private const string ContactEmailProperty = "contactEmail";

    public Publisher(string name, string contactEmail)
    {
        Name = name;
        ContactEmail = contactEmail;
        Validate();
    }
    private Publisher(JsonElement value)
    {
        ParsingHelpers.ParseMap(value, this, handlers);
        Validate();
    }

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        Validate();
        writer.WriteStartObject();
        writer.WriteString(NameProperty, Name);
        writer.WriteString(ContactEmailProperty, ContactEmail);
        writer.WriteEndObject();
    }

    // Load method
    internal static Publisher Load(JsonElement value)
    {
        return new Publisher(value);
    }

    internal void Validate()
    {
        ValidationHelpers.ValidateNullOrWhitespace(nameof(Name), Name, nameof(Publisher));
        ValidationHelpers.ValidateEmail(nameof(ContactEmail), ContactEmail, nameof(Publisher));
    }

    private static readonly FixedFieldMap<Publisher> handlers = new()
    {
        { NameProperty, (o,v) => {o.Name = v.GetString(); } },
        { ContactEmailProperty, (o,v) => {o.ContactEmail = v.GetString();  } },
    };
}

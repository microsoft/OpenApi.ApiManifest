using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class Publisher
{
    public string? Name { get; set; }
    public string? ContactEmail { get; set; }
    private const string NameProperty = "name";
    private const string ContactEmailProperty = "contactEmail";

    public Publisher(string contactEmail)
    {
        if (string.IsNullOrWhiteSpace(contactEmail)) throw new ArgumentNullException("Contact email is a required property of Publisher.");
        ContactEmail = contactEmail;
    }
    private Publisher(JsonElement value)
    {
        ParsingHelpers.ParseMap(value, this, handlers);
        // Validate that Name and ContactEmail are not null
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentNullException("Name is a required property of publisher.");
        if (string.IsNullOrWhiteSpace(ContactEmail)) throw new ArgumentNullException("Contact email is a required property of Publisher.");
    }

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!string.IsNullOrWhiteSpace(Name)) writer.WriteString(NameProperty, Name);
        if (!string.IsNullOrWhiteSpace(ContactEmail)) writer.WriteString(ContactEmailProperty, ContactEmail);

        writer.WriteEndObject();
    }
    // Load method
    internal static Publisher Load(JsonElement value)
    {
        return new Publisher(value);
    }

    private static readonly FixedFieldMap<Publisher> handlers = new()
    {
        { NameProperty, (o,v) => {o.Name = v.GetString(); } },
        { ContactEmailProperty, (o,v) => {o.ContactEmail = v.GetString();  } },
    };
}

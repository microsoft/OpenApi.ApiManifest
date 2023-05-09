using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest;

public class Publisher
{
    public string? Name { get; set; }
    public string? ContactEmail { get; set; }

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        if (!String.IsNullOrWhiteSpace(Name)) writer.WriteString("name", Name);
        if (!String.IsNullOrWhiteSpace(ContactEmail)) writer.WriteString("contactEmail", ContactEmail);

        writer.WriteEndObject();
    }
    // Load method
    internal static Publisher Load(JsonElement value)
    {
        var publisher = new Publisher();
        ParsingHelpers.ParseMap(value, publisher, handlers);
        // Validate that Name and ContactEmail are not null
        if (String.IsNullOrWhiteSpace(publisher.Name)) throw new ArgumentNullException("Name is a required property of publisher.");
        if (String.IsNullOrWhiteSpace(publisher.ContactEmail)) throw new ArgumentNullException("Contact email is a required property of Publisher.");
        return publisher;
    }

    private static FixedFieldMap<Publisher> handlers = new()
    {
        { "name", (o,v) => {o.Name = v.GetString(); } },
        { "contactEmail", (o,v) => {o.ContactEmail = v.GetString();  } },
    };
}

using System.Text.Json;
using System.Text.RegularExpressions;

namespace Microsoft.OpenApi.ApiManifest;

public class Publisher
{
    public string? Name { get; set; }
    public string? ContactEmail { get; set; }

    private const string NameProperty = "name";
    private const string ContactEmailProperty = "contactEmail";

    private static readonly Regex s_emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled, Constants.DefaultRegexTimeout);

    public Publisher(string name, string contactEmail)
    {
        Validate(name, contactEmail);

        Name = name;
        ContactEmail = contactEmail;
    }
    private Publisher(JsonElement value)
    {
        ParsingHelpers.ParseMap(value, this, handlers);
        Validate(Name, ContactEmail);
    }

    // Write method
    public void Write(Utf8JsonWriter writer)
    {
        Validate(Name, ContactEmail);

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

    private static void Validate(string? name, string? contactEmail)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name, String.Format(ErrorConstants.FieldIsRequired, "name", "publisher"));

        if (string.IsNullOrWhiteSpace(contactEmail))
            throw new ArgumentNullException(contactEmail, String.Format(ErrorConstants.FieldIsRequired, "contactEmail", "publisher"));

        if (!s_emailRegex.IsMatch(contactEmail))
            throw new ArgumentException(string.Format(ErrorConstants.FieldIsNotValid, "contactEmail"), contactEmail);
    }

    private static readonly FixedFieldMap<Publisher> handlers = new()
    {
        { NameProperty, (o,v) => {o.Name = v.GetString(); } },
        { ContactEmailProperty, (o,v) => {o.ContactEmail = v.GetString();  } },
    };
}

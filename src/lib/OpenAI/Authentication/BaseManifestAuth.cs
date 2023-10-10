// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

public abstract class BaseManifestAuth
{
    private const string TypeProperty = "type";
    private const string InstructionsProperty = "instructions";

    public string? Type { get; internal set; }
    public string? Instructions { get; set; }

    // Create handlers FixedFieldMap for BaseManifestAuth properties.
    private static readonly FixedFieldMap<BaseManifestAuth> handlers = new()
    {
        { TypeProperty, (o,v) => {o.Type = v.GetString();  } },
        { InstructionsProperty, (o,v) => {o.Instructions = v.GetString();  } }
    };

    /// <summary>
    /// Loads the common properties for all authentication types.
    /// </summary>
    /// <param name="value">The <see cref="JsonElement"/> to parse.</param>
    protected void LoadProperties(JsonElement value)
    {
        ParsingHelpers.ParseMap(value, this, handlers);
    }

    /// <summary>
    /// Write the common properties for all authentication types. This method does not write the opening and closing object tags.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to use.</param>
    protected void WriteProperties(Utf8JsonWriter writer)
    {
        writer.WriteString(TypeProperty, Type);
        if (!string.IsNullOrWhiteSpace(Instructions)) writer.WriteString(InstructionsProperty, Instructions);
    }

    public virtual void Write(Utf8JsonWriter writer) { }
}

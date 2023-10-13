
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.OpenAI.Authentication;

public class VerificationTokens : Dictionary<string, string>
{
    public VerificationTokens(IDictionary<string, string> dictionary) : base(dictionary, StringComparer.OrdinalIgnoreCase) { }
    public VerificationTokens() : base(StringComparer.OrdinalIgnoreCase) { }

    internal static VerificationTokens Load(JsonElement value)
    {
        return new VerificationTokens(ParsingHelpers.GetMapOfString(value));
    }

    public void Write(Utf8JsonWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStartObject();
        foreach (var verificationToken in this)
        {
            writer.WriteString(verificationToken.Key, verificationToken.Value);
        }
        writer.WriteEndObject();
    }
}

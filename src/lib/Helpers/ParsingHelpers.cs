// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.OpenApi.Readers;
using System.Diagnostics;
using System.Text.Json;

namespace Microsoft.OpenApi.ApiManifest.Helpers;

internal static class ParsingHelpers
{
    // The HttpClient will default to SslProtocol of none. This lets the OS pick an appropriate SSL protocol.
    private static readonly Lazy<HttpClient> s_httpClient = new(() => new HttpClient());


    internal static void ParseMap<T>(JsonElement node, T permissionsDocument, FixedFieldMap<T> handlers)
    {
        foreach (var element in node.EnumerateObject())
        {
            if (handlers.TryGetValue(element.Name, out var handler))
            {
                handler(permissionsDocument, element.Value);
            }
            else
            {
                // Logs the unknown property. We can switch to additional properties model in the future if need be.
                Debug.WriteLine($"Skipped {element.Name}. The property is unknown.");
            }
        }
    }

    internal static List<T> GetList<T>(JsonElement v, Func<JsonElement, T> load)
    {
        var list = new List<T>();
        foreach (var item in v.EnumerateArray())
        {
            list.Add(load(item));
        }
        return list;
    }

    internal static Dictionary<string, T> GetMap<T>(JsonElement v, Func<JsonElement, T> load)
    {
        var map = new Dictionary<string, T>();
        foreach (var item in v.EnumerateObject())
        {
            map.Add(item.Name, load(item.Value));
        }
        return map;
    }

    internal static SortedDictionary<string, T> GetOrderedMap<T>(JsonElement v, Func<JsonElement, T> load)
    {
        var map = new SortedDictionary<string, T>();
        foreach (var item in v.EnumerateObject())
        {
            map.Add(item.Name, load(item.Value));
        }
        return map;
    }

    internal static Dictionary<string, string> GetMapOfString(JsonElement v)
    {
        var map = new Dictionary<string, string>();
        foreach (var item in v.EnumerateObject())
        {
            var value = item.Value.GetString();
            map.Add(item.Name, string.IsNullOrWhiteSpace(value) ? string.Empty : value!);
        }
        return map;
    }

    internal static List<string> GetListOfString(JsonElement v)
    {
        var list = new List<string>();
        foreach (var item in v.EnumerateArray())
        {
            var value = item.GetString();
            if (value != null)
                list.Add(value);
        }
        return list;
    }

    internal static HashSet<string> GetHashSetOfString(JsonElement v)
    {
        var hashSet = new HashSet<string>();
        foreach (var item in v.EnumerateArray())
        {
            var value = item.GetString();
            if (value != null)
                _ = hashSet.Add(value);
        }
        return hashSet;
    }

    internal static SortedSet<string> GetOrderedHashSetOfString(JsonElement v)
    {
        var sortedSet = new SortedSet<string>();
        foreach (var item in v.EnumerateArray())
        {
            var value = item.GetString();
            if (value != null)
                _ = sortedSet.Add(value);
        }
        return sortedSet;
    }

    /// <summary>
    /// Parse properties.
    /// </summary>
    /// <param name="context">Name-value pair separated by ';'.</param>
    internal static Dictionary<string, string> ParseProperties(string context)
    {
        var properties = new Dictionary<string, string>();
        foreach (var pair in ParseKey(context))
        {
            properties.Add(pair.Key, pair.Value);
        }

        return properties;
    }

    /// <summary>
    /// Enumerate the key value pairs for the configuration key.
    /// </summary>
    /// <param name="key">Configuration key supplied in the setting.</param>
    /// *<returns>Key value pairs.</returns>
    internal static IEnumerable<KeyValuePair<string, string>> ParseKey(string key)
    {
        foreach (var pair in key.Split(';'))
        {
            if (string.IsNullOrEmpty(pair))
                continue;

            var index = pair.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            if (index == -1)
                throw new InvalidOperationException($"Unable to parse: {key}. Format is name1=value1;name2=value2;...");

            var keyValue = new KeyValuePair<string, string>(pair.Substring(0, index), pair.Substring(index + 1));
            yield return keyValue;
        }
    }

    internal static async Task<ReadResult> ParseOpenApiAsync(Uri openApiFileUri, bool inlineExternal, CancellationToken cancellationToken)
    {
        using var stream = await GetStreamAsync(openApiFileUri).ConfigureAwait(false);
        return await ParseOpenApiAsync(stream, openApiFileUri, inlineExternal, cancellationToken).ConfigureAwait(false);
    }

    internal static async Task<ReadResult> ParseOpenApiAsync(Stream stream, Uri openApiFileUri, bool inlineExternal, CancellationToken cancellationToken)
    {
        ReadResult result = await new OpenApiStreamReader(new OpenApiReaderSettings
        {
            LoadExternalRefs = inlineExternal,
            BaseUrl = openApiFileUri
        }
        ).ReadAsync(stream, cancellationToken).ConfigureAwait(false);

        return result;
    }

    internal static async Task<Stream> GetStreamAsync(Uri uri)
    {
        if (!uri.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"The input {uri} is not a valid url", nameof(uri));
        try
        {
            return await s_httpClient.Value.GetStreamAsync(uri).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Could not download the file at {uri}", ex);
        }
    }
}

internal class FixedFieldMap<T> : Dictionary<string, Action<T, JsonElement>>
{
    public FixedFieldMap() : base(StringComparer.OrdinalIgnoreCase)
    {

    }
}

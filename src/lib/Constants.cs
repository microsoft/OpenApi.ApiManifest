// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.OpenApi.ApiManifest;

internal static class Constants
{
    public static readonly TimeSpan DefaultRegexTimeout = TimeSpan.FromSeconds(5);
}

internal static class ErrorMessage
{
    public const string FieldIsRequired = "'{0}' is a required property of '{1}'.";
    public const string FieldIsNotValid = "'{0}' is not valid.";
    public const string FieldLengthExceeded = "'{0}' length exceeded. Maximum length allowed is '{1}'.";
    public const string BaseUrlIsNotValid = "The {0} must be a valid URL and end in a slash.";
    public const string ApiDependencyNotFound = "Failed to get a valid apiDependency from the provided apiManifestDocument. The property is required generate a complete {0}.";
    public const string ApiDescriptionUrlNotFound = "ApiDescriptionUrl is missing in the provided apiManifestDocument. The property is required generate a complete {0}.";
    public const string OpenApiDocumentIsNull = "The OpenAPI document is null.";
}
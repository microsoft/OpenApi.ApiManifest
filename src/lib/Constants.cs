// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.OpenApi.ApiManifest
{
    internal static class Constants
    {
        public static readonly TimeSpan DefaultRegexTimeout = TimeSpan.FromSeconds(5);
    }

    internal static class ErrorMessage
    {
        public static string FieldIsRequired = "'{0}' is a required property of '{1}'.";
        public static string FieldIsNotValid = "'{0}' is not valid.";
        public static string FieldLengthExceeded = "'{0}' length exceeded. Maximum length allowed is '{1}'.";
        public static string BaseUrlIsNotValid = "The {0} must be a valid URL and end in a slash.";
    }
}
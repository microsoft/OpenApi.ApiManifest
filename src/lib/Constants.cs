namespace Microsoft.OpenApi.ApiManifest
{
    internal static class Constants
    {
        public static readonly TimeSpan DefaultRegexTimeout = TimeSpan.FromSeconds(5);
    }

    internal static class ErrorConstants
    {
        public static string FieldIsRequired = "'{0}' is a required property of '{1}'.";
        public static string FieldIsNotValid = "'{0}' is not valid.";
    }
}
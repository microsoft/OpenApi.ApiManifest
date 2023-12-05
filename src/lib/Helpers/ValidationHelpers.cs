using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.OpenApi.ApiManifest.Helpers
{
    internal static class ValidationHelpers
    {
        internal static void ValidateNullOrWhitespace(string parameterName, string? value, string parentName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, ErrorMessage.FieldIsRequired, parameterName, parentName));
        }

        internal static void ValidateLength(string parameterName, string? value, int maxLength)
        {
            if (value?.Length > maxLength)
                throw new ArgumentOutOfRangeException(parameterName, string.Format(CultureInfo.InvariantCulture, ErrorMessage.FieldLengthExceeded, parameterName, maxLength));
        }

        internal static void ValidateEmail(string parameterName, string? value, string parentName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, ErrorMessage.FieldIsRequired, parameterName, parentName));
            else
                ValidateEmail(parameterName, value!);
        }

        internal static void ValidateBaseUrl(string parameterName, string? baseUrl)
        {
            // Check if the baseUrl is a valid URL and ends in a slash.
            if (string.IsNullOrWhiteSpace(baseUrl) || !baseUrl!.EndsWith("/", StringComparison.Ordinal) || !Uri.TryCreate(baseUrl, UriKind.Absolute, out _))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, ErrorMessage.BaseUrlIsNotValid, nameof(baseUrl)), parameterName);
        }

        internal static void ThrowIfNull(object? obj, string? paramName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }

        private static readonly Regex s_emailRegex = new(@"^[^@\s]+@[^@\s]+$", RegexOptions.Compiled, Constants.DefaultRegexTimeout);
        private static void ValidateEmail(string parameterName, string value)
        {
            if (!s_emailRegex.IsMatch(value))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, ErrorMessage.FieldIsNotValid, parameterName), parameterName);
        }
    }
}

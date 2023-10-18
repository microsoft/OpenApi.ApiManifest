// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.OpenApi.ApiManifest.Exceptions
{
    public class ApiManifestException : Exception
    {
        public ApiManifestException(string message) : base(message)
        {
        }

        public ApiManifestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

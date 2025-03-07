# Changelog

## [2.0.0-preview2](https://github.com/microsoft/OpenApi.ApiManifest/compare/v2.0.0-preview1...v2.0.0-preview2) (2025-03-07)


### Bug Fixes

* upgrades oai.net to preview 12 ([8d3b960](https://github.com/microsoft/OpenApi.ApiManifest/commit/8d3b9604e0a19a10449f9d74815a8231734a4e8e))
* upgrades oai.net to preview 12 ([b283f44](https://github.com/microsoft/OpenApi.ApiManifest/commit/b283f445216c2005ed02f1ec8ab11b8c1d3141d7))

## [0.5.6] - 2024-03-06

### Changed

- Removed upper bound on system.text.json to allow people migrate to net9

## [0.5.5] - 2024-03-06

### Changed

- Fixed a bug where the api dependencies would be deduplicated if they had the same key with different casing.

## [0.5.4] - 2023-12-06

### Added

- Added support for netStandard2.0.

## [0.5.3] - 2023-11-16

### Added

- Added ToApiManifest extension method on OpenApiDocument. #46

## [0.5.2] - 2023-11-06

### Added

- Added support for conversion of API manifest document to OpenAI Plugin manifest. #4
- Added VerificationTokens property to OpenAI Plugin manifest auth type. #32
- Added OpenAI Plugin manifest validation. #32
- Added API Manifest validation. #5
- Added ApplicationName property to ApiManifestDocument. #5

### Changed

- Renamed Request class to RequestInfo to align with the API manifest specification. #21
- Renamed Auth property in ApiDependency to AuthorizationRequirements to align with the API manifest specification. #5

## [0.5.1] - 2023-08-17

### Changed

- Fixed typos in properties.

## [0.5.0] - 2023-08-17

### Added

- Initial release of the library.

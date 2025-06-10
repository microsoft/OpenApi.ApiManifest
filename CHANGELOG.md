# Changelog

## [2.0.0-preview6](https://github.com/microsoft/OpenApi.ApiManifest/compare/v2.0.0-preview5...v2.0.0-preview6) (2025-06-10)


### Features

* upgrades openapi.net.odata to the latest version ([f5aa2c1](https://github.com/microsoft/OpenApi.ApiManifest/commit/f5aa2c12a61c7054912efa71ce5fe61314b65843))
* upgrades openapi.net.odata to the latest version ([2835d30](https://github.com/microsoft/OpenApi.ApiManifest/commit/2835d301b4128f7b5182b5e88812f7cad1c89a72))

## [2.0.0-preview5](https://github.com/microsoft/OpenApi.ApiManifest/compare/v2.0.0-preview4...v2.0.0-preview5) (2025-06-03)


### Bug Fixes

* update namespace reference following OAI.net updates ([3219727](https://github.com/microsoft/OpenApi.ApiManifest/commit/321972705930a7d06e155c64b857d055cf55795c))

## [2.0.0-preview4](https://github.com/microsoft/OpenApi.ApiManifest/compare/v2.0.0-preview3...v2.0.0-preview4) (2025-04-17)


### Features

* upgrades oai.net to preview17 ([4c64ca9](https://github.com/microsoft/OpenApi.ApiManifest/commit/4c64ca9f7384d027831431950e35738369dd7c62))
* upgrades oai.net to preview17 ([97d96b4](https://github.com/microsoft/OpenApi.ApiManifest/commit/97d96b4a00948ddef538f9655e5a00bd99311309))

## [2.0.0-preview3](https://github.com/microsoft/OpenApi.ApiManifest/compare/v2.0.0-preview2...v2.0.0-preview3) (2025-03-18)


### Features

* upgrade oai.net ([14c3a80](https://github.com/microsoft/OpenApi.ApiManifest/commit/14c3a80e224213af77a967903a055b18af82dc5f))


### Bug Fixes

* upgrade oai.net to preview14 ([99435db](https://github.com/microsoft/OpenApi.ApiManifest/commit/99435db3c066a8d6c392ec350510e3bf5ab78cf9))

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

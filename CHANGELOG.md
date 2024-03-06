# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

- Fixes a bug where the api dependencies would be deduplicated if they had the same key with different casing.

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

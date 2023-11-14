# OpenAPI to API Manifest Mapping

## Overview

This document provides a mapping that's used to convert an OpenAPI document to an API manifest document. An OpenAPI document is a standard format for describing the interface and operations of a web service. An API manifest is a standard that's used to declare an application's HTTP API dependencies and includes links to API descriptions, specifics of HTTP API requests, and related authorization details.

## Mapping Diagram

The following diagram illustrates how an OpenAPI document is mapped to an API manifest document.

``` mermaid
graph LR
    subgraph OpenApiDocument
    A1[Info.Contact.Name]
    A2[Info.Contact.Email]
    A3[Info.Title]
    A4[Servers.Url]
    A5[Info.Version]
    A6[Paths.Key]
    A7[Paths.Operations.Key]
    end
    subgraph ApiManifestDocument
    B1[Publisher.Name]
    B2[Publisher.Email]
    B3[ApiDependencies.Key]
    B4["ApiDependencies[key].ApiDeploymentBaseUrl"]
    B5["ApiDependencies[key].ApiDescriptionVersion"]
    B6["ApiDependencies[key].Requests.UriTemplate"]
    B7["ApiDependencies[key].Requests.Method"]
    end
    A1 -- "( 1 )" --> B1
    A2 -- "( 2 )" --> B2
    A3 -- "( 3 )" --> B3
    A4 -- "( 4 )" --> B4
    A5 -- "( 5 )" --> B5
    A6 -- "( 6 )" --> B6
    A7 -- "( 7 )" --> B7
```

### Mapping Steps

1. `Publisher.Name`: If `Info.Contact.Name` is present in the OpenAPI document, it maps to `Publisher.Name` in the API Manifest document. If not, the default value is `publisher-name`. This field is required in the API Manifest.
2. `Publisher.Email`: If `Info.Contact.Email` is present in the OpenAPI document, it maps to `Publisher.Email` in the API Manifest document. If not, the default value is `publisher-email@example.com`. This field is required in the API Manifest.
3. `ApiDependencies.Key`: If a customer doesn't provide a key for an ApiDependency in the API Manifest document, the `Info.Title` from the OpenAPI document is used. The converter modifies the `Info.Title` value by removing any leading or trailing whitespace and replacing any spaces between words with `-`
4. `ApiDependencies[key].ApiDeploymentBaseUrl`: If the `Servers` field in the OpenAPI document contains at least one server, the URL of the first server maps to this field in the API Manifest document. If not, this field is assumed to be null.
5. `ApiDependencies[key].ApiDescriptionVersion`: The `Info.Version` from the OpenAPI document maps to this field in the API Manifest document.
6. `ApiDependencies[key].Requests.UriTemplate`: The `Paths.Key` from the OpenAPI document maps to `Requests.UriTemplate` field in the API Manifest document.
7. `ApiDependencies[key].Requests.Method`: The `Paths.Operations.Key` from the OpenAPI document maps to `Requests.Method` field in the API Manifest document.

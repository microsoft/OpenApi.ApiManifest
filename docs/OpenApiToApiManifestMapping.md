# OpenAPI to API Manifest Mapping

## Overview

This document specifies the requirements and procedures for converting an OpenAPI document to an API manifest document. An OpenAPI document is a standard format for describing the interface and operations of a web service. An API manifest is a way to store the dependencies that an application has on HTTP APIs.

## Mapping Diagram

The following diagram illustrates the mapping from the OpenAPI document to the API manifest document.

```mermaid
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
    B3[ApiDependencies.ApiName]
    B4[ApiDependencies.ApiDeploymentBaseUrl]
    B5[ApiDependencies.ApiDescriptionVersion]
    B6[ApiDependencies.Requests.UriTemplate]
    B7[ApiDependencies.Requests.Method]
    end
    A1 -- "1" --> B1
    A2 -- "2" --> B2
    A3 -- "3" --> B3
    A4 -- "4" --> B4
    A5 -- "5" --> B5
    A6 -- "6" --> B6
    A7 -- "7" --> B7
```

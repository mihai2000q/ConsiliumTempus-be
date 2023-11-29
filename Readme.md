# Consilium Tempus - Backend

- [Abstract](#abstract)
- [Get Started](#get-started)
    - [Restore Dependencies](#restore-dependencies)
    - [Dotnet User Secrets](#dotnet-user-secrets)
    - [Final Steps](#final-steps)
- [Testing](#testing)
- [Architecture](#architecture)
- [Api](#api)
- [Domain Models](#domain-models)

## Abstract

This is the backend of the Consilium Tempus application.

## Get Started

To get started developing on this backend application you will need to do the following steps.

### Dotnet User Secrets

To make sure that the dotnet user secrets are all setup type in the terminal the following command:

```sh
dotnet user-secrets list --project /src/ConsiliumTempus.Api
```

The above command should list out one secret key that is used for the Jwt Token Generator Settings.

### Restore Dependencies

To restore the nuget dependencies try the following command in the terminal:

```sh
dotnet restore
```

### Final steps

Now, to check whether the app can build without any errors run:

```sh
dotnet build
```

For testing purposes, run the application:

```sh
dotnet run
```

The application will run in https mode by default, but that can be changed by:

Send a request to test if it works, see examle below. By default, the host is `https://localhost:7121/api`

```js
GET {{host}}/auth/login
```

It will say access denied, however, now you know that you have a working connection to the API.

## Testing

The backend of the application is tested using the **xUnit** framework. The tests are divided into Unit and Integration Tests.<br> 
For more information check out the documentation on [Testing](docs/Testing.md)

## Architecture

The architecture implemented in this application is following the principles of Clean Coding, Clean Architecture and Domain-Driven-Design. 
The 4 layers of this type of architecture feature the:
- Domain Layer - the place where the domain models are structured
- Application Layer - intermediator between layers that validates and makes abstract calls to the infrastructure
- Infrastructure Layer - the data access layer for the database
- Presentation Layer - the endpoint where the application becomes exposed

For more, go check out [Architecture](docs/Architecture.md).

## Api

More about the Api and the HTTP Requests and Response can be found at [Api documentation](docs/Api.md).

## Domain Models

More about the domain models can be found at [Domain Models](docs/Domain.md).
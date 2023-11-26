# Consilium Tempus - Backend

- [Abstract](#abstract)
- [Get Started](#get-started)
    - [Restore Dependencies](#restore-dependencies)
    - [Final Steps](#final-steps)
- [Testing](#testing)
- [Architecture](#architecture)
- [Api](#api)
- [Domain Models](#domain-models)

## Abstract

This is the backend of the Consilium Tempus application.

## Get Started

To get started developing on this backend application you will need to do the following steps.

### Restore Dependencies

To restore the nuget dependencies try the following command in the terminal:

```
dotnet restore
```

### Final steps

Now, to check whether the app can build without any errors run:

```
dotnet build
```

For testing purposes, run the application:

```
dotnet run
```

The application will run in https mode by default, but that can be changed by:

Send a request to test if it works, see examle below. By default, the host is: ``https://localhost:7121/api``

```js
GET {{host}}/auth/login
```

It will say access denied, however, now you know that you have a working connection to the API.

## Testing

The backend of the application is tested using the **xUnit** framework. The tests are divided into Unit and Integration Tests.<br> 
For more information check out the documentation on [Testing](docs/Testing.md)

## Architecture

The architecture of this app...
See more at [Architecture](docs/Architecture.md)

## Api

The api of this app...
See more at [Api](docs/Api.md)

## Domain Models

More about the domain models can be found at [Domain Models](docs/Domain.md)
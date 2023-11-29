# Consilium Tempus - Backend

- [Abstract](#abstract)
- [Get Started](#get-started)
    - [Dotnet User Secrets](#dotnet-user-secrets)
    - [Docker](#docker)
    - [Local Development](#local-development)
- [Testing](#testing)
- [Architecture](#architecture)
- [Api](#api)
- [Domain Models](#domain-models)
- [Database](#database)

## Abstract

This is the backend of the Consilium Tempus application. It is written completely in C# using the ASP.Net Core framework and is using .NET 7.

## Get Started

To get started on contributing for the development of this backend application you can do it locally or by using Docker, by following the next steps.

### Dotnet User Secrets

For the development environment, a secret key for the Jwt Settings must be set in a safe storage. 

To initialize the environment for dotnet user-secrets, type in the following command:

```sh
dotnet user-secrets init
```

To add a ket-value secret pair, type in the following command (be sure that you are in the right directory):

```sh
dotnet user-secrets set --project /src/ConsiliumTempus.Api "JwtSettings:SecretKey" "Super-Secret-Key-Example"
```

To make sure that the dotnet user secrets are all setup type in the terminal the following command:

```sh
dotnet user-secrets list --project /src/ConsiliumTempus.Api
```

The above command should list out one secret key that is used for the Jwt Token Generator Settings.

To be noted that for the production environment another key is gonna be randomly written and encrypted using an official encrypting algorithm.

### Docker

docker compose file provided...

environment file ...

default user is SA

inside `appsettings.Development.json`, change the Server parameter from Localhost to the container name, which by default is: `CT-Database`

it is running in development mode by default via composer...

### Local Development

For local development you will either install your own database instance or use the one from the docker containers (recommended).

In case you do prefer another database instance, don't forget to change the database parameters inside the `.env` file and the `appsettings.Development.json` from within the Api Project.

#### Restore Dependencies

To restore the nuget dependencies try the following command in the terminal:

```sh
dotnet restore
```

#### Build

Now, to check whether the app can build without any errors run:

```sh
dotnet build
```

#### Run

To run the application, type in the following command:

```sh
dotnet run
```

The application will run in https by default, but that can be changed by inside the profile on the `launchsettings.json` file:

#### Test Development Environemnt

To send a request (via Postman) in order to test if everything works accordingly, see example below. By default, the *host* is `https://localhost:7123/api`

```js
POST {{host}}/auth/login
```

And the body of the request shall be:

```json
{
    "email": "Some@Example.com",
    "password": "Password123"
}
```

It will return an Invalid Credentials Error, however, now you know that you have a working connection to the API and to the database.

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

## Database

The database chosen for this application infrastructure is a Microsoft SQL Server. <br>
For more information about the database please check out the [documentation](docs/Database.md). 
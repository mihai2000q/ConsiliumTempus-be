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

To add a secret key-value pair, type in the following command (be sure that you are in the right directory):

```sh
dotnet user-secrets set --project ./src/ConsiliumTempus.Api "JwtSettings:SecretKey" "Super-Secret-Key-Example"
```

The above command "injects" the secret key: "Super-Secrey-Key-Example" into the .NET configuration files of the Api, `appsettings.json` and `appsettings.Development.json`, into the "JwtSettings" JSON object into the SecretKey Property (Note: if the name of the object, or the property shall change, the user-secret should too).

To make sure that the dotnet user secrets are all setup type in the terminal the following command:

```sh
dotnet user-secrets list --project ./src/ConsiliumTempus.Api
```

The above command should list out one secret key that is used for the Jwt Token Generator Settings.

To be noted that for the production environment another key is gonna be randomly written and encrypted using an official encrypting algorithm, as the dotnet user-secrets is used ONLY for development.

### Docker

If you decide to run the application in Docker, a `docker-compose.yml` file has been provided. This file uses variables from the `.env` file, which is locally defined for each user. Therefore, you can copy paste the `.env.dev` file and rename it to `.env`. It contains the parameters needed for the Database container initialization and appliance of migrations shell script.

Next, to sync up the Application with the Database, go inside the `appsettings.Development.json` file (inside `src/ConsiliumTempus.Api`), and change the *Server* parameter from **Localhost** to the container name, which by default is **CT-Database**, and also the *Port* from **7123** to **1433**.

Once you have the environment files setup, just type in the following command:

```sh
docker compose up -d
```

This command will open up 2 containers: one for the database and the other for the application, which is running by default in development mode.

To run only the docker database, run the following command:

```sh
docker compose up -d database
```

To finalize the database setup, run the `apply-migration-to-database.sh` shell script.

To enable the production mode in Docker, the `docker-compose.yml` should be changed (however, that does not work at the moment).

### Local Development

For local development you will either install your own database instance or use the one from the docker containers (recommended). <br>
For recapitulation purposes, just procure an `.env` file and start only the database container (the `appsettings.Development.json` shoud match the `.env` and the *Server* is **Localhost**).

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

To send a request (via Postman) in order to test if everything works accordingly, see example below. By default, the *host* is `http://localhost:7121/api` for http, and `http://localhost:7121/api` for https (you can use any of them in https mode).

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
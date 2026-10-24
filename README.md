# Consilium Tempus - Backend

- [Abstract](#abstract)
- [Overview](#overview)
- [Get Started](#get-started)
    - [Prerequisites](#prerequisites)
    - [Dotnet User Secrets](#dotnet-user-secrets)
    - [Docker Containers](#docker-containers)
    - [Local Development](#local-development)
    - [Test Development Environment](#test-development-environment)
    - [Sample Data](#sample-data)
- [Testing](#testing)
- [Git](#git)
- [Architecture](#architecture)
- [Security](#security)
- [Api](#api)
- [Domain Models](#domain-models)
- [Database](#database)
- [Configuration](#configuration)
- [Continuous Integration](#continuous-integration)
- [Feature Development Workflow](#feature-development-workflow)
- [Dependency Licensing](#dependency-licensing)
- [License](#license)

## Abstract

This is the backend of the Consilium Tempus application. 
It is written completely in C# using the ASP.Net Core framework.

## Overview

For a summary of what the application does and how its concepts relate (workspaces, projects, sprints, tasks, etc.),
check out the [Overview](docs/Overview.md).

## Get Started

To get started on contributing to the development of this backend application,
you can do it locally or by using Docker,
by following the next steps.

At a glance, the first run looks like this:

1. install the [prerequisites](#prerequisites)
2. set the [secret key](#dotnet-user-secrets)
3. create the `.env` file, start the database and apply the migration
   (the first three steps from [Docker Containers](#docker-containers), which are the same for both ways)
4. run the application, either [in Docker](#docker-containers) (the last two steps) or [locally](#local-development)
5. [send a test request](#test-development-environment)

### Prerequisites

#### Microsoft SDK

This application is written in .NET 10.0; therefore, the developer requires the Microsoft SDK of that version. 
If you do not have it already, please download it from here `https://dotnet.microsoft.com/en-us/download/dotnet/10.0` 
(you may have to restart afterward).

#### Docker

Developing becomes easier while using isolated containers provided by Docker, so, 
if you do not have it installed already, please do so from here: 
`https://www.docker.com/products/docker-desktop/` (you may have to restart afterward).

### Dotnet User Secrets

For the development environment, a secret key for the Jwt Settings must be set in a safe storage. 
The store has already been initialized on the Api Project; all that is left is to add the key. 

To add a secret key-value pair, type in the following command (be sure that the key is longer than 36 characters):

```sh
dotnet user-secrets set --project ./src/ConsiliumTempus.Api "JwtSettings:SecretKey" "This-is-a-Super-Duper-Secret-Key-Example"
```

The above command injects the secret key: *"This-is-a-Super-Duper-Secret-Key-Example"* 
into the .NET development configuration file of the Api, `appsettings.Development.json`, 
into the **JwtSettings** JSON object right inside the **SecretKey** Property 
(Note: if the name of the object, or the property shall change, the user-secret should too).

To make sure that the dotnet user secrets are all setup type in the terminal, the following command:

```sh
dotnet user-secrets list --project ./src/ConsiliumTempus.Api
```

The above command should list out one secret key used for the Jwt Token Generator Settings.

To be noted that for the production environment, you have to provide your own key inside the `appsettings.json`, 
as the dotnet user-secrets feature is used only and only for development.

### Docker Containers

If you decide to run the application in Docker, a `docker-compose.yml` file has been provided.
Follow the steps below, in this order:

1. Create the `.env` file, which is locally defined for each user,
   by copying the `.env.dev` file and renaming it to `.env`.
   It contains the parameters needed for the Database container initialization
   and the Docker Compose services (see [Configuration](docs/Configuration.md#environment-file)).
   On Linux/macOS, also set `USER_SECRETS_PATH` in the `.env` file (see `.env.sample`),
   as the user secrets folder defaults to the Windows location.

2. Start the application, by typing the following command:

   ```sh
   docker compose up -d
   ```

   This command will build and start the containers, in this order:
   the database, the migrator (a one-shot service that applies the migrations to the database and exits),
   and the application, which is running by default in development mode.
   <br>
   Docker Compose overrides the database settings of the application container with the values from the `.env` file,
   so `appsettings.Development.json` does not have to be changed (see [Configuration](docs/Configuration.md#environment-file)).

### Local Development

For local development, you will either install your own database instance or use the one from the docker containers
(recommended).
<br>
For recapitulation purposes:
- create an `.env` file
- start only the database container
- make sure the `DatabaseSettings` from `appsettings.Development.json` match the `.env`
  (the *Port* is **7133** by default)
- apply the migrations

To run only the docker database, with the migrations applied (the `migrator` service exits when it is done),
run the following command:

```sh
docker compose up -d database migrator
```

#### Restore Dependencies

To restore the nuget dependencies, try the following command in the terminal:

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
dotnet run --project ./src/ConsiliumTempus.Api
```

The application will run in https by default, 
but that can be changed inside the profiles of the `launchSettings.json` file,
or by specifying the profile on run.

### Test Development Environment

To send a request (via Postman) to test if everything works accordingly, see the example below.
By default, the *host* is `http://localhost:7123` for http,
and `https://localhost:7122` for https (you can use any of them in https mode).
<br>
To be noted that, in case docker is used, the default port is: `7123`.

```js
POST {{host}}/api/auth/login
```

And the body of the request shall be:

```json
{
    "email": "Some@Example.com",
    "password": "Password123"
}
```

It will return an Invalid Credentials Error, however, 
now you know that you have a working connection to the API and to the database.

### Sample Data

Inside the `sample-data` directory there are `.sql` files intended to insert into the Database with sample data,
that can be used for both backend and frontend development. 
To insert it, run the `scripts/add-sample-data.sh` script.

## Testing

The backend of the application is tested using the **xUnit** framework. 
The tests are divided into Unit, Integration, Architecture and End-To-End Tests.
<br> 
For more information, check out the documentation on [Testing](docs/Testing.md).

## Git

The application is stored on the well-known cloud-based service and version control **GitHub**.
<br>
More about the decisions about Git can be found [here](docs/Git.md).

## Architecture

The architecture implemented in this application is following the principles of Clean Coding,
Clean Architecture, and Domain-Driven-Design. 
The four layers of this type of architecture feature the:
- Domain Layer—the place where the domain models are defined
- Infrastructure Layer—the data access layer for the database
- Application Layer—intermediate between layers that validates, makes abstract calls to the infrastructure and makes business logic decisions
- Presentation Layer—the endpoint where the application becomes exposed

For more, go check out the [documentation](docs/Architecture.md).

## Security

The system features an RBAC way of authorizing the user.
<br>
For more information about the security of the application, check out the [documentation](docs/Security.md).

## Api

More about the Api and the HTTP Requests and Response can be found at [Api documentation](docs/Api.md).

## Domain Models

More about the domain models can be found at [Domain Models](docs/Domain.md).

## Database

The database chosen for this application infrastructure is a Microsoft SQL Server. 
<br>
For more information about the database, please check out the [documentation](docs/Database.md). 

## Configuration

The settings of the application, the environment file and the logging are described in the [Configuration documentation](docs/Configuration.md).

## Continuous Integration

The workflows that build and test the application, and how the releases are made, are described in the [CI documentation](docs/CI.md).

## Feature Development Workflow

For a summary on best practices and how to start implementing new features, 
check out the [guidelines](docs/Feature-Development-Workflow.md).

## Dependency Licensing

Some libraries moved from an open source license to a commercial one, so these are pinned to their last open source release and must not be upgraded past it:

| Package | Pinned to | Commercial from |
|---|---|---|
| MediatR (and MediatR.Contracts) | 12.5.0 (2.0.1) | 13.x |
| FluentAssertions | 7.2.2 | 8.x |

Check the license of a package before bumping its major version.

## License

The project is licensed under the [Apache](https://www.apache.org/licenses/LICENSE-2.0) license.

---

**Consilium Tempus** 2023 - 2026

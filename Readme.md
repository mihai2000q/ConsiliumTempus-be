# Consilium Tempus - Backend

- [Abstract](#abstract)
- [Get Started](#get-started)
    - [Prerequisites](#prerequisites)
    - [Dotnet User Secrets](#dotnet-user-secrets)
    - [Docker Containers](#docker-containers)
    - [Local Development](#local-development)
- [Testing](#testing)
- [Git](#git)
- [Architecture](#architecture)
- [Api](#api)
- [Domain Models](#domain-models)
- [Database](#database)
- [License](#license)

## Abstract

This is the backend of the Consilium Tempus application. It is written completely in C# using the ASP.Net Core framework.

## Get Started

To get started on contributing to the development of this backend application,
you can do it locally or by using Docker, 
by following the next steps.

### Prerequisites

#### Microsoft SDK

This application is written in .NET 7.0; therefore, the developer requires the Microsoft SDK of that version. 
If you do not have it already, please download it from here `https://dotnet.microsoft.com/en-us/download/dotnet/7.0` 
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
into the .NET configuration files of the Api, `appsettings.json` and `appsettings.Development.json`, 
into the **"JwtSettings"** JSON object right inside the **SecretKey** Property 
(Note: if the name of the object, or the property shall change, the user-secret should too).

To make sure that the dotnet user secrets are all setup type in the terminal, the following command:

```sh
dotnet user-secrets list --project ./src/ConsiliumTempus.Api
```

The above command should list out one secret key used for the Jwt Token Generator Settings.

To be noted that for the production environment, another key is going to be randomly written and 
encrypted using an official encrypting algorithm, as the dotnet user-secrets feature is used ONLY for development.

### Docker Containers

If you decide to run the application in Docker, a `docker-compose.yml` file has been provided. 
This file uses variables from the `.env` file, which is locally defined for each user. 
Therefore, you can copy and paste the `.env.dev` file and rename it to `.env`. 
It contains the parameters
needed for the Database container initialization and appliance of the migrations shell script.

Next, to sync up the Application with the Database, 
go inside the `appsettings.Development.json` file (inside `src/ConsiliumTempus.Api`), 
and change the *Server* parameter from **Localhost** to the container name, 
which by default is **CT-Database**, and also the *Port* from **7123** to **1433**.

Once you have the environment files setup, type in the following command:

```sh
docker compose up -d
```

This command will open up two containers: one for the database and the other for the application,
which is running by default in development mode.

To finalize the database setup, run the `apply-migration-to-database.sh` shell script inside the *scripts* folder. 
<br>
This script will also install the necessary tools for that purpose.

To enable the production mode in Docker, the `docker-compose.yml` should be changed (however, that does not work at the moment).

### Local Development

For local development, you will either install your own database instance or use the one from the docker containers
(recommended).
<br>
For recapitulation purposes:
- create an `.env` file
- start only the database container 
- make sure the `appsettings.Development.json` still matches the `.env`
- apply the migration

To run only the docker database, run the following command:

```sh
docker compose up -d database
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

The application will run in https by default, but that can be changed by inside the profile on the `launchsettings.json` file, or specify the profile on run.

### Test Development Environment

To send a request (via Postman) to test if everything works accordingly, see the example below.
By default, the *host* is `http://localhost:7121/api` for http,
and `https://localhost:7123/api` for https (you can use any of them in https mode).
<br>
To be noted that, in case docker is used, the default port is: `7124`;

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

The backend of the application is tested using the **xUnit** framework. 
The tests are divided into Unit and Integration Tests.
<br> 
For more information, check out the documentation on [Testing](docs/Testing.md).

## Git

The application is stored on the well-known cloud-based service and version control **GitHub**.
<br>
More about the consequences for Git can be found [here](docs/Git.md).

## Architecture

The architecture implemented in this application is following the principles of Clean Coding,
Clean Architecture, and Domain-Driven-Design. 
The four layers of this type of architecture feature the:
- Domain Layer—the place where the domain models are defined
- Infrastructure Layer—the data access layer for the database
- Application Layer—intermediate between layers that validates and makes abstract calls to the infrastructure
- Presentation Layer—the endpoint where the application becomes exposed

For more, go check out the [documentation](docs/Architecture.md).

## Api

More about the Api and the HTTP Requests and Response can be found at [Api documentation](docs/Api.md).

## Domain Models

More about the domain models can be found at [Domain Models](docs/Domain.md).

## Database

The database chosen for this application infrastructure is a Microsoft SQL Server. 
<br>
For more information about the database, please check out the [documentation](docs/Database.md). 

## License

Copyright 2023 Consilium Tempus

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

---

**Consilium Tempus** 2023 - 2024
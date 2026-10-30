# Consilium Tempus Backend Configuration

* [Environments](#environments)
* [Settings](#settings)
  * [Jwt Settings](#jwt-settings)
  * [Cors Settings](#cors-settings)
  * [Database Settings](#database-settings)
  * [Other Settings](#other-settings)
* [Environment File](#environment-file)
* [Logging](#logging)

The application is configured inside the `src/ConsiliumTempus.Api` project, through the `appsettings` files,
the dotnet user secrets and, for Docker, the `.env` file.

## Environments

The application runs in one of the following environments:

- **Development**, which is used for local development and by the Docker environment from the README
- **Testing**, which is used by the [integration tests](Testing.md#integration-testing)
- **Production**, which is the one that .NET uses when no environment is set

For each of them, the settings are read from the following sources, from the weakest to the strongest
(the next one overrides the previous):

1. `appsettings.json`, with the settings that are common to all the environments
2. `appsettings.{Environment}.json` (i.e., `appsettings.Development.json`)
3. the dotnet user secrets (only for Development, see [Dotnet User Secrets](../README.md#dotnet-user-secrets))
4. the environment variables

## Settings

### Jwt Settings

| Setting         | Description                                                                          |
|-----------------|--------------------------------------------------------------------------------------|
| `SecretKey`     | the key that signs the tokens. It is empty on purpose and it must be provided        |
| `ExpiryMinutes` | the lifetime of an access token, in minutes (5 by default), plus a clock skew of 1 minute |
| `Issuer`        | the issuer of the tokens, that is also validated on each request                     |
| `Audience`      | the audience of the tokens, that is also validated on each request                   |

Development provides the secret key through the user secrets, and the other environments through the `appsettings.json`
(the CI injects it from a secret, see [Continuous Integration](CI.md)).

The refresh token is not configurable, as it always lasts one month.

### Database Settings

| Setting    | Description                                  |
|------------|----------------------------------------------|
| `Server`   | the host name of the SQL Server              |
| `Port`     | the port of the SQL Server                   |
| `Name`     | the name of the database                     |
| `User`     | the user that connects to the database       |
| `Password` | the password of that user                    |

They are put together to create the connection string of the database.
The default values depend on where the application runs:

| Where                                                  | Source                         | Server                          | Port   |
|--------------------------------------------------------|--------------------------------|---------------------------------|--------|
| On the host, in Development                            | `appsettings.Development.json` | `Localhost` (the local machine) | `7133` |
| In Docker (`docker-compose.yml`, `docker-compose-prod.yml`) | the `.env` file (see below) | `database` (the service)        | `1433` |

The values inside `appsettings.json` are empty, so any other environment has to provide its own
(i.e., through the `DatabaseSettings__*` environment variables).

The integration tests do not use any of them, as they create their own database container.

### Cors Settings

| Setting          | Description                                                                          |
|------------------|--------------------------------------------------------------------------------------|
| `FrontendOrigin` | the only origin that the CORS policy of the frontend allows (i.e., `http://localhost:8123`) |

It is empty in the `appsettings.json` on purpose, and the application does not start without it.
Development and the integration tests (`appsettings.Testing.json`) allow `http://localhost:8123`,
and the production Docker Compose reads it from the `FRONTEND_ORIGIN` of the `.env` file
(passed as `CorsSettings__FrontendOrigin`).

### Other Settings

- **Logging** and **Serilog** configure the logs (see [Logging](#logging))
- **AllowedHosts** is the standard .NET setting, and it allows all the hosts

Some values are not settings, as they are written in the code:

- the CORS policy of the frontend allows the methods *GET*, *POST*, *PUT*, *PATCH* and *DELETE* and any header (see `CorsPolicies`)
- the `api` prefix of all the routes

## Environment File

The `.env` file is read by Docker Compose and by the e2e tests.
It is created by copying `.env.dev` (see [Docker Containers](../README.md#docker-containers)).

| Variable            | Description                                                                                  |
|---------------------|----------------------------------------------------------------------------------------------|
| `DATABASE_PORT`     | the port of the host that is mapped to the port of the database container                     |
| `DATABASE_NAME`     | the name of the database                                                                     |
| `DATABASE_USER`     | the user of the database                                                                     |
| `DATABASE_PASSWORD` | the password of the database user                                                            |
| `FRONTEND_ORIGIN`   | the origin that the CORS policy allows, only required by `docker-compose-prod.yml` (see [Cors Settings](#cors-settings)) |
| `API_PORT`          | the port of the host that is mapped to the port `8080` of the application container (also used by the e2e tests) |
| `SAMPLE_DATA_ENABLED` | `true` to insert the sample data when the migrator runs, only for development (see [Sample Data](../README.md#sample-data)) |
| `USER_SECRETS_PATH` | optional, the folder of the dotnet user secrets, for Linux and macOS (see `.env.sample`)     |

Docker Compose passes the database variables to the application container as the `DatabaseSettings__*` environment variables,
which override the `appsettings` files (the server is the `database` service and the port is `1433`).
So, in Docker, the `.env` file is the only source of the database settings.

When running the application directly on the host, the `DatabaseSettings` from `appsettings.Development.json`
must still match the `.env` file (the *Port* with `DATABASE_PORT`, and the *Name*, *User* and *Password*).

## Logging

The logs are written by **Serilog**, which is configured from the `Serilog` section of the `appsettings` files.
Each log goes to the following places:

- the console
- a text file, `logs/backend-log.api-.log` in Development (`backend-log.webapi-.log` otherwise)
- a JSON file with the same name, inside the same folder

The files are created inside the `logs` folder, next to the running application, and a new one is created each day.
The writing is asynchronous, and the folder is ignored by Git.

The minimum level is *Information* in Development (with the MVC messages only from *Warning* up),
and *Warning* in the other environments (except for a few hosting and routing messages, that are *Information*).

The format of the text files mentions a `CorrelationId` and a `Username`,
however, nothing populates them for now, so they should not be relied upon.

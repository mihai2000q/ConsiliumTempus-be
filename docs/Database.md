# Consilium Tempus Backend Database

The RDBMS chosen for this application infrastructure is a Microsoft SQL Server.

## Migrations

As the infrastructure layer relies on the **Entity Framework Core**, to release, update or create any kind of migration we will use the EF CLI provided by the dotnet tools.

To install the dotnet EF Core CLI, please type the following in the terminal:

```sh
dotnet tool install --global dotnet-ef --version 7.0.14
```
The version has been mentioned so that we avoid using .NET 8, as the project is still in .NET 7.

To make sure that it has been installed you can type the following:

```sh
dotnet tool list --global
```

In order to create a migration you can use the following command: 

```sh
dotnet ef migrations add Update -p ./src/ConsiliumTempus.Infrastructure -s ./src/ConsiliumTempus.Api/
```

That will create a migration named with a timestamp followed by the suffix *Update*. 

The **Naming Convention** is \[VERB\_TABLE(\_COLUMN)\], for example, if the user table has been modified by the _Email_ Column, the name would be `Update_Users_Email`, otherwise, if the table has been thoroughly modified `Update_Users` suffices.

To apply the migration you can type the following command:

```sh
dotnet ef database update -p ./src/ConsiliumTempus.Infrastructure -s ./src/ConsiliumTempus.Api/ --connection "Server={{database_server}},{{database_port}};Database={{database_name}};User Id={{database_user}};Password={{database_password}};Encrypt=false"
```

Just make sure that inside the double brackets you add the actual parameters, but by default:
- **database_server** is *localhost* (for local development)
- **database_port** is *7133* (Lead Choice)
- **database_name** is ConsiliumTempus (Lead Choice)
- **database_user** is SA (by default from MsSql)
- **database_password** is StrongPassword123 (Lead Choice)

Check out the `.env` file, in case they have been changed.

Alternatively, the shell script `apply-migration-to-database.sh` can be used, as you did when you got setup.

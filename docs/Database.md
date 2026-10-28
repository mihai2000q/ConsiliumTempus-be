# Consilium Tempus Backend Database

* [Naming Convention](#naming-convention)
  * [Tables](#tables)
* [Conventions](#conventions)
  * [Delete Behavior](#delete-behavior)
  * [Generated Keys](#generated-keys)
  * [Audit](#audit)
* [Migrations](#migrations)
  * [Dotnet tools](#dotnet-tools)
  * [Create Migration](#create-migration)
  * [Migration Naming Convention](#migration-naming-convention)
  * [Apply Migration](#apply-migration)
  * [Apply Migration—with the connection flag](#apply-migrationwith-the-connection-flag)

The RDBMS chosen for this application infrastructure is a Microsoft SQL Server.
<br>
More about the Database Design choices can be read at [Database Diagrams](Domain.md#database-diagrams), 
and for a clear picture of the whole application check out the [Database Diagram](database-diagrams/Database.Diagram.md). 

## Naming Convention

### Tables

It has been decided to use **singular** for table naming because of several reasons,
such as aestheticism, order or simplicity, plus **Pascal Case** as it is a C# based application.

## Conventions

Some of the rules of the model are applied to the whole model through **EF Core conventions**,
instead of being repeated on each configuration.
They live in `Persistence/Conventions`, and they are registered in the `ConsiliumTempusDbContext`
(`ConfigureConventions`).
A convention applies only when the configuration did not say otherwise,
so a configuration can always override it explicitly.

### Delete Behavior

By default, **EF Core** cascades the delete of the required relationships, which can delete data by accident
(i.e., deleting a user would delete all the tasks that they created).
That is why every relationship is **restrictive** (`Restrict`, which is `NO ACTION` in SQL Server),
unless its delete behavior is configured explicitly, through the `RestrictDeleteBehaviorConvention`
(so the configurations never have to say `Restrict`).

Whenever the delete of a principal should affect its dependents, the relationship has to say it:

- `Cascade`, when the dependents belong to the principal, and cannot exist without it
(i.e., the sprints of a project, or the custom fields of a task)
- `SetNull`, when the dependents should survive without the principal
(i.e., the person of a people custom field, when the user is deleted)

In any other case, the dependents have to be cleared by the application before the principal is deleted
(i.e., the *User Deleted* event clears the user from the tasks and from the audit data).

A few things to keep in mind:

- SQL Server allows only **one cascading action** (`Cascade` or `SetNull`) from a table to the same table,
and it does not allow **multiple cascade paths** either.
That is why the custom fields cascade from their tasks, but the custom field setups do not cascade from their workspace
(see the comments on the Project Task and the Custom Field Setup configurations).
- The delete behavior of the types that are mapped to their own table (table per type) cannot be configured,
and it always cascades to them.
- `DeleteBehaviorTest` lists all the relationships that cascade or set to null,
so the list has to be updated on purpose when one of them is added or changed.

### Generated Keys

The identifiers are created by the domain, never by the database.
That is why the `NeverGenerateKeysConvention` makes sure that none of the primary keys is generated
(no `IDENTITY` columns, no default values), without having to say it on each configuration.

### Audit

The [Audit](Domain.md#value-objects) of an entity (i.e., of a project sprint) is an **owned type**:
its columns (`CreatedById`, `CreatedDateTime`, `UpdatedById` and `UpdatedDateTime`) are in the table of the entity,
so there is no *Audit* table, and it goes away with the entity that holds it.
The configurations say it with `OwnsAudit`.

The entities that hold an audit implement `IAudited`, and the domain does not create nor update it.
The `StampAuditInterceptor` (which goes after the one that publishes the domain events, as they add and modify entities too)
stamps the current user and the date and time when the changes are saved, based on the state of the entity:

- **Added**: the audit is created, unless the entity already has one (i.e., the one of the data that is imported).
- **Modified**: the audit is updated, so any change on the entity updates it
(i.e., a stage whose position changes because another one is moved).

Two things do not count as a change of the entity: its **last activity** (the project and the workspace refresh it
whenever something under them changes), and the audit itself.
A change that is saved on another table but that has to count as an update of the entity
(i.e., the custom fields of a task) is marked explicitly, with `IProjectTaskRepository.MarkAsUpdated`.

When there is no current user (i.e., there is no request), or the current user is being deleted, the user is left empty.
The *User Deleted* event clears the user from the audits, which does not update them.

## Migrations

Before anything, migrations refer to the process of modifying a database schema in a controlled and organized way. 
This typically involves creating, updating, 
or deleting tables, columns, or other database structures to reflect changes in the application's data model.
Migrations ensure that changes to the database schema are versioned, 
and can be applied consistently across different environments (development, testing, production).
Also, they can be rolled back at any time.

As the infrastructure layer relies on the **Entity Framework Core**, to release,
update or create any kind of migration, we will use the EF CLI provided by the dotnet tools.

### Dotnet tools

To install the dotnet **EF Core CLI**, please type the following in the terminal:

```sh
dotnet tool install --global dotnet-ef --version 10.0.13
```

The version has been mentioned just as a precaution, as the project is using .Net 10
(it is the same version that the migrator image is built with, see the `Dockerfile`).
If an older version is already installed, update it with `dotnet tool update --global dotnet-ef --version 10.0.13`.

To make sure that it has been installed, you can type the following:

```sh
dotnet tool list --global
```

### Create Migration

To create a migration, you can use the following command:

```sh
dotnet ef migrations add Update_User_Email -p ./src/ConsiliumTempus.Infrastructure -s ./src/ConsiliumTempus.Api/
```

That will create a migration named with a timestamp followed by the suffix *Update_User_Email*.

Also, the [Database Diagrams](database-diagrams/Database.Diagram.md) are written by hand, so whenever a migration changes the schema,
the affected diagrams have to be updated as well.

### Migration Naming Convention

The **Naming Convention** is *VERB\_TABLE\_COLUMN*, for example,
if the user table has been modified by the _Email_ Column, the name would be `Update_User_Email`,
otherwise, if the table has been thoroughly modified `Update_User` suffices.

### Apply Migration

In Docker, the migrations are applied by the `migrator` service of the Docker Compose files,
which runs every time before the application starts (and it is safe to run again, as already applied migrations are skipped).
The migrator is built as an [EF Core migration bundle](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying#bundles),
so it needs neither the SDK nor the EF tools to run. To apply the migrations on their own, type the following command:

```sh
docker compose up -d database migrator
```

Outside of Docker, or when the application runs on the host machine against the database container,
you can also type the following command:

```sh
dotnet ef database update -p ./src/ConsiliumTempus.Infrastructure -s ./src/ConsiliumTempus.Api/
```

The command uses the configuration of the Api project in the *Development* environment
(`appsettings.Development.json` and the user secrets),
so the database has to be reachable from the machine that runs it,
i.e., `localhost` and the published port `7133` of the database container.
If that file points to the name of the container (**CT-Database**), use the connection flag from below.

### Apply Migration—with the connection flag

In case, the database target is not the one that the application already uses;
the _connection_ flag can be mentioned:

```sh
dotnet ef database update -p ./src/ConsiliumTempus.Infrastructure -s ./src/ConsiliumTempus.Api/ --connection "Server={{database_server}},{{database_port}};Database={{database_name}};User Id={{database_user}};Password={{database_password}};Encrypt=false"
```

Make sure that inside the double brackets you add the actual parameters, but by default:

- **database_server** is *localhost* (for local development)
- **database_port** is *7133* (Lead Choice)
- **database_name** is ConsiliumTempus (Lead Choice)
- **database_user** is SA (by default from MsSql)
- **database_password** is StrongPassword123 (Lead Choice)

Check out the `.env` file, and the `appsettings.Development.json`, in case they have been changed.

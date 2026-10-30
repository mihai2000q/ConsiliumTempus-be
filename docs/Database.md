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
- `OnDeleteBehaviorConfigurationTest` lists all the relationships that cascade or set to null,
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

The entities that hold an audit implement `IAudit`, and the domain does not create nor update it.

The `StampInterceptor` (which goes after the one that publishes the domain events, as they add and modify entities too)
stamps the current user and the date and time when the changes are saved, based on the state of the entity:

- **Added**: the audit is created, unless the entity already has one (i.e., the one of the data that is imported).
- **Modified**: the audit is updated, so any change on the entity updates it
(i.e., a stage whose position changes because another one is moved).

When there is no current user (i.e., there is no request), or the current user is being deleted, the user is left empty.
The only exception is the registration of a user: nobody is logged in, so the user that is registering is the one who does it
(i.e., the personal workspace that is created for them). This is only when exactly one user is added in the same save.
The *User Deleted* event clears the user from the audits (and from the authorships), which does not update them.

#### IAudit, IAuthored or ITimestamps

As a suggestion, not a rule: when an entity should be stamped, the choice depends on who can change it.

- An entity that **more than one user** can change should implement `IAudit`, so that it says who did it and when
(i.e., a workspace or a membership, which any admin can update).
- An entity that **only its author** can change (i.e., a comment) should implement `IAuthored`,
so that it says who wrote it, and when it was written and last edited. There is no updater to say,
as it is always the author.
- An entity that **only one user** can change and that is not written by a user (i.e., the data of a user,
such as their name or email) can implement `ITimestamps`, as the user is always the same one, so only the dates are useful.

### Authorship

The [Authorship](Domain.md#value-objects) of an entity (i.e., of a comment) is an **owned type** too:
its columns (`AuthorId`, `CreatedDateTime` and `UpdatedDateTime`) are in the table of the entity,
and the configurations say it with `OwnsAuthorship` (which also works for the entities that are owned, like the comments of a task).

The entities that hold it implement `IAuthored`, and the domain does not create nor update it either.
The `StampInterceptor` stamps it when the changes are saved, in the same way as the audit (with the same current user):

- **Added**: the authorship is created, with the current user as the author (unless the entity already has one).
- **Modified**: only the date of the update changes, as the author never does.

A change of an authored entity does not update the audit of the entity that it is a part of
(i.e., a comment does not update its task).
`AuthorshipConfigurationTest` makes sure that every `IAuthored` entity maps its authorship (with `OwnsAuthorship`).

#### Excluded from the audit

Some changes are not a change of the entity itself, so they do not update its audit.
They are excluded in the configuration of the entity, with `ExcludeFromAudit`:

- the **last activity** of a project and of a workspace, which is refreshed whenever something under them changes
- the **custom order position** of a project stage and of a project task, which changes whenever another one is moved or added

```csharp
builder.Property(p => p.LastActivity).ExcludeFromAudit();
builder.Navigation(t => t.CustomOrderPosition).ExcludeFromAudit();
```

`AuditExclusionConfigurationTest` lists all the exclusions, so a new one has to be added to the list on purpose.
`AuditConfigurationTest` makes sure that every `IAudit` entity maps its audit (with `OwnsAudit`).

A change that has to count anyway can be marked with `MarkAsUpdated` (on the repository, which marks it on the `ConsiliumTempusDbContext`),
without touching the entity. It is used when:

- the change is saved on another table (i.e., the custom fields of a task)
- the change is excluded, but the user did it to that entity (i.e., the stage or the task that the user moves,
as opposed to the ones that are only shifted)

#### ExecuteUpdate

`ExecuteUpdate` (and `ExecuteDelete`) do not go through the change tracker, so they skip the `StampInterceptor`.
That is why an `ExecuteUpdate` on an entity that has an audit is **forbidden**, unless:

- it only changes properties that are excluded from the audit (i.e., the last activity), or
- the change must not count as an update of the entity (i.e., when a user is deleted, `UserRepository.ClearReferences`
clears it from the audits of the entities that it touched, as that does not update them).

Any other change has to be loaded, changed and saved, so that the audit is stamped.

They run inside the transaction of the command (the `UnitOfWorkBehavior` wraps the handler and the save in a `TransactionScope`),
as the domain events that call them are published while the changes are saved.

They also do not update the entities that are already tracked, so the entities that a command loads
have to be changed through the entities instead (i.e., the invitations of a deleted user are loaded and removed).

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

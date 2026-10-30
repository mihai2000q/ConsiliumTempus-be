# Database Diagrams

## Project

### Relationships

- **Many-to-one** relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate.
- **Many-to-one** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
- **Many-to-many** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **UserHasFavoriteProject** entity).
- **Many-to-many** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **ProjectHasAllowedMember** entity).
- **One-to-many** relationship with the [Project Status](../../domain/entities/project/Entity.ProjectStatus.md) entity.
- **One-to-many** relationship with the [Project Sprint](../../domain/aggregates/Aggregate.ProjectSprint.md) aggregate.
- 2x **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **Audit**).

### Owned Entities Relationships

#### Project Status

- 2x **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **Audit**).

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    User ||--o{ Project : "OwnerId"
    Workspace ||--o{ Project : "WorkspaceId"
    Project ||--o{ ProjectHasAllowedMember : "ProjectId"
    User ||--o{ ProjectHasAllowedMember : "UserId"
    User |o--o{ ProjectStatus : "CreatedById"
    User |o--o{ ProjectStatus : "UpdatedById"
    Project ||--o{ ProjectStatus : "ProjectId"
    Project ||--o{ UserHasFavoriteProject : "ProjectId"
    User ||--o{ UserHasFavoriteProject : "UserId"
    Project ||--o{ ProjectHasCustomFieldSetup : "ProjectId"
    User |o--o{ Project : "CreatedById"
    User |o--o{ Project : "UpdatedById"
    Project ||--o{ ProjectSprint : "ProjectId"

    Project {
        uniqueidentifier Id PK
        nvarchar(100) Name
        nvarchar(max) Description
        bit IsPrivate
        uniqueidentifier OwnerId FK
        int Lifecycle
        datetime2 LastActivity
        uniqueidentifier WorkspaceId FK
        uniqueidentifier CreatedById FK "nullable"
        datetime2 CreatedDateTime
        uniqueidentifier UpdatedById FK "nullable"
        datetime2 UpdatedDateTime
    }
    ProjectHasAllowedMember {
        uniqueidentifier ProjectId PK, FK
        uniqueidentifier UserId PK, FK
    }
    ProjectStatus {
        uniqueidentifier Id PK
        nvarchar(50) Title
        int Status
        nvarchar(max) Description
        uniqueidentifier ProjectId FK
        uniqueidentifier CreatedById FK "nullable"
        datetime2 CreatedDateTime
        uniqueidentifier UpdatedById FK "nullable"
        datetime2 UpdatedDateTime
    }
    UserHasFavoriteProject {
        uniqueidentifier UserId PK, FK
        uniqueidentifier ProjectId PK, FK
    }
    User {
        uniqueidentifier Id PK
    }
    Workspace {
        uniqueidentifier Id PK
    }
    ProjectHasCustomFieldSetup {
        uniqueidentifier CustomFieldSetupId PK, FK
        uniqueidentifier ProjectId PK, FK
    }
    ProjectSprint {
        uniqueidentifier Id PK
    }
```

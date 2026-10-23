# Database Diagrams

## Workspace

### Relationships

- **Many-to-many** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
(through the **Many-to-one** relationship with the [Membership](../../domain/entities/Entity.Membership.md) entity).
- **Many-to-many** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
  (through the **UserHasFavoriteWorkspace** entity).
- **Many-to-one** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
- **One-to-Many** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.
- **One-to-Many** relationship with the [Custom Field Setup](../../domain/aggregates/Aggregate.CustomFieldSetup.md) aggregate (optional).
- **One-to-Many** relationship with the [Workspace Invitation](../../domain/entities/workspace/Entity.WorkspaceInvitation.md) entity.

### Entities Relationships

#### Workspace Invitation

- 2x **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    User ||--o{ Workspace : "OwnerId"
    User ||--o{ Membership : "UserId"
    WorkspaceRole ||--o{ Membership : "WorkspaceRoleId"
    Workspace ||--o{ Membership : "WorkspaceId"
    User ||--o{ UserHasFavoriteWorkspace : "UserId"
    Workspace ||--o{ UserHasFavoriteWorkspace : "WorkspaceId"
    User ||--o{ WorkspaceInvitation : "CollaboratorId"
    User ||--o{ WorkspaceInvitation : "SenderId"
    Workspace ||--o{ WorkspaceInvitation : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_Date : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_DateTime : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_Duration : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_MultiSelect : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_Number : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_People : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_SingleSelect : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_Text : "WorkspaceId"
    Workspace |o--o{ CustomFieldSetup_Time : "WorkspaceId"
    Workspace ||--o{ Project : "WorkspaceId"

    Workspace {
        uniqueidentifier Id PK
        nvarchar(100) Name
        nvarchar(max) Description
        bit IsPersonal
        uniqueidentifier OwnerId FK
        datetime2 LastActivity
        datetime2 CreatedDateTime
        datetime2 UpdatedDateTime
    }
    Membership {
        uniqueidentifier UserId PK, FK
        uniqueidentifier WorkspaceId PK, FK
        int WorkspaceRoleId FK
        datetime2 CreatedDateTime
        datetime2 UpdatedDateTime
    }
    UserHasFavoriteWorkspace {
        uniqueidentifier UserId PK, FK
        uniqueidentifier WorkspaceId PK, FK
    }
    WorkspaceInvitation {
        uniqueidentifier Id PK
        datetime2 CreatedDateTime
        uniqueidentifier SenderId FK
        uniqueidentifier CollaboratorId FK
        uniqueidentifier WorkspaceId FK
    }
    User {
        uniqueidentifier Id PK
    }
    WorkspaceRole {
        int Id PK
    }
    CustomFieldSetup_Date {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_DateTime {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Duration {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_MultiSelect {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Number {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_People {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_SingleSelect {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Text {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Time {
        uniqueidentifier Id PK
    }
    Project {
        uniqueidentifier Id PK
    }
```

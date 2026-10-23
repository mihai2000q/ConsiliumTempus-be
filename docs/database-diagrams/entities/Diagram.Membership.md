# Database Diagrams

## Membership

### Relationships

- **Many-to-one** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
- **Many-to-one** relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate.
- **Many-to-one** relationship with the [Workspace Role](../../domain/entities/Entity.WorkspaceRole.md) entity.

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    User ||--o{ Membership : "UserId"
    WorkspaceRole ||--o{ Membership : "WorkspaceRoleId"
    Workspace ||--o{ Membership : "WorkspaceId"

    Membership {
        uniqueidentifier UserId PK, FK
        uniqueidentifier WorkspaceId PK, FK
        int WorkspaceRoleId FK
        datetime2 CreatedDateTime
        datetime2 UpdatedDateTime
    }
    User {
        uniqueidentifier Id PK
    }
    WorkspaceRole {
        int Id PK
    }
    Workspace {
        uniqueidentifier Id PK
    }
```

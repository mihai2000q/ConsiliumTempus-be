# Database Diagrams

## Workspace Role

### Relationships

- **Many-to-many** relationship with the [Permission](../../domain/entities/Entity.Permission.md) entity.
- **One-to-Many** relationship with the [Membership](../../domain/entities/Entity.Membership.md) entity.

### Diagram

```mermaid
erDiagram
    Permission ||--o{ WorkspaceRoleHasPermission : "PermissionId"
    WorkspaceRole ||--o{ WorkspaceRoleHasPermission : "WorkspaceRoleId"
    WorkspaceRole ||--o{ Membership : "WorkspaceRoleId"

    WorkspaceRole {
        int Id PK
        nvarchar(max) Name
        nvarchar(max) Description
    }
    WorkspaceRoleHasPermission {
        int WorkspaceRoleId PK, FK
        int PermissionId PK, FK
    }
    Permission {
        int Id PK
    }
    Membership {
        uniqueidentifier UserId PK, FK
        uniqueidentifier WorkspaceId PK, FK
    }
```

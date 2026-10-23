# Database Diagrams

## Permission

### Relationships

- **Many-to-many** relationship with the [Workspace Role](../../domain/entities/Entity.WorkspaceRole.md) entity.

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    Permission ||--o{ WorkspaceRoleHasPermission : "PermissionId"
    WorkspaceRole ||--o{ WorkspaceRoleHasPermission : "WorkspaceRoleId"

    Permission {
        int Id PK
        nvarchar(max) Name
    }
    WorkspaceRoleHasPermission {
        int WorkspaceRoleId PK, FK
        int PermissionId PK, FK
    }
    WorkspaceRole {
        int Id PK
    }
```

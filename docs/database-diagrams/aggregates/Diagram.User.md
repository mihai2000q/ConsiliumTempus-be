# Database Diagrams

## User

### Relationships

- **Many-to-many** relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate
(through the **Many-to-one** relationship with the [Membership](../../domain/entities/Entity.Membership.md) entity).
- **Many-to-many** relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate
(through the **UserHasFavoriteWorkspace**).
- 2x **One-to-Many** relationship with the [Workspace Invitation](../../domain/entities/workspace/Entity.WorkspaceInvitation.md) entity.
- **Many-to-many** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate
(through the **UserHasFavoriteProject**).
- **Many-to-many** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate
  (through the **ProjectHasAllowedMember**).
- **One-to-many** relationship with the [Custom Field](../../domain/entities/project-task/Entity.CustomField.md) entity
(the *People* variant).
- **One-to-many** relationship with the [Refresh Token](../../domain/entities/Entity.RefreshToken.md) entity.
- 2x **One-to-many** relationship with the [Audit](../../domain/entities/Entity.Audit.md) entity.
- **One-to-many** relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate.
- **One-to-many** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.
- 3x **One-to-many** relationships with the [Project Task](../../domain/aggregates/Aggregate.ProjectTask.md) aggregate.
- **One-to-many** relationship with the [Project Task Comment](../../domain/entities/project-task/Entity.ProjectTaskComment.md) entity.

### Diagram

```mermaid
erDiagram
    User ||--o{ RefreshToken : "UserId"
    RefreshToken ||--o{ RefreshTokenHistory : "RefreshTokenId"
    User ||--o{ Membership : "UserId"
    WorkspaceRole ||--o{ Membership : "WorkspaceRoleId"
    Workspace ||--o{ Membership : "WorkspaceId"
    User ||--o{ UserHasFavoriteWorkspace : "UserId"
    Workspace ||--o{ UserHasFavoriteWorkspace : "WorkspaceId"
    Project ||--o{ UserHasFavoriteProject : "ProjectId"
    User ||--o{ UserHasFavoriteProject : "UserId"
    User |o--o{ Audit : "CreatedById"
    User |o--o{ Audit : "UpdatedById"
    User ||--o{ Workspace : "OwnerId"
    User ||--o{ Project : "OwnerId"
    User ||--o{ WorkspaceInvitation : "CollaboratorId"
    User ||--o{ WorkspaceInvitation : "SenderId"
    User ||--o{ ProjectHasAllowedMember : "UserId"
    User |o--o{ ProjectTask : "AssigneeId"
    User ||--o{ ProjectTask : "CreatedById"
    User |o--o{ ProjectTask : "ReviewerId"
    User |o--o{ CustomField_People : "PersonId"
    User ||--o{ ProjectTaskComment : "CreatedById"

    User {
        uniqueidentifier Id PK
        nvarchar(256) Email UK
        nvarchar(max) Password
        nvarchar(100) FirstName
        nvarchar(100) LastName
        nvarchar(50) Role "nullable"
        date DateOfBirth "nullable"
        datetime2 CreatedDateTime
        datetime2 UpdatedDateTime
    }
    RefreshToken {
        uniqueidentifier Id PK
        datetime2 ExpiryDateTime
        bit IsInvalidated
        datetime2 CreatedDateTime
        uniqueidentifier UserId FK
    }
    RefreshTokenHistory {
        uniqueidentifier Id PK
        uniqueidentifier JwtId
        datetime2 CreatedDateTime
        uniqueidentifier RefreshTokenId FK
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
    UserHasFavoriteProject {
        uniqueidentifier UserId PK, FK
        uniqueidentifier ProjectId PK, FK
    }
    WorkspaceRole {
        int Id PK
    }
    Audit {
        uniqueidentifier Id PK
    }
    Workspace {
        uniqueidentifier Id PK
    }
    Project {
        uniqueidentifier Id PK
    }
    WorkspaceInvitation {
        uniqueidentifier Id PK
    }
    ProjectHasAllowedMember {
        uniqueidentifier ProjectId PK, FK
        uniqueidentifier UserId PK, FK
    }
    ProjectTask {
        uniqueidentifier Id PK
    }
    CustomField_People {
        uniqueidentifier Id PK
    }
    ProjectTaskComment {
        uniqueidentifier Id PK
    }
```

# Database Diagrams

## Project Sprint

### Relationships

- **Many-to-one** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.
- **One-to-many** relationship with the [Project Stage](../../domain/entities/project-sprint/Entity.ProjectStage.md) entity.
- 2x **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **Audit**).

### Owned Entities Relationships

#### Project Stage

- **One-to-many** relationship with the [Project Task](../../domain/aggregates/Aggregate.ProjectTask.md) aggregate.
- 2x **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **Audit**).

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    User |o--o{ ProjectSprint : "CreatedById"
    User |o--o{ ProjectSprint : "UpdatedById"
    Project ||--o{ ProjectSprint : "ProjectId"
    User |o--o{ ProjectStage : "CreatedById"
    User |o--o{ ProjectStage : "UpdatedById"
    ProjectSprint ||--o{ ProjectStage : "SprintId"
    ProjectStage ||--o{ ProjectTask : "StageId"

    ProjectSprint {
        uniqueidentifier Id PK
        nvarchar(50) Name
        date StartDate "nullable"
        date EndDate "nullable"
        uniqueidentifier ProjectId FK
        uniqueidentifier CreatedById FK "nullable"
        datetime2 CreatedDateTime
        uniqueidentifier UpdatedById FK "nullable"
        datetime2 UpdatedDateTime
    }
    ProjectStage {
        uniqueidentifier Id PK
        nvarchar(50) Name
        int CustomOrderPosition
        uniqueidentifier SprintId FK
        uniqueidentifier CreatedById FK "nullable"
        datetime2 CreatedDateTime
        uniqueidentifier UpdatedById FK "nullable"
        datetime2 UpdatedDateTime
    }
    User {
        uniqueidentifier Id PK
    }
    Project {
        uniqueidentifier Id PK
    }
    ProjectTask {
        uniqueidentifier Id PK
    }
```

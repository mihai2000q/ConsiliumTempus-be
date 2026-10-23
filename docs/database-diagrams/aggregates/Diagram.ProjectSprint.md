# Database Diagrams

## Project Sprint

### Relationships

- **Many-to-one** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.
- **One-to-many** relationship with the [Project Stage](../../domain/entities/project-sprint/Entity.ProjectStage.md) entity.
- **One-to-one** relationship with the [Audit](../../domain/entities/Entity.Audit.md) entity.

### Owned Entities Relationships

#### Project Stage

- **One-to-many** relationship with the [Project Task](../../domain/aggregates/Aggregate.ProjectTask.md) aggregate.
- **One-to-one** relationship with the [Audit](../../domain/entities/Entity.Audit.md) entity.

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    Audit ||--o| ProjectSprint : "AuditId"
    Project ||--o{ ProjectSprint : "ProjectId"
    Audit ||--o| ProjectStage : "AuditId"
    ProjectSprint ||--o{ ProjectStage : "SprintId"
    ProjectStage ||--o{ ProjectTask : "StageId"

    ProjectSprint {
        uniqueidentifier Id PK
        nvarchar(50) Name
        date StartDate "nullable"
        date EndDate "nullable"
        uniqueidentifier ProjectId FK
        uniqueidentifier AuditId FK, UK
    }
    ProjectStage {
        uniqueidentifier Id PK
        nvarchar(50) Name
        int CustomOrderPosition
        uniqueidentifier SprintId FK
        uniqueidentifier AuditId FK, UK
    }
    Audit {
        uniqueidentifier Id PK
    }
    Project {
        uniqueidentifier Id PK
    }
    ProjectTask {
        uniqueidentifier Id PK
    }
```

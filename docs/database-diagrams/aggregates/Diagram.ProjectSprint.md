# Database Diagrams

## Project Sprint

### Relationships

- **Many-to-one** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.
- **One-to-many** relationship with the [Project Stage](../../domain/entities/project-sprint/Entity.ProjectStage.md) entity.
- **Many-to-one** relationship with the [Audit](../../domain/entities/Entity.Audit.md) entity.

### Owned Entities Relationships

#### Project Stage

- **One-to-many** relationship with the [Project Task](../../domain/aggregates/Aggregate.ProjectTask.md) aggregate.
- **Many-to-one** relationship with the [Audit](../../domain/entities/Entity.Audit.md) entity.

### Diagram

<img src="../../images/database-diagrams/aggregates/diagram.project-sprint.png" alt="Project Sprint Diagram" width="75%"/>

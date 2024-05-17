# Database Diagrams

## Project Task

### Relationships

- **Many-to-one** relationship with the [Project Stage](../../domain/entities/project-sprint/Entity.ProjectStage.md) entity.
- **One-to-Many** relationship with the [Project Task Comment](../../domain/entities/project-task/Entity.ProjectTaskComment.md) entity.
- Three **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.

### Diagram

<img src="../../images/domain/diagrams/aggregates/diagram.project-task.png" alt="Project Task Diagram" width="75%"/>

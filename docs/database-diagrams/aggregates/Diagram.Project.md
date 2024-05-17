# Database Diagrams

## Project

### Relationships

- **Many-to-one** relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate.
- **One-to-Many** relationship with the [Project Sprint](../../domain/aggregates/Aggregate.ProjectSprint.md) entity.
  - The Sprint has a **One-to-Many** relationship with the 
    [Project Stage](../../domain/entities/project-sprint/Entity.ProjectStage.md) entity.
    - The Section has a **One-to-Many** relationship with the
      [Project Task](../../aggregates/Aggregate.ProjectTask) aggregate.

### Diagram

<img src="../../images/domain/diagrams/aggregates/diagram.project.png" alt="Project Diagram" width="75%"/>

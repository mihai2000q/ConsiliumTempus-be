# Database Diagrams

## Workspace

### Relationships

- **Many-to-many** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
(through the **Many-to-one** relationship with the [Membership](../../domain/entities/Entity.Membership.md) entity).
- **Many-to-one** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.
- **One-to-Many** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.

### Diagram

<img src="../../images/domain/diagrams/aggregates/diagram.workspace.png" alt="Workspace Diagram" width="75%"/>

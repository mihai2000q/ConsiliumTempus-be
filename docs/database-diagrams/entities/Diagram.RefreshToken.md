# Database Diagrams

## Refresh Token

### Relationships

- **Many-to-One** relationship with the [User](../../domain/aggregates/Aggregate.User.md) entity.
- **One-to-Many** relationship with the [Refresh Token History](../../domain/entities/refresh-token/Entity.RefreshTokenHistory.md) entity.

### Diagram

<img src="../../images/database-diagrams/entities/diagram.refresh-token.png" alt="Refresh Token Diagram" width="75%"/>

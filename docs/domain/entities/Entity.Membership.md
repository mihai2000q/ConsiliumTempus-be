# Domain Entities

## Membership

This entity holds the relationship between the [User](../aggregates/Aggregate.User.md) 
and the [Workspace]((../aggregates/Aggregate.Workspace.md)). 
In addition, it also persists the timestamps and the role of the user inside the workspace.

```csharp
class Membership 
{
    Membership Create()
    void Update()
}
```

```json
{
  "user": {},
  "workspace": {},
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "workspaceRoleId": 2
}
```

For database design, check out the [Membership Diagram](../../database-diagrams/entities/Diagram.Membership.md).

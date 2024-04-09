# Domain Entities

## Membership

This entity holds the relationship between the [User](../aggregates/Aggregate.User.md) 
and the [Workspace]((../aggregates/Aggregate.Workspace.md)). 
In addition, it also persists the timestamps and the role of the user inside the workspace.

```csharp
class Membership 
{
    Membership Create()
    void UpdateWorkspaceRole()
}
```

```json
{
  "user": {},
  "workspace": {},
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "workspaceRole": {}
}
```

For database design, check out the [Membership Diagram](../diagrams/entities/Diagram.Membership.md).

## Domain Event

- **Membership Created** when the membership gets created, 
it will automatically attach the enum role to the Database Context (so that it does not throw a duplicate key error).
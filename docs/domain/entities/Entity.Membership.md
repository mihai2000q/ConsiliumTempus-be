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

### Filters

- **UserName** to filter by the name of the user
- **WorkspaceRoleId** to filter by the workspace role id
- **WorkspaceRoleName** to filter by the workspace role name

### Orders

- **UserEmail** to order by the email of the user
- **UserFirstName** to order by the first name of the user
- **UserLastName** to order by the last name of the user
- **UserName** to order by the name of the user
- **WorkspaceRoleId** to order by the workspace role id
- **WorkspaceRoleName** to order by the workspace role name
- **CreatedDateTime** to order by the creation date
- **UpdatedDateTime** to order by the last modified date
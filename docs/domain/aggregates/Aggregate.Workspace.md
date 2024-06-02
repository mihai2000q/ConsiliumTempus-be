# Domain Aggregates

## Workspace

This is the **workspace** of the application. It holds basic data like its name or the description.

```csharp
class Workspace
{
    Workspace Create()
    void Update()
    void AddUserMembership()
    void RefreshActivity()
    void TransferOwnership()
    void UpdateIsPersonal()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": { "value": "Workspace 1" },
  "description": { "value": "This is the description of the workspace" },
  "isPersonal": { "value": true },
  "isFavorite": { "value": true },
  "owner": {},
  "lastActivity": "2020-01-01T00:00:00.0000000Z",
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "memberships": [{}]
}
```

For database design checkout the [Workspace Diagram](../../database-diagrams/aggregates/Diagram.Workspace.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters
- The **Description** cannot be longer than 1000 characters

### Domain Errors

- **Not Found** when the workspace cannot be found
- **Personal Workspace** when the workspace is personal (used to restrict deletion)

### Value Objects

- **IsPersonal** which encapsulates a boolean *Value*, 
and it's used to determine whether the workspace is made to be the primary place for a user. 
(One user must have exactly one workspace where they are owners, and it is a personal workspace, 
otherwise they can be part of other people's personal workspaces, but they can't be owners).

### Filters

- **Name** to filter by name

### Orders

- **Name** to order alphabetically
- **LastActivity** to order by activity
- **UpdatedDateTime** to order by last modified date
- **CreatedDateTime** to order by creation time

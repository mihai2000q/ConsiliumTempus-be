# Domain Aggregates

## Workspace

This is the **workspace** of the application. It holds basic data like its name or the description.

```csharp
class Workspace
{
    Workspace Create()
    void Update()
    void AddUserMembership()
    void RefreshUpdatedDateTime()
    void TransferOwnership()
    void UpdateIsPersonal()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": { "value": "Workspace 1" },
  "description": { "value": "This is the description of the workspace" },
  "owner": {},
  "isPersonal": { "value": true },
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "memberships": [{}]
}
```

For database design checkout the [Workspace Diagram](../diagrams/aggregates/Diagram.Workspace.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters
- The **Description** cannot be longer than 1000 characters

### Domain Errors

- **Not Found** when the workspace cannot be found
- **Personal Workspace** when the workspace is personal

### Value Objects

- **IsPersonal** which encapsulates a boolean *Value*, 
and it's used to determine whether the workspace is made to be the primary place for a user. 
(One user must have exactly one workspace where they are owners, and it is a personal workspace, 
otherwise they can be part of other people's personal workspaces, but they can't be owners).

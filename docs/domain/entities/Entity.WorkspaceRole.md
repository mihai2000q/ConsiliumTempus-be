# Domain Entities

## Workspace Role

This entity derives from a smart enumeration, so it does not have any other functions.
Its only purpose is to serve as an Enum class for the three roles: *View*, *Member* and *Admin*. 

```csharp
class WorkspaceRole 
{
    WorkspaceRole View
    WorkspaceRole Member
    WorkspaceRole Admin
}
```

```json
{
  "id": 1,
  "name": "View",
  "description": "This role can only view new tasks"
}
```

For database design, check out the [Workspace Role Diagram](../diagrams/entities/Diagram.WorkspaceRole.md).
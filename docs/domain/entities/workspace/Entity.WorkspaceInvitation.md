# Domain Entities

## Workspace Invitation

This is the workspace **invitation** entity.

```csharp
class WorkspaceInvitation
{
    WorkspaceInvitation Create()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "sender": {},
  "collaborator": {},
  "workspace": {}
}
```

For database design checkout the [Workspace Diagram](../../../database-diagrams/aggregates/Diagram.Workspace.md).

### Domain Errors

- **Not Found** when the workspace invitation cannot be found
- **Already Invited** when the workspace invitation cannot be sent because the user has already been invited
- **Already Collaborator** when the workspace invitation cannot be sent because the user is already a collaborator
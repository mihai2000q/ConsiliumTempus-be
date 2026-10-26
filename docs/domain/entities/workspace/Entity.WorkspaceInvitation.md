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

- **Not Found** (`WorkspaceInvitation.NotFound`, 404) when the workspace invitation cannot be found
- **Already Invited** (`WorkspaceInvitation.AlreadyInvited`, 409) when the workspace invitation cannot be sent because the user has already been invited
- **Already Collaborator** (`WorkspaceInvitation.AlreadyCollaborator`, 409) when the workspace invitation cannot be sent or accepted because the user is already a collaborator
- **Not Invited User** (`WorkspaceInvitation.NotInvitedUser`, 403) when the invitation cannot be accepted or rejected because the current user is not the one it was sent to
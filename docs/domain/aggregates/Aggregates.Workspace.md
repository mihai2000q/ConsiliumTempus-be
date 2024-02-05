# Domain Aggregates

## User

This is the **workspace** of the application.

```csharp
class Workspace
{
    Workspace Create()
}
```

```json
{
  "id": {"value": "00000000-0000-0000-0000-000000000000"},
  "name": "Workspace 1",
  "description": "This is the description of the workspace",
  "users": [{}]
}
```

For database design checkout the [Workspace Diagram](../diagrams/Diagram.Workspace.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters
- The **Description** cannot be longer than 1000 characters

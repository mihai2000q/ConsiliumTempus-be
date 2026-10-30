# Domain Entities

## Project Task Comment

This is the project task **comment** entity.

```csharp
class ProjectTaskComment
{
    ProjectTaskComment Create()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "message": { "value": "The processes are fixed" },
  "date": "2020-01-01",
  "timeSpent": "08:30",
  "task": {},
  "authorship": {}
}
```

For database design checkout the [Project Task Diagram](../../../database-diagrams/aggregates/Diagram.ProjectTask.md).

### Properties Validation

- The **Message** cannot be longer than 256 characters

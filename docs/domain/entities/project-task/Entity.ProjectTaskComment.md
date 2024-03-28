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
  "createdBy": {},
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "date": "2020-01-01",
  "timeSpent": "08:30",
  "task": {}
}
```

For database design checkout the [Project Task Comment Diagram](../../diagrams/entities/project-task/Diagram.ProjectTaskComment.md).

### Properties Validation

- The **Message** cannot be longer than 256 characters

# Domain Entities

## Project Task

This is the project **task** entity.

```csharp
class ProjectTask
{
    ProjectTask Create()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": "Task 1",
  "description": "This is the description of the task",
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "assignee": {},
  "reviewer": {},
  "dueDate": "2020-01-01",
  "section": {}
}
```

For database design checkout the [Project Task Diagram](../../diagrams/entities/project/Diagram.ProjectTask.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters
- The **Description** cannot be longer than 1000 characters

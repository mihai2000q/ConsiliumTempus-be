# Domain Aggregates

## Project Task

This is the project **task** entity.

```csharp
class ProjectTask
{
    ProjectTask Create()
    void AddComment()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": { "value": "Task 1" },
  "description": { "value": "This is the description of the task" } ,
  "isCompleted": { "value": false },
  "createdBy": {},
  "order": { "value": 1 },
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "assignee": {},
  "reviewer": {},
  "dueDate": "2020-01-01",
  "estimatedDuration": "08:30",
  "section": {},
  "comments": [{}]
}
```

For database design checkout the [Project Task Diagram](../diagrams/aggregates/Diagram.ProjectTask.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters
- The **Description** cannot be longer than 1000 characters

### Entities

- [Project Task Comment](../entities/project-task/Entity.ProjectTaskComment.md)

### Value Objects

- **IsCompleted**, which encapsulates a boolean *Value*

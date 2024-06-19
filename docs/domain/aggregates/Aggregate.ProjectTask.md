# Domain Aggregates

## Project Task

This is the project **task** entity.

```csharp
class ProjectTask
{
    ProjectTask Create()
    void Update()
    void UpdateOverview()
    void UpdatetCustomOrderPosition()
    void AddComment()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": { "value": "Task 1" },
  "description": { "value": "This is the description of the task" } ,
  "customOrderPosition": { "value": 1 },
  "isCompleted": { "value": false },
  "createdBy": {},
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "assignee": {},
  "reviewer": {},
  "dueDate": "2020-01-01",
  "estimatedDuration": "08:30",
  "stage": {},
  "comments": [{}]
}
```

For database design checkout the [Project Task Diagram](../../database-diagrams/aggregates/Diagram.ProjectTask.md).

### Entities

- [Project Task Comment](../entities/project-task/Entity.ProjectTaskComment.md)

### Properties Validation

- The **Name** cannot be longer than 256 characters

### Domain Errors

- **Not Found** when the project cannot be found

### Value Objects

- **IsCompleted**, which encapsulates a boolean *Value*

### Filters

- **Name** to filter by name
- **IsCompleted** to filter by completed tasks

### Orders

- **Name** to order alphabetically
- **IsCompleted** to order by completed tasks
- **UpdatedDateTime** to order by last modified date
- **CreatedDateTime** to order by creation time
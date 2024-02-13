# Domain Aggregates

## Project

This is the **project** aggregate.

```csharp
class Project
{
    Project Create()
    void AddSprint()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": "Project 1",
  "description": "This is the description of the project",
  "isFavorite": false,
  "isPrivate": true,
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "workspace": {},
  "sprints": [{}]
}
```

For database design checkout the [Project Diagram](../diagrams/aggregates/Diagram.Project.md).

### Entities

- [Project Sprint](../entities/project/Entity.ProjectSprint.md)
- [Project Section](../entities/project/Entity.ProjectSection.md)
- [Project Task](../entities/project/Entity.ProjectTask.md)

### Properties Validation

- The **Name** cannot be longer than 100 characters
- The **Description** cannot be longer than 1000 characters

### Domain Errors

- **Not Found** when the project cannot be found

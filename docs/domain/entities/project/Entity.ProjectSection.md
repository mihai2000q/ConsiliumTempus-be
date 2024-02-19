# Domain Entities

## Project Section

This is the project **section** entity.

```csharp
class ProjectSection
{
    ProjectSection Create()
    void AddTask()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": "Section 1",
  "order": 0,
  "sprint": {},
  "tasks": [{}]
}
```

For database design checkout the [Project Section Diagram](../../diagrams/entities/project/Diagram.ProjectSection.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters

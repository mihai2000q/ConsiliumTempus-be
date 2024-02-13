# Domain Entities

## Project Sprint

This is the project **sprint** entity.

```csharp
class ProjectSprint
{
    ProjectSprint Create()
    void AddSection()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": "Sprint 1",
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "project": {},
  "sections": [{}]
}
```

For database design checkout the [Project Sprint Diagram](../../diagrams/entities/project/Diagram.ProjectSprint.md).

### Properties Validation

- The **Name** cannot be longer than 100 characters

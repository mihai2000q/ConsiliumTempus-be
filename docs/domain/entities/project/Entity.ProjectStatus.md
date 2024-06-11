# Domain Entities

## Project Status

This is the project **status** entity.

```csharp
class ProjectStatus
{
    ProjectStatus Create()
    void Update()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "title": { "value": "Status Update - 23 May" },
  "Status": "OnTrack",
  "Description": { "value": "Today we have had a successful meeting with one of the shareholders." },
  "project": {},
  "audit": {}
}
```

For database design checkout the [Project Diagram](../../../database-diagrams/aggregates/Diagram.Project.md).

### Properties Validation

- The **Title** cannot be longer than 100 characters

### Domain Errors

- **Not Found** when the project status cannot be found
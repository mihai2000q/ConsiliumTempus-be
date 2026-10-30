# Domain Entities

## Project Stage

This is the project **stage** entity.

```csharp
class ProjectStage
{
    ProjectStage Create()
    void Update()
    void UpdateCustomOrderPosition()
    void AddTask()
    void RemoveTask()
    ProjectStage CopyToSprint()
    void Move()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": { "value": "Stage 1" },
  "customOrderPosition": { "value": 0 },
  "sprint": {},
  "audit": {},
  "tasks": [{}]
}
```

For database design checkout the [Project Sprint Diagram](../../../database-diagrams/aggregates/Diagram.ProjectSprint.md).

### Properties Validation

- The **Name** cannot be longer than 50 characters

### Domain Errors

- **Not Found** (`ProjectStage.NotFound`, 404) when the project stage cannot be found
- **Only One Stage** (`ProjectStage.OnlyOneStage`, 409) when the project sprint only has one stage
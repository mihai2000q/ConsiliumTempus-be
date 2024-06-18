# Domain Entities

## Project Sprint

This is the project **sprint** entity.

```csharp
class ProjectSprint
{
    ProjectSprint Create()
    void Update()
    void UpdateEndDate()
    void AddStage()
    void AddStages()
    void RemoveStage()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": { "value": "Sprint 1" },
  "startDate": "2020-01-01",
  "endDate": "2022-12-12",
  "project": {},
  "audit": {},
  "stages": [{}]
}
```

For database design checkout the [Project Sprint Diagram](../../database-diagrams/aggregates/Diagram.ProjectSprint.md).

### Entities

- [Project Stage](../entities/project-sprint/Entity.ProjectStage.md)

### Properties Validation

- The **Name** cannot be longer than 100 characters

### Domain Errors

- **Not Found** when the project sprint cannot be found
- **Only One Sprint** when the project does not have more than one sprint
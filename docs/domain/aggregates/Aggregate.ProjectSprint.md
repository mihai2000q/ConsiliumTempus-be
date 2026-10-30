# Domain Aggregates

## Project Sprint

This is the project **sprint** aggregate.

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

- The **Name** cannot be longer than 50 characters

### Domain Errors

- **Not Found** (`ProjectSprint.NotFound`, 404) when the project sprint cannot be found
- **Only One Sprint** (`ProjectSprint.OnlyOneSprint`, 409) when the project has only one sprint, so it cannot be removed

### Filters

- **Name** to filter by name

Besides the filters that can be searched by (see [Api](../../api/Api.ProjectSprint.md#get-collection-project-sprint-request)),
the **From This Year** filter (`ProjectSprintFilter.FromThisYear`) is added by the application when the sprints of this year
are requested. It keeps the project sprints that **start** or **end** on or after the first day of the current year (UTC),
or that were **created** on or after it. Each of the three is enough for a sprint to be kept.
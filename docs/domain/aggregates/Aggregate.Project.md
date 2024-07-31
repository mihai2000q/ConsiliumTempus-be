
# Domain Aggregates

## Project

This is the **project** aggregate.

```csharp
class Project
{
    Project Create()
    void Update()
    void UpdateFavorites()
    void UpdatePrivacy()
    void UpdateOverview()
    void RefreshActivity()
    void AddSprint()
    void AddStatus()
    void RemoveStatus()
    void AddAllowedMember()
    void RemoveAllowedMember()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": "Project 1",
  "description": { "value": "This is the description of the project" },
  "isPrivate": { "value": true },
  "owner": {},
  "lifecycle": "Active",
  "lastActivity": "2020-01-01T00:00:00.0000000Z",
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "workspace": {},
  "sprints": [{}],
  "statuses": [{}],
  "favorites": [{}],
  "allowedMembers": [{}]
}
```

For database design checkout the [Project Diagram](../../database-diagrams/aggregates/Diagram.Project.md).

### Entities

- [Project Status](../entities/project/Entity.ProjectStatus.md)

### Properties Validation

- The **Name** cannot be longer than 100 characters

### Domain Errors

- **Not Found** when the project cannot be found
- **Allowed Member Not Found** when the allowed member cannot be found
- **Not Private** when the project is not private
- **Already Allowed Member** when the user is already an allowed member of the project
- **Remove Yourself** when the user tries to remove himself from the allowed members of the project

### Domain Events

- **Project Created** when the project gets created, it will also create sprints, stages and tasks as examples for user. 

### Filters

- **Name** to filter by name
- **IsPrivate** to filter by accessibility
- **Lifecycle** to filter by lifecycle
- **LatestStatus** to filter by the latest status
- **LastActivity** to filter by activity
- **CreatedDateTime** to filter by created date time
- **UpdatedDateTime** to filter by updated date time

### Orders

- **Name** to order alphabetically
- **LastActivity** to order by activity
- **UpdatedDateTime** to order by last modified date
- **CreatedDateTime** to order by creation time

### Enums

- **ProjectLifecycle** to represent the current lifecycle of the project (i.e., active or archived)
- **ProjectStatusType** to represent the current status of the project (i.e., off track or completed)
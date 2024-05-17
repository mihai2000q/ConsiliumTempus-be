# Domain Entities

## Permission

This entity is used only to hold the values of the [Permissions Enum](../../Domain.md/#enums) 
inside the database and do the mapping with the [Workspace Role](Entity.WorkspaceRole.md) 
inside the [Workspace Role Has Permission](../../Domain.md/#relations) relation entity.

```csharp
class Permission 
{
    Permission Create()
}
```

```json
{
  "id": 1,
  "name": "ReadWorkspace"
}
```

For database design, check out the [Permission Diagram](../../database-diagrams/entities/Diagram.Permission.md).
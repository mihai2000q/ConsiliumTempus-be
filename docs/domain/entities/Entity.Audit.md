# Domain Entities

## Audit

This entity takes care of holding data about the creation and update of other entities.

```csharp
class Audit 
{
    Audit Create()
    void Update()
    void Nullify()
}
```

```json
{
  "createdBy": {},
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedBy": {},
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z"
}
```

For database design, check out the [Audit Diagram](../../database-diagrams/entities/Diagram.Audit.md).

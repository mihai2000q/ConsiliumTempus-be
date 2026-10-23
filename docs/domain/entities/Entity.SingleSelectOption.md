# Domain Entities

## Single Select Option

This entity represents an available option inside a single select custom field setup 
or the selected option on a single select custom field.

```csharp
class SingleSelectOption 
{
    SingleSelectOption Create()
    void Update()
    void UpdateCustomOrderPosition()
}
```

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "color": "#FF1122",
  "value": "High",
  "customOrderPosition": { "value": 0 }
}
```

For database design, check out the [Single Select Option Diagram](../../database-diagrams/entities/Diagram.SingleSelectOption.md).

### Properties Validation

- The **Value** cannot be longer than 50 characters 

### Domain Errors

- **Not Found** (`SingleSelectOption.NotFound`, 404) when the single select option is not found
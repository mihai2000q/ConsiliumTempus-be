# Domain Entities

## Multi Select Option

This entity represents an available option inside a multi select custom field setup 
or one of the selected options on a multi select custom field.

```csharp
class MultiSelectOption 
{
    MultiSelectOption Create()
    void Update()
    void UpdateCustomOrderPosition()
}
```

```json
{
  "color": "#FF1122",
  "value": "High",
  "customOrderPosition": 0
}
```

For database design, check out the [Multi Select Option Diagram](../../database-diagrams/entities/Diagram.MultiSelectOption.md).

### Properties Validation

- The **Value** cannot be longer than 50 characters

### Domain Errors

- **Not Found** when the multi select option is not found
# Domain Entities

## Custom Field

This is the **custom field** entity.

```csharp
class CustomField
{
    CustomField Create()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "task": {},
  "setup": {}
}
```

For database design checkout the [Project Task Diagram](../../../database-diagrams/aggregates/Diagram.ProjectTask.md).

### Variants

All the below represent entities that are derived from the Custom Field.

#### Date

```csharp
class DateCustomField : CustomField
{
    void Update()
}
```

```json
{
  "date": "2022-12-25"
}
```

#### Date Time

```csharp
class DateTimeCustomField : CustomField
{
    void Update()
}
```

```json
{
  "dateTime": "2020-01-01T00:00:00.0000000Z"
}
```

#### Duration

```csharp
class DurationCustomField : CustomField
{
    void Update()
}
```

```json
{
  "duration": "00:23:12"
}
```

#### Multi Select

```csharp
class MultiSelectCustomField : CustomField
{
    void AddOption()
    void RemoveOption()
}
```

```json
{
  "options": [{}]
}
```

#### Number

```csharp
class NumberCustomField : CustomField
{
    void Update()
}
```

```json
{
  "number": 123
}
```

#### People

```csharp
class PeopleCustomField : CustomField
{
    void Update()
}
```

```json
{
  "person": {}
}
```

#### Single Select

```csharp
class SingleSelectCustomField : CustomField
{
    void Update()
}
```

```json
{
  "option": {}
}
```

#### Text

```csharp
class TextCustomField : CustomField
{
    void Update()
}
```

```json
{
  "text": "some text"
}
```

#### Time

```csharp
class TimeCustomField : CustomField
{
    void Update()
}
```

```json
{
  "time": "23:55"
}
```

# Domain Aggregates

## Custom Field Setup

This is the **custom field setup** aggregate. 
It is used to introduce new custom fields on projects or workspaces and to define their structure, how they should look.
Usually, it either is initialized in a workspace and then multiple projects, or just a project.

```csharp
class CustomFieldSetup
{
    void Update()
    void UpdateWorkspace()
    void AddProject()
    void RemoveProject()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000" },
  "name": "First Custom Field Setup",
  "description": { "value": "This is the description of the setup" },
  "workspace": null,
  "projects": [{}],
  "audit": {}
}
```

For database design checkout the [Custom Field Setup Diagram](../../database-diagrams/aggregates/Diagram.CustomFieldSetup.md).

### Properties Validation

- The **Name** cannot be longer than 50 characters

### Domain Errors

- **Not Found** when the custom field setup cannot be found
- **Not Global** when the custom field setup is not globally available in the workspace 
(i.e., it is available in just one project)
- **Already Global** when the custom field setup is already global
- **Project Already Present** when the custom field setup already has the project

### Domain Events

- **Added Custom Field Setup To Project**, when the custom field setup is added to a project (or created on one)
it will initialize all the project tasks in the project with the corresponding custom field and default value
- **Removed Custom Field Setup From Project** when the custom field setup is removed from a project,
then delete all the corresponding custom fields from the tasks in the project

### Value Objects

- **Number Custom Field Settings** used to encapsulate settings for the number custom field setup,
such as the number of decimals after the floating point, or if the number should be rounded, etc.

### Variants

All the below represent entities that are derived from the Custom Field Setup.

#### Date

```csharp
class DateCustomFieldSetup : CustomFieldSetup
{
    DateCustomFieldSetup Create()
    void Update()
}
```

```json
{
  "defaultDate": "2022-12-25"
}
```

#### Date Time

```csharp
class DateTimeCustomFieldSetup : CustomFieldSetup
{
    DateTimeCustomFieldSetup Create()
    void Update()
}
```

```json
{
  "defaultDateTime": "2020-01-01T00:00:00.0000000Z"
}
```

#### Duration

```csharp
class DurationCustomFieldSetup : CustomFieldSetup
{
    DurationCustomFieldSetup Create()
    void Update()
}
```

```json
{
  "defaultDuration": "00:23:12"
}
```

#### Multi Select

```csharp
class MultiSelectCustomFieldSetup : CustomFieldSetup
{
    MultiSelectCustomFieldSetup Create()
    void Update()
    void AddOption()
    void RemoveOption()
    void MoveOption()
}
```

```json
{
  "options": [{}]
}
```

#### Number

```csharp
class NumberCustomFieldSetup : CustomFieldSetup
{
    NumberCustomFieldSetup Create()
    void Update()
}
```

```json
{
  "settings": {},
  "defaultNumber": 0
}
```

##### Properties Validation

- The **Decimals** cannot be bigger than 9

#### People

```csharp
class PeopleCustomFieldSetup : CustomFieldSetup
{
    PeopleCustomFieldSetup Create()
    void Update()
}
```

#### Single Select

```csharp
class SingleSelectCustomFieldSetup : CustomFieldSetup
{
    SingleSelectCustomFieldSetup Create()
    void Update()
    void AddOption()
    void RemoveOption()
    void MoveOption()
}
```

```json
{
  "options": [{}],
  "defaultOption": {} 
}
```

#### Text

```csharp
class TextCustomFieldSetup : CustomFieldSetup
{
    TextCustomFieldSetup Create()
    void Update()
}
```

```json
{
  "defaultText": "default"
}
```

#### Time

```csharp
class TimeCustomFieldSetup : CustomFieldSetup
{
    TimeCustomFieldSetup Create()
    void Update()
}
```

```json
{
  "defaultTime": "23:55"
}
```
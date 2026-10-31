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
  "name": { "value": "First Custom Field Setup" },
  "description": { "value": "This is the description of the setup" },
  "workspace": null,
  "projects": [{}],
  "audit": {}
}
```

For database design checkout the [Custom Field Setup Diagram](../../database-diagrams/aggregates/Diagram.CustomFieldSetup.md).

### Global and Specific

A custom field setup is in one of two states:

- **Global**, when it belongs to a workspace (the *workspace* is set).
  It is available to all the projects of the workspace, and it can be added to or removed from them,
  so the *projects* are the ones that currently use it, if any
- **Specific**, when it belongs to a single project (the *workspace* is null),
  which is the only one in the *projects*

The aggregate tells its state with `IsGlobal`, and answers with `OwningWorkspace` (its workspace,
or the workspace of its project when it is specific) and `OwningProject` (the project that a specific setup belongs to,
and none when it is global). They are not persisted, they come from the *workspace* and the *projects*,
so the rule is not repeated by the ones that need it (i.e., the authorization, to find the workspace and the project
of a setup).

A specific custom field setup becomes global by being made global, which gives it the workspace of its project
(it keeps the project).

### Properties Validation

- The **Name** cannot be longer than 50 characters

### Domain Errors

- **Not Found** (`CustomFieldSetup.NotFound`, 404) when the custom field setup cannot be found
- **Not Global** (`CustomFieldSetup.NotGlobal`, 409) when the custom field setup is not globally available in the workspace 
(i.e., it is available in just one project)
- **Already Global** (`CustomFieldSetup.AlreadyGlobal`, 409) when the custom field setup is already global
- **Project Already Present** (`CustomFieldSetup.ProjectAlreadyPresent`, 409) when the custom field setup already has the project
- **Type Mismatch** (`CustomFieldSetup.TypeMismatch`, 409) when the type of an update does not match the type of the custom field setup

### Domain Events

- **Added Custom Field Setup To Project**, when the custom field setup is added to a project (or created on one)
it will initialize all the project tasks in the project with the corresponding custom field and default value
- **Removed Custom Field Setup From Project** when the custom field setup is removed from a project,
then delete all the corresponding custom fields from the tasks in the project
- **Removed Option From Multi Select Custom Field Setup** when an option is removed from a custom field setup,
then clean up the corresponding multi select custom fields of the option
- **Removed Option From Single Select Custom Field Setup** when an option is removed from a custom field setup,
then clear the option of the corresponding single select custom fields
(the default option of the custom field setup is cleared too, if it was the removed one)

### Value Objects

- **Number Custom Field Settings** used to encapsulate settings for the number custom field setup,
such as the number of decimals after the floating point, or if the number should be rounded, etc.

### Variants

All the below represent entities that are derived from the Custom Field Setup.
Each of them says which type it is through `Type` (the *CustomFieldType*), so the rest of the application
does not have to look at its class to know it.

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
  "defaultNumber": { "value": 0 }
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
  "defaultOptionId": "00000000-0000-0000-0000-000000000000"
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
  "defaultText": { "value": "default" }
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
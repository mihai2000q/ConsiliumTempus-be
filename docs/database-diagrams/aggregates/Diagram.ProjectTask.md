# Database Diagrams

## Project Task

### Relationships

- **Many-to-one** relationship with the [Project Stage](../../domain/entities/project-sprint/Entity.ProjectStage.md) entity.
- **One-to-Many** relationship with the [Custom Field](../../domain/entities/project-task/Entity.CustomField.md) entity.
- **One-to-Many** relationship with the [Project Task Comment](../../domain/entities/project-task/Entity.ProjectTaskComment.md) entity.
- Four **Many-to-one** relationships with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(two of them through the **Audit**).

### Owned Entities Relationships

#### Project Task Comment

- **Many-to-one** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate
(through the **Authorship**).

### Custom Field Relationships

- **Many-to-one** relationship with the [Custom Field Setup](../../domain/aggregates/Aggregate.CustomFieldSetup.md) aggregate.

##### Variants

###### Multi Select

- **Many-to-Many** relationship with the [Multi Select Option](../../domain/entities/Entity.MultiSelectOption.md) entity
(via the *CustomField_MultiSelectHasOption* entity).

###### People

- **Many-to-One** relationship with the [User](../../domain/aggregates/Aggregate.User.md) aggregate.

###### Single Select

- **Many-to-One** (optional) relationship with the [Single Select Option](../../domain/entities/Entity.SingleSelectOption.md) entity.

### Diagram

For the notation used, check out the [Legend](../Database.Diagram.md#legend).

```mermaid
erDiagram
    ProjectStage ||--o{ ProjectTask : "StageId"
    User |o--o{ ProjectTask : "AssigneeId"
    User |o--o{ ProjectTask : "CreatedById"
    User |o--o{ ProjectTask : "UpdatedById"
    User |o--o{ ProjectTask : "ReviewerId"
    CustomFieldSetup_Date ||--o{ CustomField_Date : "SetupId"
    ProjectTask ||--o{ CustomField_Date : "ProjectTaskId"
    CustomFieldSetup_DateTime ||--o{ CustomField_DateTime : "SetupId"
    ProjectTask ||--o{ CustomField_DateTime : "ProjectTaskId"
    CustomFieldSetup_Duration ||--o{ CustomField_Duration : "SetupId"
    ProjectTask ||--o{ CustomField_Duration : "ProjectTaskId"
    CustomFieldSetup_MultiSelect ||--o{ CustomField_MultiSelect : "SetupId"
    ProjectTask ||--o{ CustomField_MultiSelect : "ProjectTaskId"
    CustomFieldSetup_Number ||--o{ CustomField_Number : "SetupId"
    ProjectTask ||--o{ CustomField_Number : "ProjectTaskId"
    CustomFieldSetup_People ||--o{ CustomField_People : "SetupId"
    ProjectTask ||--o{ CustomField_People : "ProjectTaskId"
    User |o--o{ CustomField_People : "PersonId"
    CustomFieldSetup_SingleSelect ||--o{ CustomField_SingleSelect : "SetupId"
    ProjectTask ||--o{ CustomField_SingleSelect : "ProjectTaskId"
    SingleSelectOption |o--o{ CustomField_SingleSelect : "OptionId"
    CustomFieldSetup_Text ||--o{ CustomField_Text : "SetupId"
    ProjectTask ||--o{ CustomField_Text : "ProjectTaskId"
    CustomFieldSetup_Time ||--o{ CustomField_Time : "SetupId"
    ProjectTask ||--o{ CustomField_Time : "ProjectTaskId"
    ProjectTask ||--o{ ProjectTaskComment : "TaskId"
    User |o--o{ ProjectTaskComment : "AuthorId"
    CustomField_MultiSelect ||--o{ CustomField_MultiSelectHasOption : "CustomFieldId"
    MultiSelectOption ||--o{ CustomField_MultiSelectHasOption : "OptionId"

    ProjectTask {
        uniqueidentifier Id PK
        nvarchar(256) Name
        nvarchar(max) Description
        int CustomOrderPosition
        bit IsCompleted
        datetime2 CompletedOn "nullable"
        uniqueidentifier AssigneeId FK "nullable"
        uniqueidentifier ReviewerId FK "nullable"
        date DueDate "nullable"
        time EstimatedDuration "nullable"
        uniqueidentifier StageId FK
        uniqueidentifier CreatedById FK "nullable"
        datetime2 CreatedDateTime
        uniqueidentifier UpdatedById FK "nullable"
        datetime2 UpdatedDateTime
    }
    CustomField_Date {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        date Date "nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_DateTime {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        datetime2 DateTime "nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_Duration {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        bigint Duration "nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_MultiSelect {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        uniqueidentifier SetupId FK
    }
    CustomField_Number {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        decimal Number "precision 38, scale 9, nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_People {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        uniqueidentifier PersonId FK "nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_SingleSelect {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        uniqueidentifier OptionId FK "nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_Text {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        nvarchar(max) Text "nullable"
        uniqueidentifier SetupId FK
    }
    CustomField_Time {
        uniqueidentifier Id PK
        uniqueidentifier ProjectTaskId FK
        time Time "nullable"
        uniqueidentifier SetupId FK
    }
    ProjectTaskComment {
        uniqueidentifier Id PK
        nvarchar(256) Message
        uniqueidentifier AuthorId FK "nullable"
        date Date "nullable"
        time TimeSpent "nullable"
        uniqueidentifier TaskId FK
        datetime2 CreatedDateTime
        datetime2 UpdatedDateTime
    }
    CustomField_MultiSelectHasOption {
        uniqueidentifier OptionId PK, FK
        uniqueidentifier CustomFieldId PK, FK
    }
    User {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Date {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_DateTime {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Duration {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_MultiSelect {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Number {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_People {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_SingleSelect {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Text {
        uniqueidentifier Id PK
    }
    CustomFieldSetup_Time {
        uniqueidentifier Id PK
    }
    MultiSelectOption {
        uniqueidentifier Id PK
    }
    SingleSelectOption {
        uniqueidentifier Id PK
    }
    ProjectStage {
        uniqueidentifier Id PK
    }
```

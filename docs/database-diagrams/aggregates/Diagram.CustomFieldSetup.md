# Database Diagrams

## Custom Field Setup

### Relationships

- **One-to-One** relationship with the [Audit](../../domain/entities/Entity.Audit.md) entity.
- **Many-to-One** (optional) relationship with the [Workspace](../../domain/aggregates/Aggregate.Workspace.md) aggregate.
- **Many-to-Many** relationship with the [Project](../../domain/aggregates/Aggregate.Project.md) aggregate.
(via the *ProjectHasCustomFieldSetup* entity)
- **One-to-Many** relationship with the [Custom Field](../../domain/entities/project-task/Entity.CustomField.md) entity.

#### Variants

##### Multi Select

- **One-to-Many** relationship with the [Multi Select Option](../../domain/entities/Entity.MultiSelectOption.md) entity.

##### Single Select

- **One-to-Many** relationship with the [Single Select Option](../../domain/entities/Entity.SingleSelectOption.md) entity.

### Diagram

```mermaid
erDiagram
    Audit ||--o| CustomFieldSetup_Date : "AuditId"
    Workspace |o--o{ CustomFieldSetup_Date : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_DateTime : "AuditId"
    Workspace |o--o{ CustomFieldSetup_DateTime : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_Duration : "AuditId"
    Workspace |o--o{ CustomFieldSetup_Duration : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_MultiSelect : "AuditId"
    Workspace |o--o{ CustomFieldSetup_MultiSelect : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_Number : "AuditId"
    Workspace |o--o{ CustomFieldSetup_Number : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_People : "AuditId"
    Workspace |o--o{ CustomFieldSetup_People : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_SingleSelect : "AuditId"
    Workspace |o--o{ CustomFieldSetup_SingleSelect : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_Text : "AuditId"
    Workspace |o--o{ CustomFieldSetup_Text : "WorkspaceId"
    Audit ||--o| CustomFieldSetup_Time : "AuditId"
    Workspace |o--o{ CustomFieldSetup_Time : "WorkspaceId"
    CustomFieldSetup_MultiSelect ||--o{ MultiSelectOption : "SetupId"
    CustomFieldSetup_SingleSelect ||--o{ SingleSelectOption : "SetupId"
    Project ||--o{ ProjectHasCustomFieldSetup : "ProjectsId"
    CustomFieldSetup_Date ||--o{ CustomField_Date : "SetupId"
    CustomFieldSetup_DateTime ||--o{ CustomField_DateTime : "SetupId"
    CustomFieldSetup_Duration ||--o{ CustomField_Duration : "SetupId"
    CustomFieldSetup_MultiSelect ||--o{ CustomField_MultiSelect : "SetupId"
    CustomFieldSetup_Number ||--o{ CustomField_Number : "SetupId"
    CustomFieldSetup_People ||--o{ CustomField_People : "SetupId"
    CustomFieldSetup_SingleSelect ||--o{ CustomField_SingleSelect : "SetupId"
    SingleSelectOption |o--o{ CustomField_SingleSelect : "OptionId"
    CustomFieldSetup_Text ||--o{ CustomField_Text : "SetupId"
    CustomFieldSetup_Time ||--o{ CustomField_Time : "SetupId"
    MultiSelectOption ||--o{ CustomField_MultiSelectHasOption : "OptionId"

    CustomFieldSetup_Date {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        date DefaultDate "nullable"
    }
    CustomFieldSetup_DateTime {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        datetime2 DefaultDateTime "nullable"
    }
    CustomFieldSetup_Duration {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        bigint DefaultDuration "nullable"
    }
    CustomFieldSetup_MultiSelect {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
    }
    CustomFieldSetup_Number {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        nvarchar(3) CurrencyCode "nullable"
        smallint Decimals
        bit Rounding
        decimal DefaultNumber "precision 38, scale 9, nullable"
    }
    CustomFieldSetup_People {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
    }
    CustomFieldSetup_SingleSelect {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        uniqueidentifier DefaultOptionId "nullable"
    }
    CustomFieldSetup_Text {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        nvarchar(max) DefaultText "nullable"
    }
    CustomFieldSetup_Time {
        uniqueidentifier Id PK
        nvarchar(50) Name
        nvarchar(max) Description
        uniqueidentifier WorkspaceId FK "nullable"
        uniqueidentifier AuditId FK, UK
        time DefaultTime "nullable"
    }
    MultiSelectOption {
        uniqueidentifier Id PK
        nvarchar(50) Value
        nvarchar(7) Color
        int CustomOrderPosition
        uniqueidentifier SetupId FK
    }
    SingleSelectOption {
        uniqueidentifier Id PK
        nvarchar(50) Value
        nvarchar(7) Color
        int CustomOrderPosition
        uniqueidentifier SetupId FK
    }
    ProjectHasCustomFieldSetup {
        uniqueidentifier CustomFieldSetupsId PK "references any CustomFieldSetup_* table"
        uniqueidentifier ProjectsId PK, FK
    }
    Audit {
        uniqueidentifier Id PK
    }
    Workspace {
        uniqueidentifier Id PK
    }
    Project {
        uniqueidentifier Id PK
    }
    CustomField_Date {
        uniqueidentifier Id PK
    }
    CustomField_DateTime {
        uniqueidentifier Id PK
    }
    CustomField_Duration {
        uniqueidentifier Id PK
    }
    CustomField_MultiSelect {
        uniqueidentifier Id PK
    }
    CustomField_Number {
        uniqueidentifier Id PK
    }
    CustomField_People {
        uniqueidentifier Id PK
    }
    CustomField_SingleSelect {
        uniqueidentifier Id PK
    }
    CustomField_Text {
        uniqueidentifier Id PK
    }
    CustomField_Time {
        uniqueidentifier Id PK
    }
    CustomField_MultiSelectHasOption {
        uniqueidentifier OptionId PK, FK
        uniqueidentifier CustomFieldId PK, FK
    }
```

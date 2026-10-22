# Database Diagrams

## Multi Select Option

### Relationships

- **Many-to-Many** relationship with the [Custom Field Multi Select](../../domain/entities/project-task/Entity.CustomField.md) entity
(via the *CustomField_MultiSelectHasOption* entity).
- **Many-to-One** relationship with the [Custom Field Setup Multi Select](../../domain/aggregates/Aggregate.CustomFieldSetup.md) aggregate.

### Diagram

```mermaid
erDiagram
    CustomFieldSetup_MultiSelect ||--o{ MultiSelectOption : "SetupId"
    CustomField_MultiSelect ||--o{ CustomField_MultiSelectHasOption : "CustomFieldId"
    MultiSelectOption ||--o{ CustomField_MultiSelectHasOption : "OptionId"

    MultiSelectOption {
        uniqueidentifier Id PK
        nvarchar(50) Value
        nvarchar(7) Color
        int CustomOrderPosition
        uniqueidentifier SetupId FK
    }
    CustomField_MultiSelectHasOption {
        uniqueidentifier OptionId PK, FK
        uniqueidentifier CustomFieldId PK, FK
    }
    CustomFieldSetup_MultiSelect {
        uniqueidentifier Id PK
    }
    CustomField_MultiSelect {
        uniqueidentifier Id PK
    }
```

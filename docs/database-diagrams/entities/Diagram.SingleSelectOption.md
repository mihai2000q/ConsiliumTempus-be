# Database Diagrams

## Single Select Option

### Relationships

- **One-to-Many** relationship with the [Custom Field Single Select](../../domain/entities/project-task/Entity.CustomField.md) entity.
- **Many-to-One** relationship with the [Custom Field Setup Single Select](../../domain/aggregates/Aggregate.CustomFieldSetup.md) aggregate.

### Diagram

```mermaid
erDiagram
    CustomFieldSetup_SingleSelect ||--o{ SingleSelectOption : "SetupId"
    SingleSelectOption |o--o{ CustomField_SingleSelect : "OptionId"

    SingleSelectOption {
        uniqueidentifier Id PK
        nvarchar(50) Value
        nvarchar(7) Color
        int CustomOrderPosition
        uniqueidentifier SetupId FK
    }
    CustomFieldSetup_SingleSelect {
        uniqueidentifier Id PK
    }
    CustomField_SingleSelect {
        uniqueidentifier Id PK
    }
```

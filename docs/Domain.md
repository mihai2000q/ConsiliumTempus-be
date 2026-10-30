# Consilium Tempus Backend Domain Models

* [Consilium Tempus Backend Domain Models](#consilium-tempus-backend-domain-models)
  * [General](#general)
  * [Aggregates](#aggregates)
  * [Entities](#entities)
  * [Value Objects](#value-objects)
  * [Domain Errors](#domain-errors)
  * [Enums](#enums)
  * [Relations](#relations)
  * [Database Diagrams](#database-diagrams)

Below, you will find a complete list and documentation for each **Aggregate**, **Entity** and **Value Object**.
<br>
Additionally, on the bottom of the page there can be found [Database Diagrams](#database-diagrams) 
representing the relationships of the aggregates with other aggregates and/or entities.
For a business-level summary of how these models relate, check out the [Overview](Overview.md).

## General

Each aggregate, entity or value object has an empty private constructor
so that the Entity Framework Core can initialize them.

The JSON examples inside the documentation of each aggregate and entity show the domain model,
i.e., a value object appears as `{ "value": ... }`. They are not the requests or the responses of the Api
(for those, check out the [Api documentation](Api.md)).

The domain errors mention their code, and the status code that the Api responds with
(see [Errors](Api.md#errors)).

## Aggregates

All Aggregates inherit from the **Aggregate Root** class, which requires a unique class to be the Id.
This Id has to inherit from the **Aggregate Root Id**, which is a value object
(they won't be included in the value object lists as they are required and part of all the aggregates).
**Aggregate Root** is also essentially an **Entity** at its roots,
however, an **Aggregate** will usually be a model that contains all the relations of multiple entities.

Each aggregate can have its own set of **entities** or **value objects**.

Each aggregate has a set of possible *errors*, *filters*, 
*filter properties*, *orders*, *order properties* or *properties validation*.
The aforementioned refers to the constraints that each property has
(i.e., the name cannot be longer than 100 characters).

To allow a new order or filter for a property for an entity that already supports it, add a new filter/order property.

The aggregates are:

- [Custom Field Setup Aggregate](domain/aggregates/Aggregate.CustomFieldSetup.md)
- [Project Aggregate](domain/aggregates/Aggregate.Project.md)
- [Project Sprint Aggregate](domain/aggregates/Aggregate.ProjectSprint.md)
- [Project Task Aggregate](domain/aggregates/Aggregate.ProjectTask.md)
- [User Aggregate](domain/aggregates/Aggregate.User.md)
- [Workspace Aggregate](domain/aggregates/Aggregate.Workspace.md)

## Entities

Essentially, an entity holds data and an Id, and by definition, two entities are equal only if their Ids are equal.
All entities can hold domain events (including aggregates).

Typically, the entities will be persisted in the database on another table.

The entities are:

- [Custom Field](domain/entities/project-task/Entity.CustomField.md)
- [Membership](domain/entities/Entity.Membership.md)
- [Multi Select Option](domain/entities/Entity.MultiSelectOption.md)
- [Permission](domain/entities/Entity.Permission.md)
- [Project Stage](domain/entities/project-sprint/Entity.ProjectStage.md)
- [Project Status](domain/entities/project/Entity.ProjectStatus.md)
- [Project Task Comment](domain/entities/project-task/Entity.ProjectTaskComment.md)
- [Refresh Token](domain/entities/Entity.RefreshToken.md)
- [Refresh Token History](domain/entities/refresh-token/Entity.RefreshTokenHistory.md)
- [Single Select Option](domain/entities/Entity.SingleSelectOption.md)
- [Workspace Invitation](domain/entities/workspace/Entity.WorkspaceInvitation.md)
- [Workspace Role](domain/entities/Entity.WorkspaceRole.md)

## Value Objects

Essentially, a value object holds only values and, by definition,
two value objects are equal only if all the values are equal with one another.

Typically, the value objects will be flattened on the object before persisting in the database.

The shared value objects are the following (the ones that are specific to an aggregate or an entity,
such as *IsCompleted* or *FirstName*, are described in their own documentation):

- **Audit**, which holds who created and who updated something, and when
  (*CreatedBy*, *CreatedDateTime*, *UpdatedBy* and *UpdatedDateTime*).
  It is a part of the entities that implement `IAudit` (the custom field setups, the memberships, the projects, the project sprints,
  the project stages, the project statuses, the project tasks and the workspaces), and the domain never creates nor updates it:
  it is stamped by the `StampInterceptor` when the changes are saved
  (see [Audit](Database.md#audit))
- **Authorship**, which holds who wrote something, and when it was written and last edited
  (*Author*, *CreatedDateTime* and *UpdatedDateTime*), for what only its author can edit.
  It is a part of the entities that implement `IAuthored` (the project task comments), and the domain never creates nor updates it
  either: it is stamped by the `StampInterceptor` when the changes are saved
  (see [Authorship](Database.md#authorship))
- Authentication
  - **IsInvalidated**, which encapsulates a boolean *Value*
  - **JwtId**, which encapsulates a Guid *Value*
- **CustomOrderPosition**, which encapsulates an integer *Value,* and it's used to achieve custom ordering
- **DecimalNumber**, which encapsulates a decimal number *Value*
- **Description**, which encapsulates a string *Value*
- **IsPrivate**, which encapsulates a boolean *Value*
- **Message**, which encapsulates a string *Value*
- **Name**, which encapsulates a string *Value*
- **Title**, which encapsulates a string *Value*
- **Text**, which encapsulates a string *Value*

## Domain Errors

These errors are not tied to any aggregate, therefore notable and only used in the Application Layer.

- Authentication
  - **Invalid Credentials** (`Authentication.InvalidCredentials`, 401) when, upon authentication, the credentials are invalid
  - **Invalid Tokens** (`Authentication.InvalidTokens`, 401) when, upon refreshing the token, either the access or refresh token is invalid

## Enums

The shared enums defined in the Domain of the application are the following
(the ones that are specific to an aggregate, such as the project lifecycle, are described in their own documentation):

- **CustomFieldType**, which is used to represent the type of the custom field or its setup
- **FilterOperator**, which is used to represent the different operators that can be used for filtering 
(i.e., equal or less than)
- **OrderType**, which is used to determine whether the order should be ascending or descending
- **Permissions**, which are used on the Presentation Layer for Authorization
- **Project Authorization Level**, which is used on the Presentation Layer for Project Authorization
- **Workspace Authorization Level**, which is used on the Presentation Layer for Workspace Authorization

## Relations

The classes inside the Relations package contain the entities used to create the joint tables between other entities.
<br>
Those include:

- **CustomFieldMultiSelectHasOption**
- **ProjectHasAllowedMember**
- **ProjectHasCustomFieldSetup**
- **UserHasFavoriteProject**
- **UserHasFavoriteWorkspace**
- **WorkspaceRoleHasPermission**

## Database Diagrams

The database diagrams show the relations that the above entities have inside the database.
For a full picture of the database diagram, check this out [Database Diagram](database-diagrams/Database.Diagram.md).
<br>
For individual diagrams (they exclude additional info about external entities, 
except the primary key and direct relations), check below:

- Aggregates
  - [Custom Field Setup Diagram](database-diagrams/aggregates/Diagram.CustomFieldSetup.md)
  - [Project Diagram](database-diagrams/aggregates/Diagram.Project.md)
  - [Project Sprint Diagram](database-diagrams/aggregates/Diagram.ProjectSprint.md)
  - [Project Task Diagram](database-diagrams/aggregates/Diagram.ProjectTask.md)
  - [User Diagram](database-diagrams/aggregates/Diagram.User.md)
  - [Workspace Diagram](database-diagrams/aggregates/Diagram.Workspace.md)
- Entities
  - [Membership Diagram](database-diagrams/entities/Diagram.Membership.md)
  - [Multi Select Option Diagram](database-diagrams/entities/Diagram.MultiSelectOption.md)
  - [Permission Diagram](database-diagrams/entities/Diagram.Permission.md)
  - [Refresh Token Diagram](database-diagrams/entities/Diagram.RefreshToken.md)
  - [Single Select Option Diagram](database-diagrams/entities/Diagram.SingleSelectOption.md)
  - [Workspace Role Diagram](database-diagrams/entities/Diagram.WorkspaceRole.md)
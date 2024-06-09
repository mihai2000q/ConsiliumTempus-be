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

## General

Each aggregate, entity or value object has an empty private constructor
so that the Entity Framework Core can initialize them.

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

- [Project Aggregate](domain/aggregates/Aggregate.Project.md)
- [Project Sprint Aggregate](domain/aggregates/Aggregate.Project.md)
- [Project Task Aggregate](domain/aggregates/Aggregate.ProjectTask.md)
- [User Aggregate](domain/aggregates/Aggregate.User)
- [Workspace Aggregate](domain/aggregates/Aggregate.Workspace)

## Entities

Essentially, an entity holds data and an Id, and by definition, two entities are equal only if their Ids are equal.
All entities can hold domain events (including aggregates).

Typically, the entities will be persisted in the database on another table.

The entities are:

- [Audit](domain/entities/Entity.Audit.md)
- [Membership](domain/entities/Entity.Membership.md)
- [Permission](domain/entities/Entity.Permission.md)
- [Refresh Token](domain/entities/Entity.RefreshToken.md)
- [Workspace Role](domain/entities/Entity.WorkspaceRole.md)

## Value Objects

Essentially, a value object holds only values and, by definition,
two value objets are equal only if all the values are equal with one another.

Typically, the value objects will be flattened on the object before persisting in the database.

The value objects are:

- **CustomOrderPosition**, which encapsulates an int *Value,* and it's used to achieve custom ordering
- **Description**, which encapsulates a string *Value*
- **IsPrivate**, which encapsulates a boolean *Value*
- **Message**, which encapsulates a string *Value*
- **Name**, which encapsulates a string *Value*
- **Title**, which encapsulates a string *Value*

## Domain Errors

These errors are not tied to any aggregate, therefore notable and only used in the Application Layer.

- Authentication
  - **Invalid Credentials** when, upon authentication, the credentials are invalid
  - **Invalid Tokens** when, upon refreshing the token, either the access or refresh token is invalid

## Enums

The enums defined in the Domain of the application are:

- **FilterOperator**, which is used to represent the different numerous operators that can be used for filtering 
(i.e., equal or less than)
- **OrderType**, which is used to determine whether the order should be ascending or descending
- **Permissions**, which are used on the Presentation Layer for Authorization

## Relations

The classes inside the Relations package contain the entities used to create the joint tables between other entities.
<br>
Those include:

- **WorkspaceRoleHasPermission**

## Database Diagrams

The database diagrams show the relations that the above entities have inside the database.
For a full picture of the database diagram, check this out [Database Diagram](database-diagrams/Database.Diagram.md).
<br>
For individual diagrams (they exclude additional info about external entities, 
except the primary key and direct relations), check below:

- Aggregates
  - [Project Diagram](database-diagrams/aggregates/Diagram.Project.md)
  - [Project Sprint Diagram](database-diagrams/aggregates/Diagram.ProjectSprint.md)
  - [Project Task Diagram](database-diagrams/aggregates/Diagram.ProjectTask.md)
  - [User Diagram](database-diagrams/aggregates/Diagram.User.md)
  - [Workspace Diagram](database-diagrams/aggregates/Diagram.Workspace.md)
- Entities
  - [Audit Diagram](database-diagrams/entities/Diagram.Audit.md)
  - [Membership Diagram](database-diagrams/entities/Diagram.Membership.md)
  - [Permission Diagram](database-diagrams/entities/Diagram.Permission.md)
  - [Refresh Token Diagram](database-diagrams/entities/Diagram.RefreshToken.md)
  - [Workspace Role Diagram](database-diagrams/entities/Diagram.WorkspaceRole.md)
  - Project Entities
    - [Project Status Diagram](database-diagrams/entities/project/Diagram.ProjectStatus.md)
  - Project Sprint Entities
    - [Project Stage Diagram](database-diagrams/entities/project-sprint/Diagram.ProjectStage.md)
  - Project Task Entities
    - [Project Task Comment Diagram](database-diagrams/entities/project-task/Diagram.ProjectTaskComment.md)

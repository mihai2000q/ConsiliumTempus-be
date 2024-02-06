# Consilium Tempus Backend Domain Models

* [Consilium Tempus Backend Domain Models](#consilium-tempus-backend-domain-models)
  * [General](#general)
  * [Aggregates](#aggregates)
  * [Entities](#entities)
  * [Value Objects](#value-objects)
  * [General Domain Errors](#general-domain-errors)
  * [Database Diagrams](#database-diagrams)

Below, you will find a complete list and documentation for each **Aggregate**, **Entity** and **Value Object**.
<br>
Additionally, on the bottom of the page there can be found [Database Diagrams](#database-diagrams) 
representing the relationships of the aggregates with other aggregates and/or entities.

## General

Each aggregate, entity or value object has an empty private constructor 
so that the Entity Framework can initialize them.

## Aggregates

All Aggregates inherit from the **Aggregate Root** class, which requires a unique class to be the Id.
This Id has to inherit from the **Aggregate Root Id**, which is a value object 
(they won't be included in the value object lists as they are required and part of all the aggregates).
**Aggregate Root** is also essentially an **Entity** at its roots,
however, an *Aggregate** will usually be a model that contains all the relations of multiple entities.

Each aggregate can have its own set of **entities** or **value objects**.

Each aggregate has a set of possible *errors* and *properties validation*.
The aforementioned refers to the constraints that each property has
(i.e., the name cannot be longer than 100 characters).

The aggregates are:

- [User Aggregate](domain/aggregates/Aggregates.User.md)
- [Workspace Aggregate](domain/aggregates/Aggregates.Workspace.md)

## Entities

Essentially, an entity holds data and an Id, and by definition, two entities are equal only if their Ids are equal.
All entities can hold domain events (including aggregates).

Typically, the entities will be persisted in the database on another table.

The entities are:

- Nothing atm

## Value Objects

Essentially, a value object holds only values and, by definition, 
two value objets are equal only if all the values are equal with one another.

Typically, the value objects will be flattened on the object before persisting in the database.

The value objects are:

- Nothing atm.

## General Domain Errors

These errors are not tied to any aggregate, therefore notable and only used in the Application Layer.

- Authentication
  - **Invalid Credentials** when, upon authentication, the credentials are invalid
  - **Invalid Token** when the token is deemed invalid (i.e., userId is wrong), typically when it has been tampered with

## Database Diagrams

For a full picture of the database diagram, check this out [Database Diagram](Database.Diagram.md).
These diagrams show the relations that the above aggregates have inside the database.

- [User Diagram](domain/diagrams/Diagram.User.md)
- [Workspace Diagram](domain/diagrams/Diagram.Workspace.md)
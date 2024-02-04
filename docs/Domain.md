# Consilium Tempus Backend Domain Models

Below, you will find a complete list and documentation for each **Aggregate** and **Entity**.
Each aggregate has a set of possible *errors* and *properties validation*.
The aforementioned refers to the constraints that each property has 
(i.e., the name cannot be longer than 100 characters).
<br>
Additionally, on the bottom of the page there can be found [Database Diagrams](#database-diagrams) 
representing the relationships of the aggregates with other aggregates and/or entities.

## Aggregates

- [User Aggregate](domain/aggregates/Aggregates.User.md)

## Entities

- Nothing atm

## Notable Errors

These errors are not tied to any aggregate, therefore notable and only used in the Application Layer.

- Authentication
  - **Invalid Credentials** for when, upon authentication, the credentials are invalid

## Database Diagrams

These diagrams show the relations that the above aggregates have inside the database.

- [User Diagram](domain/diagrams/Diagram.User.md)
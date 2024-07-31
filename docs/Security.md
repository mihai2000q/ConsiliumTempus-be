# Consilium Tempus Backend Security

* [Access Control](#access-control)
* [Role-Based Access Control (RBAC)](#role-based-access-control-rbac)
* [Workspace Authorization levels](#workspace-authorization-levels)
* [Workspace Roles](#workspace-roles)
* [Permissions](#permissions)
* [Workspace Roles to Permissions](#workspace-roles-to-permissions)

In a general context, the application implements different methods to keep the application secure.
One of the practices used includes: RBAC, hashing and more.

## Access Control

Before gaining access to the application, the user must go through a suite of steps:

- **Identification**, which refers to a way for the user to be identified (can be public info).
  The application uses the E-mail as a unique identifier.
- **Authentication**, which refers to the possession of a JWT Token to gain access to the application.
  It also refers to how the application stores the password, and in our case,
  the system uses BCrypt as a password-hashing mechanism to store the password securely.
- **Authorization**, which refers to actually checking the permissions of the user.
  For example, if the "delete" endpoint for the workspace is accessed,
  check whether the current role of the user has the permission to proceed with the operation.

## Role-Based Access Control (RBAC)

The system implements a **Role-Based Access Control** (RBAC) access control type of solution.
That means that there is a central administrated set of controls.
It also supports the principles of the *least privilege* and *separation of duties*.
The roles tend to be in a hierarchy, so each role inherits from the one below,
thus restricting the power held by an individual.

This security method is used to manage conflicts of interest and fraud or tampering with the application.

However, these roles are exclusive to the [Workspace](domain/aggregates/Aggregate.Workspace) component.
For example, if the user is not in a workspace, he cannot create any project, portfolio or add any tasks.
So he basically cannot use the application to its fullest
(thankfully, all users must have a workspace, and one is created upon registration).

Also, it should be mentioned that there are a few exceptions, such as:

- a few actions can be done only by the owner of the workspace, or the owner of the project 
(i.e., giving up ownership of the workspace/project to another collaborator)
- a few actions don't require permission, they simply require that you are part of the workspace
  (i.e., leaving a workspace, anyone can do it, but they have to be part of it first)
  
Those are categorized as **Workspace Authorization Levels** and **Project Authorization Levels**.

## Workspace Authorization Levels

There are only a few levels in the workspace that differ from just having permission, and those are:
- **Is Workspace Owner**, which requires the user that made the request to be the owner of the workspace
- **Is Collaborator**, which requires the user that made the request to be part of the workspace (i.e., a collaborator)

## Project Authorization Levels

There are only a few levels in the project that differ from just having permission, and those are:
- **Is Project Owner**, which requires the user that made the request to be the owner of the project
- **Is Allowed**, which requires the user that made the request to be part of the allowed members of a project

## Workspace Roles

The owner will, most of the time, be able to delete the data that they have added.

The workspace roles are:

- **View**, they can mostly just read information
- **Member**, they can, in most of the cases, read, update and sometimes even add data
- **Admin**, they can do everything

## Permissions

The permissions will be classified on their respective component:

- Workspace
  - Read
  - Read Overview
  - Read Invitations
  - Update
  - Update Favorites
  - Update Overview
  - Delete
  - Invite Collaborator
  - Read Collaborators
  - Update Collaborator
- Project
  - Create
  - Read
  - Read Overview
  - Read Collection
  - Update
  - Update Favorites
  - Update Overview
  - Delete
  - Add Status
  - Read Statuses
  - Update Status
  - Remove Status
  - Read Allowed Members
- Project Sprint
  - Create
  - Read
  - Read Collection
  - Update
  - Delete
  - Add Stage
  - Read Stages
  - Move Stage
  - Update Stage
  - Remove Stage
- Project Task
  - Create
  - Read
  - Read Collection
  - Move
  - Update
  - Update Is Completed
  - Update Overview
  - Delete

## Workspace Roles to Permissions

The Access Control List of the system is the following:

|                                    | View | Member | Admin |
|------------------------------------|:----:|:------:|:-----:|
| Read Workspace                     |  X   |   X    |   X   |
| Read Overview Workspace            |  X   |   X    |   X   |
| Read Invitations From Workspace    |      |        |   X   |
| Update Workspace                   |      |   X    |   X   |
| Update Favorites Workspace         |  X   |   X    |   X   |
| Update Overview Workspace          |      |   X    |   X   |
| Delete Workspace                   |      |        |   X   |
| Invite Collaborator To Workspace   |      |        |   X   |
| Read Collaborators From Workspace  |  X   |   X    |   X   |
| Update Collaborator From Workspace |      |        |   X   |
| Create Project                     |      |        |   X   |
| Read Project                       |  X   |   X    |   X   |
| Read Overview Project              |  X   |   X    |   X   |
| Read Collection Project            |  X   |   X    |   X   |
| Update Project                     |      |   X    |   X   |
| Update Favorites Project           |  X   |   X    |   X   |
| Update Overview Project            |      |   X    |   X   |
| Delete Project                     |      |        |   X   |
| Add Status To Project              |      |        |   X   |
| Read Statuses From Project         |      |   X    |   X   |
| Update Status From Project         |      |   X    |   X   |
| Remove Status From Project         |      |        |   X   |
| Read Allowed Members From Project  |      |   X    |   X   |
| Create Project Sprint              |      |        |   X   |
| Read Project Sprint                |  X   |   X    |   X   |
| Read Collection Project Sprint     |  X   |   X    |   X   |
| Update Project Sprint              |      |   X    |   X   |
| Delete Project Sprint              |      |        |   X   |
| Add Stage To Project Sprint        |      |        |   X   |
| Read Stages From Project Sprint    |  X   |   X    |   X   |
| Move Stage From Project Sprint     |      |        |   X   |
| Update Stage From Project Sprint   |      |   X    |   X   |
| Remove Stage From Project Sprint   |      |        |   X   |
| Create Project Task                |      |   X    |   X   |
| Read Project Task                  |  X   |   X    |   X   |
| Read Collection Project Task       |  X   |   X    |   X   |
| Move Project Task                  |      |   X    |   X   |
| Update Project Task                |      |   X    |   X   |
| Update Is Completed Project Task   |      |   X    |   X   |
| Update Overview Project Task       |      |   X    |   X   |
| Delete Project Task                |      |   X    |   X   |
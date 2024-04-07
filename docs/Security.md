# Consilium Tempus Backend Security

* [Access Control](#access-control)
* [Role-Based Access Control (RBAC)](#role-based-access-control-rbac)
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
If the user is not in a workspace, he cannot create any project, portfolio or add any tasks.
So he basically cannot use the application to its fullest
(thankfully, all users must have a workspace, and one is created upon registering).

## Workspace Roles

The owner will, most of the time, be able to delete the data that they have added.

The workspace roles are:

- **View**, they can mostly just read information
- **Member**, they can, in most of the cases, read, add, update data and sometimes delete
- **Admin**, they can do everything

## Permissions

The permissions will be classified on their respective component:

- Workspace
  - Read
  - Update
  - Delete
- Project
  - Read
  - Read Collection
  - Create
  - Update
  - Delete
- Project Sprint
  - Create
  - Update
  - Delete

## Workspace Roles to Permissions

The Access Control List of the system is the following:

|                         | View | Member | Admin |
|-------------------------|------|--------|-------|
| Read Workspace          | X    | X      | X     |
| Update Workspace        |      | X      | X     |
| Delete Workspace        |      |        | X     |
| Create Project          |      |        | X     |
| Read Project            | X    | X      | X     |
| Read Collection Project | X    | X      | X     |
| Update Project          |      | X      | X     |
| Delete Project          |      |        | X     |
| Create Project Sprint   |      |        | X     |
| Update Project Sprint   |      | X      | X     |
| Delete Project Sprint   |      |        | X     |
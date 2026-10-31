# Consilium Tempus Backend Security

* [Access Control](#access-control)
* [Role-Based Access Control (RBAC)](#role-based-access-control-rbac)
* [Workspace Authorization Levels](#workspace-authorization-levels)
* [Project Authorization Levels](#project-authorization-levels)
* [Targeted Entity](#targeted-entity)
* [Hidden Resources](#hidden-resources)
* [Workspace Roles](#workspace-roles)
* [Permissions](#permissions)
* [Workspace Roles to Permissions](#workspace-roles-to-permissions)
* [Adding a New Permission](#adding-a-new-permission)

In a general context, the application implements different methods to keep the application secure.
Some of the practices used include: RBAC, hashing and more.

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

However, these roles are exclusive to the [Workspace](domain/aggregates/Aggregate.Workspace.md) component.
For example, if the user is not in a workspace, they cannot create any project or add any tasks.
So they basically cannot use the application to its fullest
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

Some endpoints combine several checks, and all of them have to pass.
For example, most of the project endpoints require both a permission and the **Is Allowed** level,
while the ones that manage the allowed members, the owner or the privacy of a project (or leaving a private project)
require a Project Authorization Level together with the Workspace Authorization Level **Is Collaborator**.
The checks of each endpoint are listed in its [Api documentation](api/Api.Project.md),
and they are applied through attributes (see [Authorization](Api.md#authorization)).

## Targeted Entity

Every check starts by finding the entity that the endpoint is about, through the id that comes with the request
(the project of a project id, the workspace of a task id, etc.).
Where that id is, for each endpoint, is decided in a single place, the `AuthorizationTargets` class of the Infrastructure,
and it is used by the permission and by both authorization levels.

By convention, an endpoint is about the entity of its own controller, through the property `id`,
which is in the route for a `GET` or a `DELETE` and in the body for a `POST`, `PATCH` or a `PUT`.
The endpoints that work through a parent entity instead are the exceptions, and they are listed in the class:
- the ones that create or list entities of a parent (e.g., creating a project sprint through the `projectId`
  in the body, or listing the project tasks through the `projectStageId` in the query)
- the ones that, for the project authorization levels, are about the project, although the permission is about
  another entity (adding a custom field setup to a project, or removing it from one)

The place of the id comes from the endpoint, never from the content of the request,
because it has to be the one that the model binding is going to use.
An endpoint that cannot be resolved (an unknown controller or HTTP method) fails instead of passing.

Since the convention is not enforced by the compiler, the `AuthorizationTargetsTest` of the architecture tests
checks every endpoint that has a permission or an authorization level:
the id that it resolves has to exist in the request of the endpoint, and has to be bound from the place
that it was resolved to (route, query or body).

## Hidden Resources

The authorization must not reveal whether a resource exists.
Because of that, when the user has no relationship with the resource, the response is the same **404 Not Found**
that is returned when the resource does not exist (same title and error code, i.e., `Project.NotFound`),
instead of a **403 Forbidden**.
The user has no relationship with the resource when:
- they are not a collaborator of its workspace (checked by the permission and the **Is Collaborator** level)
- the project is private and they are not one of the allowed members (checked by the project authorization levels)

A **403** is only returned when the user does know that the resource exists
(i.e., a **View** collaborator that tries to do something that requires the **Member** role,
or a collaborator that is not the owner of the workspace).

This is achieved in the authorization handlers, which fail with a *not found* reason,
and in a result handler that turns that reason into the not found response of the resource
(the one from the identifier that was used to find it, i.e., the project for a project id).
The handlers also fail with the not found of the resource when a well-formed identifier does not exist,
so the endpoints answer the same way whether the resource is missing or hidden
(an empty or malformed identifier is left to the validation).

## Workspace Roles

The owner will, most of the time, be able to delete the data that they have added.

The workspace roles are:

- **View**, they can mostly just read information
- **Member**, they can, in most of the cases, read, update and sometimes even add data
- **Admin**, they can do everything (except for the actions that are exclusive to the owners)

The owner of a workspace is always an **Admin**: the new owner is promoted when the ownership is transferred
(or when the owner deletes their account), and the role of the owner cannot be changed by anyone.
Because the owner cannot leave or be kicked from the workspace either, a workspace always has at least one **Admin**.

## Permissions

The permissions will be classified on their respective component:

- [Custom Field Setup](api/Api.CustomFieldSetup.md)
  - Create On Project
  - Create On Workspace
  - Add To Project
  - Read
  - Read Collection From Workspace
  - Read Collection From Project
  - Update
  - Make Global
  - Delete
  - Remove From Project
- [Project](api/Api.Project.md)
  - Create
  - Read
  - Read Overview
  - Read Collection
  - Update
  - Update Favorites
  - Update Overview
  - Delete
  - Read Allowed Members
  - Add Status
  - Read Statuses
  - Update Status
  - Remove Status
- [Project Sprint](api/Api.ProjectSprint.md)
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
- [Project Task](api/Api.ProjectTask.md)
  - Create
  - Read
  - Read Collection
  - Move
  - Update
  - Update Custom Field
  - Update Is Completed
  - Update Overview
  - Delete
- [Workspace](api/Api.Workspace.md)
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
  - Kick Collaborator

## Workspace Roles to Permissions

The Access Control List of the system is the following
(it comes from the `WorkspaceRoleHasPermission` class, inside the Domain, and it is saved in the database by the migrations):

|                                                   | View | Member | Admin |
|---------------------------------------------------|:----:|:------:|:-----:|
| Create Custom Field Setup On Project              |      |        |   X   |
| Create Custom Field Setup On Workspace            |      |        |   X   |
| Add Custom Field Setup To Project                 |      |        |   X   |
| Read Custom Field Setup                           |      |   X    |   X   |
| Read Collection Custom Field Setup From Project   |      |   X    |   X   |
| Read Collection Custom Field Setup From Workspace |      |   X    |   X   |
| Update Custom Field Setup                         |      |        |   X   |
| Make Custom Field Setup Global                    |      |        |   X   |
| Delete Custom Field Setup                         |      |        |   X   |
| Remove Custom Field Setup From Project            |      |        |   X   |
| Create Project                                    |      |        |   X   |
| Read Project                                      |  X   |   X    |   X   |
| Read Overview Project                             |  X   |   X    |   X   |
| Read Collection Project                           |  X   |   X    |   X   |
| Update Project                                    |      |   X    |   X   |
| Update Favorites Project                          |  X   |   X    |   X   |
| Update Overview Project                           |      |   X    |   X   |
| Delete Project                                    |      |        |   X   |
| Read Allowed Members From Project                 |      |   X    |   X   |
| Add Status To Project                             |      |        |   X   |
| Read Statuses From Project                        |  X   |   X    |   X   |
| Update Status From Project                        |      |   X    |   X   |
| Remove Status From Project                        |      |        |   X   |
| Create Project Sprint                             |      |        |   X   |
| Read Project Sprint                               |  X   |   X    |   X   |
| Read Collection Project Sprint                    |  X   |   X    |   X   |
| Update Project Sprint                             |      |   X    |   X   |
| Delete Project Sprint                             |      |        |   X   |
| Add Stage To Project Sprint                       |      |        |   X   |
| Read Stages From Project Sprint                   |  X   |   X    |   X   |
| Move Stage From Project Sprint                    |      |        |   X   |
| Update Stage From Project Sprint                  |      |   X    |   X   |
| Remove Stage From Project Sprint                  |      |        |   X   |
| Create Project Task                               |      |   X    |   X   |
| Read Project Task                                 |  X   |   X    |   X   |
| Read Collection Project Task                      |  X   |   X    |   X   |
| Move Project Task                                 |      |   X    |   X   |
| Update Project Task                               |      |   X    |   X   |
| Update Custom Field From Project Task             |      |   X    |   X   |
| Update Is Completed Project Task                  |      |   X    |   X   |
| Update Overview Project Task                      |      |   X    |   X   |
| Delete Project Task                               |      |   X    |   X   |
| Read Workspace                                    |  X   |   X    |   X   |
| Read Overview Workspace                           |  X   |   X    |   X   |
| Read Invitations From Workspace                   |      |        |   X   |
| Update Workspace                                  |      |   X    |   X   |
| Update Favorites Workspace                        |  X   |   X    |   X   |
| Update Overview Workspace                         |      |   X    |   X   |
| Delete Workspace                                  |      |        |   X   |
| Invite Collaborator To Workspace                  |      |        |   X   |
| Read Collaborators From Workspace                 |  X   |   X    |   X   |
| Update Collaborator From Workspace                |      |        |   X   |
| Kick Collaborator From Workspace                  |      |        |   X   |

## Adding a New Permission

To add a new permission, the following steps are needed:

1. Add it to the **Permissions** enum, inside the Domain
   (the id of the permission in the database is its position in the enum, plus one).
2. Grant it to the roles that should have it, inside the `WorkspaceRoleHasPermission` class.
3. Make sure that the Infrastructure finds the id that links the request to its workspace
   (see [Targeted Entity](#targeted-entity)): nothing is needed when the endpoint follows the convention,
   otherwise add it to the exceptions of the `AuthorizationTargets` class.
   The `AuthorizationTargetsTest` of the architecture tests fails when the id is not where the endpoint binds it.
4. Create a [migration](Database.md#migrations), because both the permissions and the roles to permissions
   relations are persisted by the migrations.
5. Add the **Has Permission** attribute with the new permission to the endpoint (see [Authorization](Api.md#authorization)).
6. Update the permission list and the table above, and the documentation of the endpoint.

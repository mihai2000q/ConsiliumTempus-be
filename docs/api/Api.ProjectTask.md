# Consilium Tempus API

* [Project Task](#project-task)
  * [Get](#get)
    * [Get Request](#get-request)
    * [Get Response](#get-response)
  * [Get Collection](#get-collection)
    * [Get Collection Request](#get-collection-request)
    * [Get Collection Response](#get-collection-response)
  * [Create](#create)
    * [Create Project Task Request](#create-project-task-request)
    * [Create Project Task Response](#create-project-task-response)
  * [Move](#move)
    * [Move Project Task Request](#move-project-task-request)
    * [Move Project Task Response](#move-project-task-response)
  * [Update](#update)
    * [Update Project Task Request](#update-project-task-request)
    * [Update Project Task Response](#update-project-task-response)
  * [Update Overview](#update-overview)
    * [Update Overview Project Task Request](#update-overview-project-task-request)
    * [Update Overview Project Task Response](#update-overview-project-task-response)
  * [Delete](#delete)
    * [Delete Project Task Request](#delete-project-task-request)
    * [Delete Project Task Response](#delete-project-task-response)

## Project Task

This is the controller that takes care of creating, updating, deleting and querying Project Tasks.

### Get

Anyone that is part of the workspace can read a project
([Read Project Task Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/projects/tasks/{id}
```

- **id** is a 36-character string

#### Get Request

Sends the id of the project task inside the route of the request.

#### Get Response

Returns a project task.

```json
{
  "name": "Project Task Name 1",
  "description": "This task is gonna be awesome and I can't wait to work on it.",
  "isCompleted": true,
  "assignee": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Michael Jordan",
    "email": "michael@jordan.com"
  },
  "stage": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "In Progress"
  },
  "sprint": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Sprint 1",
    "stages": [
      {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "To do"
      },
      {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "In Progress"
      },
      {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Completed"
      }
    ]
  },
  "project": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Some Project"
  },
  "workspace": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Some Workspace"
  }
}
```

### Get Collection

Anyone that is part of the workspace can read the projects
([Read Collection Project Task Permission](../Security.md/#permissions)),
in case the workspace Id is mentioned, otherwise any logged-in user will get their projects.

```js
GET {{host}}/api/projects/tasks?projectStageId=10000000-0000-0000-0000-000000000000&search=name ct task&orderBy=name.asc&currentPage=1&pageSize=20
```

#### Get Collection Request

Sends the following as query parameters:

- **projectStageId** is used to specify the project stage that the tasks are part of
- **search** is used to filter the tasks dynamically by their properties
- **orderBy** is used to order the tasks dynamically by their properties
- **currentPage** is used to specify the current page when paginating
- **pageSize** is used to specify the size of the page when paginating

#### Get Collection Response

Returns the project tasks and their total count.

```json
{
  "tasks": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Project Task Name 1",
      "isCompleted": true,
      "assignee": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      }
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Project Task Name 2",
      "isCompleted": false,
      "assignee": null
    }
  ],
  "totalCount": 2
}
```

### Create

All members that are part of the workspace can create a project
([Create Project Task Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/tasks
```

#### Create Project Task Request

Sends body data that the new project task needs to be created.

```json
{
  "projectStageId": "10000000-0000-0000-0000-000000000000",
  "name": "Project Task Name",
  "onTop": false
}
```

#### Create Project Task Response

Returns a confirmation message that the project task has been created successfully.

### Move

All members that are part of the workspace can move a project task
([Update Project Task Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/tasks/move
```

#### Move Project Task Request

Sends body data that the project task needs to be moved.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "overId": "20000000-0000-0000-0000-000000000000"
}
```

#### Move Project Task Response

Returns a confirmation message that the project task has been moved successfully.

### Update

All members that are part of the workspace can update a project task
([Update Project Task Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/tasks
```

#### Update Project Task Request

Sends body data that the project task needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "New Project Task Name",
  "assigneeId": "10000000-0000-0000-0000-000000000000"
}
```

#### Update Project Task Response

Returns a confirmation message that the project task has been updated successfully.

### Update

All members that are part of the workspace can update a project task
([Update Project Task Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/tasks/is-completed
```

#### Update Project Task Request

Sends body data that the project task needs to update the completion status.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "isCompleted": false
}
```

#### Update Is Completed Project Task Response

Returns a confirmation message that the project task's completion status has been updated successfully.

### Update Overview

All members that are part of the workspace can update a project task overview
([Update Project Task Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/tasks/overview
```

#### Update Overview Project Task Request

Sends body data that the project overview needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "New Project Task Name",
  "description": "This is the new description of the project task that I can't wait to work on",
  "assigneeId": "10000000-0000-0000-0000-000000000000"
}
```

#### Update Overview Project Task Response

Returns a confirmation message that the project task overview has been updated successfully.

### Delete

Only admin users that are part of the workspace can delete a project
([Delete Project Task Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/projects/tasks/{id}/from/{stageId}
```

- **id** is a 36 characters strings
- **stageId** is a 36 characters strings

#### Delete Project Task Request

Sends the id of the task and the stage inside the route request.

#### Delete Project Task Response

Returns a confirmation message that the project has been deleted successfully.
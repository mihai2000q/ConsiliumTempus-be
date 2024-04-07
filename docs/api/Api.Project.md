# Consilium Tempus API

* [Project](#project)
  * [Get Collection For Workspace](#get-collection-for-workspace)
    * [Get Collection For Workspace Request](#get-collection-for-workspace-request)
    * [Get Collection For Workspace Response](#get-collection-for-workspace-response)
  * [Get Collection For User](#get-collection-for-user)
    * [Get Collection For User Response](#get-collection-for-user-response)
  * [Create](#create)
    * [Create Project Request](#create-project-request)
    * [Create Project Response](#create-project-response)
  * [Delete](#delete)
    * [Delete Project Request](#delete-project-request)
    * [Delete Project Response](#delete-project-response)

## Project

This is the controller that takes care of creating, reading, updating and deleting a Project.


### Get Collection For Workspace

Anyone that is part of the workspace can read the projects.

```js
GET {{host}}/api/projects/workspace?workspaceId={workspaceId}
```

- **workspaceId** is a 36-character string

#### Get Collection For Workspace Request

Sends the id of the workspace inside the query parameters of the request.

#### Get Collection For Workspace Response

Returns wrapped up projects' ids, names, and descriptions.

```json
{
  "projects": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Project Name 1",
      "description": "This is the first project"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Project Name 2",
      "description": "This is the second project"
    }
  ]
}
```


### Get Collection For User

Anyone logged in can fetch the projects that they are part of.

```js
GET {{host}}/api/projects/user
```

#### Get Collection For User Response

Returns wrapped up projects' ids and names.

```json
{
  "projects": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Project Name 1"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Project Name 2"
    }
  ]
}
```


### Create

Only admin users that are part of the workspace can create a project
([Create Project Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects
```

#### Create Project Request

Sends body data that the new workspace needs to be created.

```json
{
  "workspaceId": "10000000-0000-0000-0000-000000000000",
  "name": "Project Name",
  "description": "This is the description of the project",
  "isPrivate": false
}
```

#### Create Project Response

Returns a confirmation message that the project has been created successfully.


### Delete

Only admin users that are part of the workspace can delete a project
([Delete Project Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/projects/{id}
```

- **id** is a 36 characters strings

#### Delete Project Request

Sends the id of the sprint inside the route request.

#### Delete Project Response

Returns a confirmation message that the project has been deleted successfully.
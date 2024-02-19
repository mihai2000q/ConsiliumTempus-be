# Consilium Tempus API

* [Project](#project)
    * [Create](#create)
        * [Create Project Request](#create-project-request)
        * [Create Project Response](#create-project-response)
    * [Delete](#delete)
        * [Delete Project Request](#delete-project-request)
        * [Delete Project Response](#delete-project-response)

## Project

This is the controller that takes care of creating, reading, updating and deleting a Project.


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
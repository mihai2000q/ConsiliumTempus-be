# Consilium Tempus API

* [Project](#project)
    * [Create](#create)
        * [Create Project Request](#create-project-request)
        * [Create Project Response](#create-project-response)

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
  "name": "Workspace Name",
  "description": "This is the description of the workspace",
  "isPrivate": false
}
```

#### Create Project Response

Returns a confirmation message that the project has been created.

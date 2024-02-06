# Consilium Tempus API

* [Workspace](#workspace)
  * [Create](#create)
    * [Create Workspace Request](#create-workspace-request)
    * [Create Workspace Response](#create-workspace-response)

## Workspace

This is the controller that takes care of creating, reading, updating and deleting a Workspace.

### Get

Only users that are part of the workspace can retrieve it.

```js
GET {{host}}/api/workspaces/{id}
```

- **id** is a 36 characters strings

#### Get Workspace Request

Sends the id of the object inside the route request.

#### Get Workspace Response

Returns the workspace [Dto](dto/Dto.Workspace.md).

### Create

Anyone logged in can create a workspace.

```js
POST {{host}}/api/workspaces/create
```

#### Create Workspace Request

Sends body data for the new workspace needs to be created.

```json
{
  "name": "Workspace Name",
  "description": "This is the description of the workspace"
}
```

#### Create Workspace Response

Returns the newly created workspace [Dto](dto/Dto.Workspace.md).
# Consilium Tempus API

* [Workspace](#workspace)
  * [Create](#create)
    * [Create Workspace Request](#create-workspace-request)
    * [Create Workspace Response](#create-workspace-response)

## Workspace

This is the controller that takes care of creating, reading, updating and deleting a Workspace.

### Create

Anyone logged in can create a workspace.

```js
POST {{host}}/api/workspace/create
```

#### Create Workspace Request

Sends data for the new workspace needs to be created.

```json
{
  "name": "Workspace Name",
  "description": "This is the description of the workspace"
}
```

#### Create Workspace Response

Receives the newly created workspace.

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "name": "Workspace Name",
  "description": "This is the description of the workspace"
}
```
# Consilium Tempus API

* [Workspace](#workspace)
  * [Get](#get)
    * [Get Workspace Request](#get-workspace-request)
    * [Get Workspace Response](#get-workspace-response)
  * [Get Collection](#get-collection)
    * [Get Collection Workspace Response](#get-collection-workspace-response)
  * [Create](#create)
    * [Create Workspace Request](#create-workspace-request)
    * [Create Workspace Response](#create-workspace-response)
  * [Update](#update)
    * [Update Workspace Request](#update-workspace-request)
    * [Update Workspace Response](#update-workspace-response)
  * [Delete](#delete)
    * [Delete Workspace Request](#delete-workspace-request)
    * [Delete Workspace Response](#delete-workspace-response)

## Workspace

This is the controller that takes care of creating, reading, updating and deleting a Workspace.


### Get

Only users that are part of the workspace can retrieve it ([Read Workspace Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/workspaces/{id}
```

- **id** is a 36 characters strings

#### Get Workspace Request

Sends the id of the object inside the route request.

#### Get Workspace Response

Returns the workspace [Dto](dto/Dto.Workspace.md).


### Get Collection

Anyone logged in can request this data, but it will return only the workspaces that are linked to this user.

```js
GET {{host}}/api/workspaces
```

#### Get Collection Workspace Response

Returns the workspaces [Dtos](dto/Dto.Workspace.md).


### Create

Anyone logged in can create a workspace.

```js
POST {{host}}/api/workspaces
```

#### Create Workspace Request

Sends body data that the new workspace needs to be created.

```json
{
  "name": "Workspace Name",
  "description": "This is the description of the workspace"
}
```

#### Create Workspace Response

Returns the newly created workspace [Dto](dto/Dto.Workspace.md).


### Update

Only member or admin users that are part of the workspace can update it
([Update Workspace Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/workspaces
```

#### Update Workspace Request

Sends body data that the new workspace needs to be updated.
<br>
All parameters are optional except the id.

```json
{
  "id": "88882448-bd63-4731-8a05-f6333b6d22e2",
  "name": "Workspace Name",
  "description": "This is the new description of the workspace"
}
```

#### Update Workspace Response

Returns the new workspace [Dto](dto/Dto.Workspace.md).


### Delete

Only admin users that are part of the workspace can delete it 
([Delete Workspace Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/workspaces/{id}
```

- **id** is a 36 characters strings

#### Delete Workspace Request

Sends the id of the workspace inside the route request.

#### Delete Workspace Response

Returns the workspace [Dto](dto/Dto.Workspace.md).
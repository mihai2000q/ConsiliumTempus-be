# Consilium Tempus API

* [Workspace](#workspace)
  * [Get](#get)
    * [Get Workspace Request](#get-workspace-request)
    * [Get Workspace Response](#get-workspace-response)
  * [Get Collection](#get-collection)
    * [Get Collection Workspace Request](#get-collection-workspace-request)
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

Returns the workspace data.

```json
{
  "name": "Workspace Name",
  "description": "This is the description of the workspace"
}
```


### Get Collection

Anyone logged in can request this data, but it will return only the workspaces that are linked to this user.

```js
GET {{host}}/api/workspaces?order=name.asc&name=worksp
```

### Get Collection Workspace Request

Sends optional query parameters for ordering, filtering, and page-based pagination.

- **order** is used to order the collection
- **name** is used to filter by name

#### Get Collection Workspace Response

Returns the workspaces.

```json
{
  "workspaces": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Workspace 1",
      "description": "This is the first workspace"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Workspace 2",
      "description": "This is the second workspace"
    }
  ]
}
```


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

Returns a confirmation message that the workspace has been created successfully.


### Update

Only member or admin users that are part of the workspace can update it
([Update Workspace Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/workspaces
```

#### Update Workspace Request

Sends body data that the new workspace needs to be updated.
<br>

```json
{
  "id": "88882448-bd63-4731-8a05-f6333b6d22e2",
  "name": "Workspace Name",
  "description": "This is the new description of the workspace"
}
```

#### Update Workspace Response

Returns a confirmation message that the workspace has been updated successfully.


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

Returns a confirmation message that the workspace has been deleted successfully.
# Consilium Tempus API

* [Workspace](#workspace)
  * [Get](#get)
    * [Get Workspace Request](#get-workspace-request)
    * [Get Workspace Response](#get-workspace-response)
  * [Get Collaborators](#get-collaborators)
    * [Get Collaborators From Workspace Request](#get-collaborators-from-workspace-request)
    * [Get Collaborators From Workspace Response](#get-collaborators-from-workspace-response)
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

Sends the id of the workspace inside the route request.

#### Get Workspace Response

Returns the workspace data.

```json
{
  "name": "Workspace Name",
  "description": "This is the description of the workspace",
  "isFavorite": true,
  "isPersonal": false
}
```

### Get Collaborators

Only users that are part of the workspace can retrieve it ([Read Workspace Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/workspaces/{id}/collaborators?searchValue=michelle
```

- **id** is a 36 characters strings

#### Get Collaborators From Workspace Request

Sends the id of the workspace inside the route request and the following query parameters:

- **searchValue** to search through the collaborators by email, name, or other identifier

#### Get Collaborators From Workspace Response

Returns the collaborators.

```json
{
  "collaborators": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Michael Jordan",
      "email": "michaelj@gmail.com"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Stephen Curry",
      "email": "stephenc@gmail.com"
    }
  ]
}
```

### Get Collection

Anyone logged in can request this data, but it will return only the workspaces that are linked to this user.

```js
GET {{host}}/api/workspaces?pageSize=2&currentPage=1orderBy=name.asc&orderBy=updated_date_time.desc&search=name ct worksp
```

### Get Collection Workspace Request

Sends optional query parameters for ordering, filtering, and page-based pagination.

- _**pageSize**_ is used to specify the size of the page
- _**currentPage**_ is used to specify the current page
- _**orderBy**_ is used to order the collection
- _**search**_ is used to filter the collection
- _**isPersonalWorkspaceFirst**_ is used to place the personal workspace of the user on top of the others

#### Get Collection Workspace Response

Returns the workspaces and their total count.

```json
{
  "workspaces": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Workspace 1",
      "description": "This is the first workspace",
      "isPersonal": true,
      "isFavorite": true,
      "owner": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan"
      }
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Workspace 2",
      "description": "This is the second workspace",
      "isPersonal": false,
      "isFavorite": true,
      "owner": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Andreas Donner"
      }
    }
  ],
  "totalCount": 4
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
  "name": "Workspace Name"
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
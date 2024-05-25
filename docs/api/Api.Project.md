# Consilium Tempus API

* [Project](#project)
  * [Get](#get)
    * [Get Request](#get-request)
    * [Get Response](#get-response)
  * [Get Overview](#get-overview)
    * [Get Overview Request](#get-overview-request)
    * [Get Overview Response](#get-overview-response)
  * [Get Collection](#get-collection)
    * [Get Collection Request](#get-collection-request)
    * [Get Collection Response](#get-collection-response)
  * [Create](#create)
    * [Create Project Request](#create-project-request)
    * [Create Project Response](#create-project-response)
  * [Update](#update)
    * [Update Project Request](#update-project-request)
    * [Update Project Response](#update-project-response)
  * [Update Overview](#update-overview)
    * [Update Overview Project Request](#update-overview-project-request)
    * [Update Overview Project Response](#update-overview-project-response)
  * [Delete](#delete)
    * [Delete Project Request](#delete-project-request)
    * [Delete Project Response](#delete-project-response)

## Project

This is the controller that takes care of creating, updating, deleting and querying Projects.

### Get

Anyone that is part of the workspace can read a project
([Read Project Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/projects/{id}
```

- **id** is a 36-character string

#### Get Request

Sends the id of the project inside the route of the request.

#### Get Response

Returns a project.

```json
{
  "name": "Project Name 1",
  "isFavorite": false,
  "isPrivate": true
}
```

### Get Overview

Anyone that is part of the workspace can read a project overview
([Read Project Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/projects/overview/{id}
```

- **id** is a 36-character string

#### Get Overview Request

Sends the id of the project inside the route of the request.

#### Get Overview Response

Returns a project overview.

```json
{
  "description": "This is the description of the project"
}
```

### Get Collection

Anyone that is part of the workspace can read the projects
([Read Collection Project Permission](../Security.md/#permissions)),
in case the workspace Id is mentioned, otherwise any logged-in user will get their projects.

```js
GET {{host}}/api/projects?pageSize=2&currentPage=1&orders=name.desc, last_activity.asc&workspaceId=10000000-0000-0000-0000-000000000000&name=project&isFavorite=true&isPrivate=false
```

#### Get Collection Request

it sends optional query parameters to paginate, order or filter the projects by workspace, name, etc.

- _**pageSize**_ is used to specify the size of the page
- _**currentPage**_ is used to specify the current page
- _**orders**_ is used to order the collection
- _**workspaceId**_ is used to order the projects by workspace
- _**name**_ is used to filter by name
- _**isFavorite**_ is used to filter by favorites
- _**isPrivate**_ is used to filter by accessibility

#### Get Collection Response

Returns the projects, their total count and total pages, if paginated.

```json
{
  "projects": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Project Name 1",
      "description": "This is the first project",
      "isFavorite": true,
      "isPrivate": false
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Project Name 2",
      "description": "This is the second project",
      "isFavorite": true,
      "isPrivate": false
    }
  ],
  "totalCount": 5,
  "totalPages": 3
}
```

### Create

Only admin users that are part of the workspace can create a project
([Create Project Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects
```

#### Create Project Request

Sends body data that the new project needs to be created.

```json
{
  "workspaceId": "10000000-0000-0000-0000-000000000000",
  "name": "Project Name",
  "isPrivate": false
}
```

#### Create Project Response

Returns a confirmation message that the project has been created successfully.

### Update

All members that are part of the workspace can update a project
([Update Project Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects
```

#### Update Project Request

Sends body data that the project needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "New Project Name",
  "isFavorite": false
}
```

#### Update Project Response

Returns a confirmation message that the project has been updated successfully.

### Update Overview

All members that are part of the workspace can update a project overview
([Update Project Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/overview
```

#### Update Overview Project Request

Sends body data that the project overview needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "description": "New Project description that is more relevant than the last one"
}
```

#### Update Overview Project Response

Returns a confirmation message that the project overview has been updated successfully.

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
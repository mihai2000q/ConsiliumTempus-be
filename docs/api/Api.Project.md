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
  * [Get Statuses](#get-statuses)
    * [Get Statuses From Project Request](#get-statuses-from-project-request)
    * [Get Statuses From Project Request](#get-statuses-from-project-response)
  * [Create](#create)
    * [Create Project Request](#create-project-request)
    * [Create Project Response](#create-project-response)
  * [Add Status](#add-status)
    * [Add Status To Project Request](#add-status-to-project-request)
    * [Add Status To Project Response](#add-status-to-project-response)
  * [Update](#update)
    * [Update Project Request](#update-project-request)
    * [Update Project Response](#update-project-response)
  * [Update Favorites](#update-favorites)
    * [Update Favorites Project Request](#update-favorites-project-request)
    * [Update Favorites Project Response](#update-favorites-project-response)
  * [Update Overview](#update-overview)
    * [Update Overview Project Request](#update-overview-project-request)
    * [Update Overview Project Response](#update-overview-project-response)
  * [Update Status](#update-status)
    * [Update Status From Project Request](#update-status-from-project-request)
    * [Update Status From Project Response](#update-status-from-project-response)
  * [Delete](#delete)
    * [Delete Project Request](#delete-project-request)
    * [Delete Project Response](#delete-project-response)
  * [Remove Status](#remove-status)
    * [Remove Status From Project Request](#remove-status-from-project-request)
    * [Remove Status From Project Response](#remove-status-from-project-response)

## Project

This is the controller that takes care of creating, updating, deleting and querying Projects and their statuses.

### Get

Anyone that is part of the workspace can read a project
([Read Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can read it
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

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
  "lifecycle": "Active",
  "owner": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Michael Jordan",
    "email": "michael@jordan.com"
  },
  "isPrivate": true,
  "latestStatus": {
    "id": "10000000-0000-0000-0000-000000000000",
    "title": "Status 2",
    "status": "OnTrack",
    "createdBy": {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Michael Jordan",
      "email": "michael@jordan.com"
    },
    "createdDateTime": "2020-01-01T00:00:00.0000000Z",
    "updatedBy": {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Michael Jordan",
      "email": "michael@jordan.com"
    },
    "updatedDateTime": "2020-01-01T00:00:00.0000000Z"
  },
  "workspace": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Basketball Group"
  }
}
```

### Get Overview

Anyone that is part of the workspace can read a project overview
([Read Overview Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can read its overview
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

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
GET {{host}}/api/projects?pageSize=2&currentPage=1&orderBy=name.desc&orderBy=last_activity.asc&search=name sw project&search=is_favorite eq true&workspaceId=10000000-0000-0000-0000-000000000000
```

#### Get Collection Request

It sends optional query parameters to paginate, order or filter the projects by workspace, name, etc.

- **pageSize** is used to specify the size of the page
- **currentPage** is used to specify the current page
- **orderBy** is used to order the collection
- **search** is used to filter the collection
- **workspaceId** is used to filter the projects by workspace

#### Get Collection Response

Returns the projects and their total count.

```json
{
  "projects": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Project Name 1",
      "description": "This is the first project",
      "isFavorite": true,
      "lifecycle": "Active",
      "owner": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      },
      "isPrivate": false,
      "latestStatus": {
        "id": "10000000-0000-0000-0000-000000000000",
        "status": "Completed",
        "updatedDateTime": "2020-01-01T00:00:00.0000000Z"
      },
      "createdDateTime": "2020-01-01T00:00:00.0000000Z"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Project Name 2",
      "description": "This is the second project",
      "isFavorite": true,
      "lifecycle": "Archived",
      "owner": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      },
      "isPrivate": false,
      "latestStatus": {
        "id": "10000000-0000-0000-0000-000000000000",
        "status": "Completed",
        "updatedDateTime": "2020-01-01T00:00:00.0000000Z"
      },
      "createdDateTime": "2020-01-01T00:00:00.0000000Z"
    }
  ],
  "totalCount": 5
}
```

### Get Statuses

Anyone that is part of the workspace can read the project statuses
([Read Statuses From Project Permission](../Security.md/#permissions)),

When the project is private, only allowed members can read the statuses
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
GET {{host}}/api/projects/{id}/statuses
```

- **id** is a 36-character string

#### Get Statuses From Project Request

It sends the id of the project on the route of the request.

#### Get Statuses From Project Response

Returns the project statuses and their total count.

```json
{
  "statuses": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "title": "Status 1",
      "status": "OffTrack",
      "description": "This is the description of the first status",
      "createdBy": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      },
      "createdDateTime": "2020-01-01T00:00:00.0000000Z",
      "updatedBy": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      },
      "updatedDateTime": "2020-01-01T00:00:00.0000000Z"
    },
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "title": "Status 2",
      "status": "OnTrack",
      "description": "This is the description of the second status",
      "createdBy": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      },
      "createdDateTime": "2020-01-01T00:00:00.0000000Z",
      "updatedBy": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michael@jordan.com"
      },
      "updatedDateTime": "2020-01-01T00:00:00.0000000Z"
    }
  ],
  "totalCount": 5
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

### Add Status

Only admin users that are part of the workspace can add a status to the project
([Add Status To Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can add a status
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
POST {{host}}/api/projects/add-status
```

#### Add Status To Project Request

Sends body data that the new project status needs to be created.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "title": "Status Update",
  "status": "AtRisk",
  "description": "This status marks the start of a new beginning"
}
```

#### Add Status To Project Response

Returns a confirmation message that the project status has been added successfully.

### Update

All members that are part of the workspace can update a project
([Update Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can update it
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
PUT {{host}}/api/projects
```

#### Update Project Request

Sends body data that the project needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "New Project Name",
  "lifecycle": "Active"
}
```

#### Update Project Response

Returns a confirmation message that the project has been updated successfully.

### Update Favorites

Anyone that is part of the workspace can add the project to favorites
([Update Favorites Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can add the project to favorites
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
PUT {{host}}/api/projects/favorites
```

#### Update Favorites Project Request

Sends body data that the project favorites need to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "isFavorite": true
}
```

#### Update Favorites Project Response

Returns a confirmation message that the project favorites have been updated successfully.

### Update Overview

All members that are part of the workspace can update a project overview
([Update Overview Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can update its overview
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

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

### Update Status

Only members and admin users that are part of the workspace can update a status from the project
([Update Status To Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can update a status
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
PUT {{host}}/api/projects/update-status
```

#### Update Status From Project Request

Sends body data that the project status needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "statusId": "11000000-0000-0000-0000-000000000000",
  "title": "Status Update",
  "status": "AtRisk",
  "description": "This status marks the start of a new beginning"
}
```

#### Update Status From Project Response

Returns a confirmation message that the project status has been updated successfully.

### Delete

Only admin users that are part of the workspace can delete a project
([Delete Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can delete a project
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
DELETE {{host}}/api/projects/{id}
```

- **id** is a 36 characters strings

#### Delete Project Request

Sends the id of the project inside the route request.

#### Delete Project Response

Returns a confirmation message that the project has been deleted successfully.

### Remove Status

Only admin users that are part of the workspace can remove a status from the project
([Remove Status To Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can remove a status
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
DELETE {{host}}/api/projects/{id}/remove-status/{statusId}
```

- **id** is a 36-character string
- **statusId** is a 36-character string

#### Remove Status From Project Request

Sends the id of the project and the id of the status inside the route request.

#### Remove Status From Project Response

Returns a confirmation message that the project status has been removed successfully.
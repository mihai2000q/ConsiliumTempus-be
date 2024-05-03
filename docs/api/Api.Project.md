# Consilium Tempus API

* [Project](#project)
  * [Get](#get)
    * [Get Request](#get-request)
    * [Get Response](#get-response)
  * [Get Collection](#get-collection)
    * [Get Collection](#get-collection-request)
    * [Get Collection](#get-collection-response)
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
  "description": "This is the first project",
  "isFavorite": false,
  "isPrivate": true
}
```


### Get Collection

Anyone that is part of the workspace can read the projects
([Read Collection Project Permission](../Security.md/#permissions)),
in case the workspace Id is mentioned, otherwise any logged-in user will get their projects.

```js
GET {{host}}/api/projects?pageSize=2&currentPage=1&order=name.desc&workspaceId=10000000-0000-0000-0000-000000000000&name=project&isFavorite=true&isPrivate=false
```

#### Get Collection Request

it sends optional query parameters to paginate, order or filter the projects by workspace, name, etc.

- **pageSize** is used to specify the size of the page
- **currentPage** is used to specify the current page
- **order** is used to order the collection
- **workspaceId** is used to order the projects by workspace
- **name** is used to filter by name
- **isFavorite** is used to filter by favorites
- **isPrivate** is used to filter by accessibility

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
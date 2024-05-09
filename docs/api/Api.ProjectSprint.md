# Consilium Tempus API

* [Project Sprint](#project-sprint)
  * [Get](#get)
    * [Get Project Sprint Request](#get-project-sprint-request)
    * [Get Project Sprint Response](#get-project-sprint-response)
  * [Get Collection](#get-collection)
    * [Get Collection Project Sprint Request](#get-collection-project-sprint-request)
    * [Get Collection Project Sprint Response](#get-collection-project-sprint-response)
  * [Create](#create)
    * [Create Project Sprint Request](#create-project-sprint-request)
    * [Create Project Sprint Response](#create-project-sprint-response)
  * [Update](#update)
    * [Update Project Sprint Request](#update-project-sprint-request)
    * [Update Project Sprint Response](#update-project-sprint-response)
  * [Delete](#delete)
    * [Delete Project Sprint Request](#delete-project-sprint-request)
    * [Delete Project Sprint Response](#delete-project-sprint-response)

## Project Sprint

This is the controller that takes care of creating, updating, deleting and querying Project Sprints.


### Get

Anyone that is part of a workspace can access the project sprints
([Get Project Sprint Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/sprints/{id}
```

- **id** is a 36 characters strings

#### Get Project Sprint Request

Sends the id of the sprint as part of the route.

#### Get Project Sprint Response

Returns the project sprint.

```json
{
  "name": "Sprint 1",
  "startDate": "2022-10-10",
  "endDate": "2022-10-24"
}
```


### Get Collection

Anyone that is part of a workspace can access the project sprints
([Get Collection Project Sprint Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/sprints?projectId=10000000-0000-0000-0000-000000000000
```

#### Get Collection Project Sprint Request

Sends the project id as a query parameter.

- **projectId** is used to specify the project that the sprint is part of

#### Get Collection Project Sprint Response

Returns the project sprints.

```json
{
  "sprints": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Sprint 1",
      "startDate": "2022-10-10",
      "endDate": "2022-10-24"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Sprint 2",
      "startDate": null,
      "endDate": null
    }
  ]
}
```


### Create

Only admin users that are part of the workspace can create a project
([Create Project Sprint Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/sprints
```

#### Create Project Sprint Request

Sends body data that the new project sprint needs to be created.

```json
{
  "projectId": "10000000-0000-0000-0000-000000000000",
  "name": "Project Sprint Name",
  "startDate": "2024-01-01",
  "endDate": "2024-01-14"
}
```

#### Create Project Sprint Response

Returns a confirmation message that the sprint has been created successfully.


### Update

Only admin and member users that are part of the workspace can create a project
([Update Project Sprint Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/sprints
```

#### Update Project Sprint Request

Sends body data that the project sprint needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "New Project Sprint Name",
  "startDate": null,
  "endDate": null
}
```

#### Update Project Sprint Response

Returns a confirmation message that the sprint has been updated successfully.


### Delete

Only admin users that are part of the workspace can delete a project
([Delete Project Sprint Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/projects/sprints/{id}
```

- **id** is a 36 characters strings

#### Delete Project Sprint Request

Sends the id of the sprint inside the route request.

#### Delete Project Sprint Response

Returns a confirmation message that the sprint has been deleted successfully.
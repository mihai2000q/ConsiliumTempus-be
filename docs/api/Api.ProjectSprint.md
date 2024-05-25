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
  * [Add Stage](#add-stage)
    * [Add Stage To Project Sprint Request](#add-stage-to-project-sprint-request)
    * [Add Stage To Project Sprint Response](#add-stage-to-project-sprint-response)
  * [Update](#update)
    * [Update Project Sprint Request](#update-project-sprint-request)
    * [Update Project Sprint Response](#update-project-sprint-response)
  * [Update Stage](#update-stage)
    * [Update Stage From Project Sprint Request](#update-stage-from-project-sprint-request)
    * [Update Stage From Project Sprint Response](#update-stage-from-project-sprint-response)
  * [Delete](#delete)
    * [Delete Project Sprint Request](#delete-project-sprint-request)
    * [Delete Project Sprint Response](#delete-project-sprint-response)
  * [Remove Stage](#remove-stage)
    * [Remove Stage From Project Sprint Request](#remove-stage-from-project-sprint-request)
    * [Remove Stage From Project Sprint Response](#remove-stage-from-project-sprint-response)

## Project Sprint

This is the controller that takes care of creating, updating, deleting and querying Project Sprints.
Additionally, Project Stages can be added through the project sprints.

### Get

Anyone that is part of a workspace can access the project sprints
([Get Project Sprint Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/projects/sprints/{id}
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
  "endDate": "2022-10-24",
  "stages": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "To Do"
    },
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Done"
    }
  ]
}
```

### Get Collection

Anyone that is part of a workspace can access the project sprints
([Get Collection Project Sprint Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/projects/sprints?projectId=10000000-0000-0000-0000-000000000000
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

Only admin users that are part of the workspace can create a project sprint
([Create Project Sprint Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/sprints
```

#### Create Project Sprint Request

Sends body data that the new project sprint needs to be created.
It can also specify if the stages of the previous sprint (if any) can be kept/copied.

```json
{
  "projectId": "10000000-0000-0000-0000-000000000000",
  "name": "Project Sprint Name",
  "startDate": "2024-01-01",
  "endDate": "2024-01-14",
  "keepPreviousStages": true
}
```

#### Create Project Sprint Response

Returns a confirmation message that the sprint has been created successfully.

### Add Stage

Only admin users that are part of the workspace can add a project stage
([Add Stage To Project Sprint Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/sprints/add-stage
```

#### Add Stage To Project Sprint Request

Sends body data that the new project stage needs to be created.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "Project Sprint Name",
  "onTop": false
}
```

#### Add Stage To Project Sprint Response

Returns a confirmation message that the stage has been added successfully.

### Update

Only admin and member users that are part of the workspace can create a project sprint
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

### Update Stage

Only admin and member users that are part of the workspace can update a project sprint
([Update Stage From Project Sprint Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/sprints/update-stage
```

#### Update Stage From Project Sprint Request

Sends body data that the project stage needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "stageId": "10000000-0000-0000-0000-000000000000",
  "name": "In Transit"
}
```

#### Update Stage From Project Sprint Response

Returns a confirmation message that the stage has been updated successfully.

### Delete

Only admin users that are part of the workspace can delete a project sprint
([Delete Project Sprint Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/projects/sprints/{id}
```

- **id** is a 36 characters strings

#### Delete Project Sprint Request

Sends the id of the sprint inside the route request.

#### Delete Project Sprint Response

Returns a confirmation message that the sprint has been deleted successfully.

### Remove Stage

Only admin users that are part of the workspace can remove a project stage
([Remove Stage From Project Sprint Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/projects/sprints/{id}/remove-stage/{stageId}
```

- **id** is a 36-character strings
- **stageId** is a 36-character strings

#### Remove Stage From Project Sprint Request

Send the ids of the sprint and stage inside the route request.

#### Remove Stage From Project Sprint Response

Returns a confirmation message that the stage has been deleted successfully.
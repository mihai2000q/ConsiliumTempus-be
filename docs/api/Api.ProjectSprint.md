# Consilium Tempus API

* [Project Sprint](#project-sprint)
    * [Create](#create)
        * [Create Project Sprint Request](#create-project-sprint-request)
        * [Create Project Sprint Response](#create-project-sprint-response)
    * [Delete](#delete)
        * [Delete Project Sprint Request](#delete-project-sprint-request)
        * [Delete Project Sprint Response](#delete-project-sprint-response)

## Project Sprint

This is the controller that takes care of creating, updating and deleting a Project Sprint.


### Create

Only admin users that are part of the workspace can create a project
([Create Project Sprint Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/sprints
```

#### Create Project Sprint Request

Sends body data that the new workspace needs to be created.

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
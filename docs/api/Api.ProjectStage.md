# Consilium Tempus API

* [Project Stage](#project-stage)
  * [Create](#create)
    * [Create Project Stage Request](#create-project-stage-request)
    * [Create Project Stage Response](#create-project-stage-response)
  * [Update](#update)
    * [Update Project Stage Request](#update-project-stage-request)
    * [Update Project Stage Response](#update-project-stage-response)
  * [Delete](#delete)
    * [Delete Project Stage Request](#delete-project-stage-request)
    * [Delete Project Stage Response](#delete-project-stage-response)

## Project Stage

This is the controller that takes care of creating, updating and deleting Project Stages.


### Create

Only admin users that are part of the workspace can create a project
([Create Project Stage Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/projects/stages
```

#### Create Project Stage Request

Sends body data that the new workspace needs to be created.

```json
{
  "projectSprintId": "10000000-0000-0000-0000-000000000000",
  "name": "Project Stage Name"
}
```

#### Create Project Stage Response

Returns a confirmation message that the stage has been created successfully.


### Update

Only admin and member users that are part of the workspace can create a project
([Update Project Stage Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/projects/stages
```

#### Update Project Stage Request

Sends body data that the project sprint needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "New Project Stage Name"
}
```

#### Update Project Stage Response

Returns a confirmation message that the stage has been updated successfully.


### Delete

Only admin users that are part of the workspace can delete a project
([Delete Project Stage Permission](../Security.md/#permissions)).

```js
DELETE {{host}}/api/projects/stages/{id}
```

- **id** is a 36 characters strings

#### Delete Project Stage Request

Sends the id of the stage inside the route request.

#### Delete Project Stage Response

Returns a confirmation message that the stage has been deleted successfully.
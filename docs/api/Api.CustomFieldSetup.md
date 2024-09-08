# Consilium Tempus API

* [Custom Field Setup](#custom-field-setup)
  * [Get](#get)
    * [Get Request](#get-request)
    * [Get Response](#get-response)
  * [Get Collection From Workspace](#get-collection-from-workspace)
    * [Get Collection From Workspace Request](#get-collection-from-workspace-request)
    * [Get Collection From Workspace Response](#get-collection-from-workspace-response)
  * [Get Collection From Project](#get-collection-from-project)
    * [Get Collection From Project Request](#get-collection-from-project-request)
    * [Get Collection From Project Response](#get-collection-from-project-response)
  * [Create On Workspace](#create-on-workspace)
    * [Create Custom Field Setup On Workspace Request](#create-custom-field-setup-on-workspace-request)
    * [Create Custom Field Setup On Workspace Response](#create-custom-field-setup-on-workspace-response)
  * [Create On Project](#create-on-project)
    * [Create Custom Field Setup On Project Request](#create-custom-field-setup-on-project-request)
    * [Create Custom Field Setup On Project Response](#create-custom-field-setup-on-project-response)
  * [Add To Project](#add-to-project)
    * [Add Custom Field Setup To Project Request](#add-custom-field-setup-to-project-request)
    * [Add Custom Field Setup To Project Response](#add-custom-field-setup-to-project-response)
  * [Update](#update)
    * [Update Custom Field Setup Request](#update-custom-field-setup-request)
    * [Update Custom Field Setup Response](#update-custom-field-setup-response)
  * [Make Global](#make-global)
    * [Make Custom Field Setup Global Request](#make-custom-field-setup-global-request)
    * [Make Custom Field Setup Global Response](#make-custom-field-setup-global-response)
  * [Delete](#delete)
    * [Delete Custom Field Setup Request](#delete-custom-field-setup-request)
    * [Delete Custom Field Setup Response](#delete-custom-field-setup-response)
  * [Remove From Project](#remove-from-project)
    * [Delete Custom Field Setup Request](#delete-custom-field-setup-request-1)
    * [Delete Custom Field Setup Response](#delete-custom-field-setup-response-1)

## Custom Field Setup

This is the controller that takes care of creating, updating, deleting and querying Custom Field Setups.

### Get

Only members and admins of the workspace can read a custom field setup
([Read Project Task Permission](../Security.md/#permissions)).

When the project is private, only allowed members can read a custom field setup
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
GET {{host}}/api/customFieldSetups/{id}
```

- **id** is a 36-character string

#### Get Request

Sends the id of the custom Field Setup inside the route of the request.

#### Get Response

Returns a custom field setup. However, the response will be different for each type.

##### Date

```json
{
  "name": "the Date",
  "description": "This is a custom field",
  "type": "Date",
  "defaultDate": "2022-10-10"
}
```

##### Date Time

```json
{
  "name": "Date and time",
  "description": "This is a custom field",
  "type": "DateTime",
  "defaultDateTime": "2022-10-10T10:30:00"
}
```

##### Duration

```json
{
  "name": "the Duration",
  "description": "This is a custom field",
  "type": "Duration",
  "defaultDuration": "10:30:00"
}
```

##### Multi Select

```json
{
  "name": "Priorities",
  "description": "This is a custom field",
  "type": "Multi Select",
  "options": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "color": "#FF0000",
      "value": "High"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "color": "#0000FF",
      "value": "Low"
    }
  ]
}
```

##### Number

```json
{
  "name": "Budget",
  "description": "This is a custom field",
  "type": "Number",
  "settings": {
    "currencyCode": "USD",
    "decimals": 2,
    "rounding": true
  },
  "defaultNumber": 12
}
```

##### People

```json
{
  "name": "Guys",
  "description": "This is a custom field",
  "type": "People"
}
```

##### Single Select

```json
{
  "name": "Priority",
  "description": "This is a custom field",
  "type": "Multi Select",
  "options": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "color": "#FF0000",
      "value": "High"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "color": "#0000FF",
      "value": "Low"
    }
  ],
  "defaultOption": {
    "id": "20000000-0000-0000-0000-000000000000",
    "color": "#0000FF",
    "value": "Low"
  }
}
```

##### Text

```json
{
  "name": "Some Text",
  "description": "This is a custom field",
  "type": "Text",
  "defaultText": "default"
}
```

##### Time

```json
{
  "name": "the time",
  "description": "This is a custom field",
  "type": "Time",
  "defaultTime": "10:30:00"
}
```

### Get Collection From Workspace

Only members and admins of the workspace can read the custom field setups
([Read Collection Custom Field Setup From Workspace Permission](../Security.md/#permissions)),

```js
GET {{host}}/api/customFieldSetups/{workspaceId}
```

- **workspaceId** is a 36-character string

#### Get Collection From Workspace Request

Sends the id of the workspace on the route.

#### Get Collection From Workspace Response

Returns the custom field setups from workspace.

```json
{
  "customFieldSetups": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Custom Field Setup 1",
      "description": "This is custom field 1",
      "type": "Number"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Custom Field Setup 2",
      "description": "This is custom field 2",
      "type": "Date"
    }
  ]
}
```

### Get Collection From Project

Only members and admins of the workspace can read the custom field setups
([Read Collection Custom Field Setup From Project Permission](../Security.md/#permissions)),

When the project is private, only allowed members can read custom field setups
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
GET {{host}}/api/customFieldSetups/{projectId}
```

- **projectId** is a 36-character string

#### Get Collection From Project Request

Send the id of the project on the route.

#### Get Collection From Project Response

Returns the custom field setups from a project.

```json
{
  "customFieldSetups": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "name": "Custom Field Setup 1",
      "description": "This is custom field 1",
      "type": "Number"
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Custom Field Setup 2",
      "description": "This is custom field 2",
      "type": "Date"
    }
  ]
}
```

### Create On Workspace

Only admins of the workspace can create a custom field setup on workspace
([Create Custom Field Setup On Workspace Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/customFieldSetups/workspace
```

#### Create Custom Field Setup On Workspace Request

Sends body data that the new custom field setup needs to be created on workspace.

```json
{
  "workspaceId": "10000000-0000-0000-0000-000000000000",
  "name": "New Custom Field Setup",
  "description": "its description",
  "type": "Date",
  "dateCustomFieldSetup": {
    "defaultDate": "2022-10-10"
  },
  "dateTimeCustomFieldSetup": null,
  "durationCustomFieldSetup": null,
  "multiSelectCustomFieldSetup": null,
  "numberCustomFieldSetup": null,
  "singleSelectCustomFieldSetup": null,
  "textCustomFieldSetup": null,
  "timeCustomFieldSetup": null
}
```

#### Create Custom Field Setup On Workspace Response

Returns a confirmation message that the custom field setup has been created successfully.

### Create On Project

Only admins of the workspace can create a custom field setup
([Create Custom Field Setup On Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can create a custom field setup
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
POST {{host}}/api/customFieldSetups/project
```

#### Create Custom Field Setup On Project Request

Sends body data that the new custom field setup needs to be created on a project.

```json
{
  "projectId": "10000000-0000-0000-0000-000000000000",
  "name": "New Custom Field Setup",
  "description": "its description",
  "type": "MultiSelect",
  "dateCustomFieldSetup": null,
  "dateTimeCustomFieldSetup": null,
  "durationCustomFieldSetup": null,
  "multiSelectCustomFieldSetup": {
    "options": [
      {
        "value": "High",
        "color": "#FF1122"
      },
      {
        "value": "Low",
        "color": "#2211FF"
      }
    ]
  },
  "numberCustomFieldSetup": null,
  "singleSelectCustomFieldSetup": null,
  "textCustomFieldSetup": null,
  "timeCustomFieldSetup": null
}
```

#### Create Custom Field Setup On Project Response

Returns a confirmation message that the custom field setup has been created successfully.

### Add To Project

Only admins of the workspace can add the custom field setup to project
([Add Custom Field Setup To Project Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/customFieldSetups/add-project
```

#### Add Custom Field Setup To Project Request

Sends body data that the custom field setup needs to be added to project.

```json
{
  "id": "11000000-0000-0000-0000-000000000000",
  "projectId": "10000000-0000-0000-0000-000000000000"
}
```

#### Add Custom Field Setup To Project Response

Returns a confirmation message that the custom field setup has been added to the project successfully.

### Update

Only admins and members of the workspace can update a custom field setup
([Create Custom Field Setup On Project Permission](../Security.md/#permissions)).

When the project is private, only allowed members can update a custom field setup
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
PUT {{host}}/api/customFieldSetups
```

#### Update Custom Field Setup Request

Sends body data that the custom field setup needs to be updated.

```json
{
  "projectId": "10000000-0000-0000-0000-000000000000",
  "name": "Custom Field",
  "description": "its description",
  "type": "MultiSelect",
  "dateCustomFieldSetup": null,
  "dateTimeCustomFieldSetup": null,
  "durationCustomFieldSetup": null,
  "multiSelectCustomFieldSetup": {
    "operation": "Add",
    "newOption": {
      "value": "High",
      "color": "#FF1122"
    }
  },
  "numberCustomFieldSetup": null,
  "singleSelectCustomFieldSetup": null,
  "textCustomFieldSetup": null,
  "timeCustomFieldSetup": null
}
```

#### Update Custom Field Setup Response

Returns a confirmation message that the custom field setup has been updated successfully.

### Make Global

Only admins of the workspace can make the custom field setup global
([Make Custom Field Setup Global Permission](../Security.md/#permissions)).

When the project is private, only allowed members can make the custom field setup global
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
PUT {{host}}/api/customFieldSetups/global
```

#### Make Custom Field Setup Global Request

Sends body data that the custom field setup needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000"
}
```

#### Make Custom Field Setup Global Response

Returns a confirmation message that the custom field setup has been made global successfully.

### Delete

Only admins of the workspace can delete a custom field setup
([Delete Custom Field Setup Permission](../Security.md/#permissions)).

When the project is private, only allowed members can delete a custom field setup
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
DELETE {{host}}/api/customFieldSetups/{id}
```

- **id** is a 36 characters strings

#### Delete Custom Field Setup Request

Sends the id of the custom field setup on the route.

#### Delete Custom Field Setup Response

Returns a confirmation message that the custom field setup has been deleted successfully.

### Remove From Project

Only admins of the workspace can remove a custom field setup from a project
([Make Custom Field Setup Global Permission](../Security.md/#permissions)).

When the project is private, only allowed members can remove a custom field setup from a project
([Project Authorization Level: Is Allowed](../Security.md/#project-authorization-levels)).

```js
DELETE {{host}}/api/customFieldSetups/{id}/remove-project/{projectId}
```

- **id** is a 36 characters strings
- **projectId** is a 36 characters strings

#### Delete Custom Field Setup Request

Send the id of the custom field setup and of the project on the route.

#### Delete Custom Field Setup Response

Returns a confirmation message that the custom field setup has been removed from the project successfully.

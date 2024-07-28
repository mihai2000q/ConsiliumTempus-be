# Consilium Tempus API

* [Workspace](#workspace)
  * [Get](#get)
    * [Get Workspace Request](#get-workspace-request)
    * [Get Workspace Response](#get-workspace-response)
  * [Get Overview](#get-overview)
    * [Get Overview Workspace Request](#get-overview-workspace-request)
    * [Get Overview Workspace Response](#get-overview-workspace-response)
  * [Get Collection](#get-collection)
    * [Get Collection Workspace Request](#get-collection-workspace-request)
    * [Get Collection Workspace Response](#get-collection-workspace-response)
  * [Get Collaborators](#get-collaborators)
    * [Get Collaborators From Workspace Request](#get-collaborators-from-workspace-request)
    * [Get Collaborators From Workspace Response](#get-collaborators-from-workspace-response)
  * [Get Invitations](#get-invitations)
    * [Get Invitations Workspace Request](#get-invitations-workspace-request)
    * [Get Invitations Workspace Response](#get-invitations-workspace-response)
  * [Create](#create)
    * [Create Workspace Request](#create-workspace-request)
    * [Create Workspace Response](#create-workspace-response)
  * [Invite Collaborator](#invite-collaborator)
    * [Invite Collaborator To Workspace Request](#invite-collaborator-to-workspace-request)
    * [Invite Collaborator To Workspace Response](#invite-collaborator-to-workspace-response)
  * [Accept Invitation](#accept-invitation)
    * [Accept Invitation To Workspace Request](#accept-invitation-to-workspace-request)
    * [Accept Invitation To Workspace Response](#accept-invitation-to-workspace-response)
  * [Reject Invitation](#reject-invitation)
    * [Reject Invitation To Workspace Request](#reject-invitation-to-workspace-request)
    * [Reject Invitation To Workspace Response](#reject-invitation-to-workspace-response)
  * [Leave](#leave)
    * [Leave Workspace Request](#leave-workspace-request)
    * [Leave Workspace Response](#leave-workspace-response)
  * [Update](#update)
    * [Update Workspace Request](#update-workspace-request)
    * [Update Workspace Response](#update-workspace-response)
  * [Update Favorites](#update-favorites)
    * [Update Favorites Workspace Request](#update-favorites-workspace-request)
    * [Update Favorites Workspace Response](#update-favorites-workspace-response)
  * [Update Overview](#update-overview)
    * [Update Overview Workspace Request](#update-overview-workspace-request)
    * [Update Overview Workspace Response](#update-overview-workspace-response)
  * [Update Owner](#update-owner)
    * [Update Owner Workspace Request](#update-owner-workspace-request)
    * [Update Owner Workspace Response](#update-owner-workspace-response)
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

Returns the workspace.

```json
{
  "name": "Workspace Name",
  "isFavorite": true,
  "isPersonal": false,
  "owner": {
    "id": "10000000-0000-0000-0000-000000000000",
    "name": "Michael Jordan",
    "email": "michael@jordan.com"
  }
}
```

### Get Overview

Only users that are part of the workspace can retrieve it 
([Read Overview Workspace Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/workspaces/overview/{id}
```

- **id** is a 36 characters strings

#### Get Overview Workspace Request

Sends the id of the workspace inside the route request.

#### Get Overview Workspace Response

Returns the workspace overview.

```json
{
  "description": "This is the description of the workspace"
}
```

### Get Collection

Anyone logged in can request this data, but it will return only the workspaces that are linked to this user.

```js
GET {{host}}/api/workspaces?pageSize=2&currentPage=1orderBy=name.asc&orderBy=updated_date_time.desc&search=name ct worksp
```

### Get Collection Workspace Request

Sends optional query parameters for ordering, filtering, and page-based pagination.

- **pageSize** is used to specify the size of the page
- **currentPage** is used to specify the current page
- **orderBy** is used to order the collection
- **search** is used to filter the collection
- **isPersonalWorkspaceFirst** is used to place the personal workspace of the user on top of the others

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
        "name": "Michael Jordan",
        "email": "michaelj@gmail.com"
      }
    },
    {
      "id": "20000000-0000-0000-0000-000000000000",
      "name": "Workspace 2",
      "description": "This is the second workspace",
      "isPersonal": false,
      "isFavorite": true,
      "owner": {
        "id": "20000000-0000-0000-0000-000000000000",
        "name": "Andreas Donner",
        "email": "andreasd@gmail.com"
      }
    }
  ],
  "totalCount": 4
}
```

### Get Collaborators

Only users that are part of the workspace can retrieve it 
([Read Collaborators From Workspace Permission](../Security.md/#permissions)).

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

### Get Invitations

All users can see their invitations (sent or received), but only admins can see those from a workspace
([Read Invitations From Workspace Permission](../Security.md/#permissions)).

```js
GET {{host}}/api/workspaces/invitations?isSender=false&pageSize=2&currentPage=1
```

#### Get Invitations Workspace Request

Sends the following query parameters:

- **isSender** to get invitations where the current user is the sender, otherwise where the user is the invitee
- **workspaceId** to get invitations from a workspace
- **pageSize** to specify the size of the invitations returned
- **currentPage** to specify the current page 

#### Get Invitations Workspace Response

Returns the invitations and total count.

```json
{
  "invitations": [
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "sender": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael Jordan",
        "email": "michaelj@gmail.com"
      },
      "collaborator": {
        "id": "20000000-0000-0000-0000-000000000000",
        "name": "Benjamin Smith",
        "email": "bsmith@gmail.com"
      },
      "workspace": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Michael's Workspace",
        "isPersonal": true
      }
    },
    {
      "id": "10000000-0000-0000-0000-000000000000",
      "sender": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Thomson Mike",
        "email": "thomson_mike@gmail.com"
      },
      "collaborator": {
        "id": "20000000-0000-0000-0000-000000000000",
        "name": "Benjamin Smith",
        "email": "bsmith@gmail.com"
      },
      "workspace": {
        "id": "10000000-0000-0000-0000-000000000000",
        "name": "Thomson's Workspace",
        "isPersonal": false
      }
    }
  ],
  "totalCount": 2
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

### Invite Collaborator

Only admins of the workspace can invite new collaborators
([Invite Collaborator To Workspace Permission](../Security.md/#permissions)).

```js
POST {{host}}/api/workspaces/invite-collaborator
```

#### Invite Collaborator To Workspace Request

Sends body data needed to invite a new collaborator to the workspace.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "email": "benjamin_smith@gmail.com"
}
```

#### Invite Collaborator To Workspace Response

Returns a confirmation message that the invitation has been sent successfully.

### Accept Invitation

Anyone can accept an invitation to a workspace.

```js
POST {{host}}/api/workspaces/accept-invitation
```

#### Accept Invitation To Workspace Request

Sends body data needed to accept the invitation to the workspace.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "invitationId": "20000000-0000-0000-0000-000000000000"
}
```

#### Accept Invitation To Workspace Response

Returns a confirmation message that the invitation has been accepted successfully.

### Reject Invitation

Anyone can reject an invitation to a workspace.

```js
POST {{host}}/api/workspaces/reject-invitation
```

#### Reject Invitation To Workspace Request

Sends body data needed to reject the invitation to the workspace.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "invitationId": "20000000-0000-0000-0000-000000000000"
}
```

#### Reject Invitation To Workspace Response

Returns a confirmation message that the invitation has been rejected successfully.

### Leave

Only member or admin users that are part of the workspace can update it
([Workspace Authorization Level: Is Collaborator](../Security.md/#workspace-authorization-levels));

```js
PUT {{host}}/api/workspaces/leave
```

#### Leave Workspace Request

Sends body data that the workspace needs to be left.

```json
{
  "id": "10000000-0000-0000-0000-000000000000"
}
```

#### Leave Workspace Response

Returns a confirmation message that the workspace has been left successfully.

### Update

Only member or admin users that are part of the workspace can update it
([Update Workspace Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/workspaces
```

#### Update Workspace Request

Sends body data that the new workspace needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "name": "Workspace Name"
}
```

#### Update Workspace Response

Returns a confirmation message that the workspace has been updated successfully.

### Update Favorites

Anyone that is part of the workspace can add it to their favorites
([Update Favorites Workspace Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/workspaces/favorites
```

#### Update Favorites Workspace Request

Sends body data that the workspace needs to update favorites.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "isFavorite": false
}
```

#### Update Favorites Workspace Response

Returns a confirmation message that the workspace favorites have been updated successfully.

### Update Overview

Only member or admin users that are part of the workspace can update it
([Update Overview Workspace Permission](../Security.md/#permissions)).

```js
PUT {{host}}/api/workspaces/overview
```

#### Update Overview Workspace Request

Sends body data that the workspace overview needs to be updated.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "description": "This is the new description of the workspace overview"
}
```

#### Update Overview Workspace Response

Returns a confirmation message that the workspace overview has been updated successfully.

### Update Owner

Only owners of the workspace can update the owner
([Workspace Authorization Level: Is Owner](../Security.md/#workspace-authorization-levels));

```js
PUT {{host}}/api/workspaces/owner
```

#### Update Owner Workspace Request

Sends body data that the workspace needs to update the owner.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "ownerId": "10000000-0000-0000-0000-000000000000"
}
```

#### Update Owner Workspace Response

Returns a confirmation message that the workspace owner has been updated successfully.

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
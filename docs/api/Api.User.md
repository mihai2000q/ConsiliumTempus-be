# Consilium Tempus API

* [User](#user)
  * [Get](#get)
    * [Get User Request](#get-user-request)
    * [Get User Response](#get-user-response)
  * [Update](#update)
    * [Update User Request](#update-user-request)
    * [Update User Response](#update-user-response)
  * [Delete](#delete)
    * [Delete User Request](#delete-user-request)
    * [Delete User Response](#delete-user-response)

## User

This is the controller that takes care of reading, updating and deleting a User.


### Get

Anyone logged in can read the data of another user.

```js
GET {{host}}/api/users/{id}
```

- **id** is a 36 characters strings

#### Get User Request

Sends the id of the user inside the route request.

#### Get User Response

Returns the user [Dto](dto/Dto.User.md).


### Update

Only the owner of this endpoint (the user in question) can update their data.

```js
PUT {{host}}/api/users
```

#### Update User Request

Sends body data that the new workspace needs to be updated.
<br>
*Role* and *Date of Birth* are optional, but the first three are mandatory.

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "firstName": "New FirstName",
  "lastName": "New lastName",
  "role": null,
  "dateOfBirth": "2000-12-21"
}
```

#### Update User Response

Returns the new user [Dto](dto/Dto.User.md).


### Delete

Only the owner of this endpoint (the user in question) can update their data.

```js
DELETE {{host}}/api/users/{id}
```

- **id** is a 36 characters strings

#### Delete User Request

Sends the id of the user inside the route request.

#### Delete User Response

Returns the user [Dto](dto/Dto.User.md).

# Consilium Tempus API

* [User](#user)
  * [Get](#get)
    * [Get User Request](#get-user-request)
    * [Get User Response](#get-user-response)
  * [Get Current](#get-current)
    * [Get Current Response](#get-current-response)
  * [Update](#update)
    * [Update User Request](#update-user-request)
    * [Update User Response](#update-user-response)
  * [Delete](#delete)
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


### Get Current

Returns the current user based on the token provided.

```js
GET {{host}}/api/users/id
```

#### Get Current Response

Returns the current user [Dto](dto/Dto.User.md).


### Update

Only the owner of this endpoint (the user in question) can update their data 
(which is implicit, because the id is not passed, but read from the token).

```js
PUT {{host}}/api/users
```

#### Update User Request

Sends body data that the new user needs to be updated.

```json
{
  "firstName": "New FirstName",
  "lastName": "New lastName",
  "role": null,
  "dateOfBirth": "2000-12-21"
}
```

#### Update User Response

Returns the new user [Dto](dto/Dto.User.md).


### Delete

Only the owner can update their data (which is implicit, because the id is not passed, but read from the token).

```js
DELETE {{host}}/api/users
```

#### Delete User Response

Returns a confirmation message that the user has been deleted successfully.
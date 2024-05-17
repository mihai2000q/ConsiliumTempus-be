# Consilium Tempus API

* [User](#user)
  * [Get](#get)
    * [Get User Request](#get-user-request)
    * [Get User Response](#get-user-response)
  * [Get Current](#get-current)
    * [Get Current Response](#get-current-response)
  * [Update Current](#update)
    * [Update Current User Request](#update-user-request)
    * [Update Current User Response](#update-user-response)
  * [Delete Current](#delete-current)
    * [Delete Current User Response](#delete-current-user-response)

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

Returns the user data.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "firstName": "Firsty",
  "lastName": "Lasty",
  "email": "firstylasty@gmail.com",
  "role": "Software Developer"
}
```

### Get Current

Returns the current user based on the token provided.

```js
GET {{host}}/api/users/id
```

#### Get Current Response

Returns the current user data.

```json
{
  "id": "10000000-0000-0000-0000-000000000000",
  "firstName": "Firsty",
  "lastName": "Lasty",
  "email": "firstylasty@gmail.com",
  "role": "Software Developer",
  "dateOfBirth": "2000-12-23"
}
```

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

Returns a confirmation message that the user has been updated successfully.

### Delete Current

Only the owner can delete their data (which is implicit, because the id is not passed, but read from the token).

```js
DELETE {{host}}/api/users
```

#### Delete Current User Response

Returns a confirmation message that the user has been deleted successfully.
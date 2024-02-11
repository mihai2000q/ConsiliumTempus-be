# Consilium Tempus API

## User

This is the controller that takes care of reading, updating and deleting a User.

### Update

Only the owner of this endpoint (the user in question) can update their data.

```js
PUT {{host}}/api/users
```

#### Update User Request

Sends body data that the new workspace needs to be updated.
<br>
All parameters are optional except the id.

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "firstName": "New FirstName",
  "lastName": "New lastName"
}
```

#### Update User Response

Returns the new user [Dto](dto/Dto.User.md).

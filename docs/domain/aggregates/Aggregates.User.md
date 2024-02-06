# Domain Aggregates

## User

This is the **user** of the application. 
It holds sensitive data like the *password* or *email*. 
The *password* is encrypted using the BCrypt Algorithm.

```csharp
class User
{
    User Create()
    User Register()
}
```

```json
{
  "id": {
    "value": "00000000-0000-0000-0000-000000000000"
  },
  "credentials": {
    "email": "user@gmail.com",
    "password": "$2a$13$R1tGdA1LDsVG.Ge95l42oOEPQ2Xl/VvgMTkiQOODlrM5hQpISv0qC"
  },
  "name": {
    "first": "Tiffany",
    "last": "Doe"
  },
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "workspaces": [{}]
}
```

For database design checkout the [User Diagram](../diagrams/Diagram.User.md).

### Value Objects

- **Credentials**: it holds the email and password.
- **Name**: it holds the first and last name.

### Properties Validation

- The **First Name** cannot be longer than 100 characters
- The **Last Name** cannot be longer than 100 characters
- The **Email** cannot be longer than 100 characters
- The plain **Password** cannot be longer than 100 characters

### Domain Errors

- **Duplicate Email** when the email is already in use

### Domain Events

- **User Registered** when the user registers, it will automatically create a workspace
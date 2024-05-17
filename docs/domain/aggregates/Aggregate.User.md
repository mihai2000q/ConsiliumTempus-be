# Domain Aggregates

## User

This is the **user** of the application. 
It holds sensitive data like the hashed *password* or *email* and personal data of the user, like name or date of birth.

```csharp
class User
{
    User Register()
    void Update()
    void AddWorkspaceMembership()
}
```

```json
{
  "id": { "value": "00000000-0000-0000-0000-000000000000"},
  "credentials": {
    "email": "user@gmail.com",
    "password": "$2a$13$R1tGdA1LDsVG.Ge95l42oOEPQ2Xl/VvgMTkiQOODlrM5hQpISv0qC"
  },
  "firstName": { "value": "Tifanny" },
  "lastName": { "value": "Doe" },
  "role": { "value": "Software Developer" },
  "dateOfBirth": "2000-12-21",
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "memberships": [{}]
}
```

For database design, check out the [User Diagram](../diagrams/aggregates/Diagram.User.md).

### Value Objects

- **Credentials**, which holds the *Email* and *Password*
- **FirstName**, which holds a string encapsulated *Value*
- **LastName**, which holds a string encapsulated *Value*
- **Role**, which holds a string encapsulated *Value*

### Properties Validation

- The **First Name** cannot be longer than 100 characters
- The **Last Name** cannot be longer than 100 characters
- The **Email** cannot be longer than 100 characters
- The plain **Password** cannot be longer than 100 characters
- The **Role** cannot be longer than 50 characters

### Domain Errors

- **Duplicate Email** when the email is already in use
- **Not Found** when the user cannot be found

### Domain Events

- **User Registered** when the user registers, it will also create a user workspace with an admin membership
- **User Deleted** when the user is deleted, it will also delete workspaces where he was alone, 
or transfer ownership on workspaces and promote the new owner to Admin, if necessary 
# Domain Entities

## Refresh Token History

This entity represents the refresh token history to keep track of access tokens.

```csharp
class RefreshTokenHistory 
{
    RefreshTokenHistory Create()
}
```

```json
{
  "id": "88882448-bd63-4731-8a05-f6333b6d22e2",
  "jwtId": { "value": "A5D45BD1-6FF0-4E53-8A77-8F567855E472" },
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "refreshToken": {}
}
```

For database design, check out the [Refresh Token Diagram](../../../database-diagrams/entities/Diagram.RefreshToken.md).
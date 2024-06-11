# Domain Entities

## Refresh Token

This entity represents the refresh token to get a new access token.

```csharp
class RefreshToken 
{
    RefreshToken Create()
    void Refresh()
    bool HasRefreshed()
}
```

```json
{
  "id": { "value": "88882448-bd63-4731-8a05-f6333b6d22e2" },
  "expiryDateTime": "2020-02-01T00:00:00.0000000Z",
  "isInvalidated": { "value": false },
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "user": {},
  "history": [{}],
  "jwtId": "A5D45BD1-6FF0-4E53-8A77-8F567855E472",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "refreshTimes": 2
}
```

For database design, check out the [Refresh Token Diagram](../../database-diagrams/entities/Diagram.RefreshToken.md).

### Entities

- [Refresh Token History](../entities/refresh-token/Entity.RefreshTokenHistory.md)
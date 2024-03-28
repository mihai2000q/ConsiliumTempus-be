# Domain Entities

## Refresh Token

This entity represents the refresh token to get a new access token.

```csharp
class RefreshToken 
{
    RefreshToken Create()
    void UpdateUsage()
}
```

```json
{
  "id": "88882448-bd63-4731-8a05-f6333b6d22e2",
  "jwtId": "A5D45BD1-6FF0-4E53-8A77-8F567855E472",
  "expiryDateTime": "2020-01-08T00:00:00.0000000Z",
  "isInvalidated": false,
  "usedTimes": 2,
  "createdDateTime": "2020-01-01T00:00:00.0000000Z",
  "updatedDateTime": "2020-01-01T00:00:00.0000000Z",
  "user": {},
  "Value": "88882448-bd63-4731-8a05-f6333b6d22e2"
}
```

For database design, check out the [Refresh Token Diagram](../diagrams/entities/Diagram.RefreshToken.md).
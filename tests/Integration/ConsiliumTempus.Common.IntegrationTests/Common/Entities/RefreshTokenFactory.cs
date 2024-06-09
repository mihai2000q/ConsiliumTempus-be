using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.Common.Entities;

public static class RefreshTokenFactory
{
    public static RefreshToken Create(
        Guid jwtId,
        UserAggregate user,
        DateTime? expiryDateTime = null,
        bool isInvalidated = false,
        long refreshTimes = 0,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        return EntityBuilder<RefreshToken>.Empty()
            .WithProperty(nameof(RefreshToken.Id), Guid.NewGuid())
            .WithProperty(nameof(RefreshToken.JwtId), jwtId)
            .WithProperty(nameof(RefreshToken.ExpiryDateTime), expiryDateTime ?? DateTime.UtcNow.AddDays(7))
            .WithProperty(nameof(RefreshToken.IsInvalidated), isInvalidated)
            .WithProperty(nameof(RefreshToken.RefreshTimes), refreshTimes)
            .WithProperty(nameof(RefreshToken.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(RefreshToken.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(RefreshToken.User), user)
            .Build();
    }
}
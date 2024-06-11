using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Authentication;

public static class RefreshTokenHistoryFactory
{
    public static RefreshTokenHistory Create(
        Guid? jwtId = null,
        DateTime? createdDateTime = null)
    {
        return EntityBuilder<RefreshTokenHistory>.Empty()
            .WithProperty(nameof(RefreshTokenHistory.Id), Guid.NewGuid())
            .WithProperty(nameof(RefreshTokenHistory.JwtId), JwtId.Create(jwtId ?? Guid.NewGuid()))
            .WithProperty(nameof(RefreshTokenHistory.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .Build();
    }
}
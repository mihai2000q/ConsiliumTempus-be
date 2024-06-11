using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.Authentication;

public static class RefreshTokenFactory
{
    public static RefreshToken Create(
        UserAggregate user,
        DateTime? expiryDateTime = null,
        bool isInvalidated = false,
        DateTime? createdDateTime = null,
        List<RefreshTokenHistory>? history = null)
    {
        return EntityBuilder<RefreshToken>.Empty()
            .WithProperty(nameof(RefreshToken.Id), RefreshTokenId.CreateUnique())
            .WithProperty(nameof(RefreshToken.ExpiryDateTime), expiryDateTime ?? DateTime.UtcNow.AddMonths(1))
            .WithProperty(nameof(RefreshToken.IsInvalidated), IsInvalidated.Create(isInvalidated))
            .WithProperty(nameof(RefreshToken.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(RefreshToken.User), user)
            .WithField(nameof(RefreshToken.History).ToBackingField(), history ?? [])
            .Build();
    }
}
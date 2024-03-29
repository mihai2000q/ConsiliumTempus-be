using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory.Domain;

internal static class RefreshTokenFactory
{
    public static RefreshToken Create(
        Guid jwtId,
        UserAggregate user,
        DateTime? expiryDateTime = null,
        bool isInvalidated = false,
        long usedTimes = 0,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        var refreshToken = DomainFactory.GetObjectInstance<RefreshToken>();

        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.Id), Guid.NewGuid());
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.JwtId), jwtId);
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.ExpiryDateTime), expiryDateTime ?? DateTime.UtcNow.AddDays(7));
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.IsInvalidated), isInvalidated);
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.UsedTimes), usedTimes);
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref refreshToken, nameof(refreshToken.User), user);
        
        return refreshToken;
    }
}
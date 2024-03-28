using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Common.Entities;

public static class RefreshTokenFactory
{
    public static RefreshToken Create(
        string? jwtId = null,
        UserAggregate? user = null,
        bool invalidated = false,
        DateTime? expiryDate = null)
    {
        var refreshToken = RefreshToken.Create(
            jwtId ?? Guid.NewGuid().ToString(),
            user ?? UserFactory.Create());
        
        typeof(RefreshToken).GetProperty(nameof(refreshToken.IsInvalidated))
            ?.SetValue(refreshToken, invalidated);

        expiryDate ??= DateTime.UtcNow.AddDays(7);
        typeof(RefreshToken).GetProperty(nameof(refreshToken.ExpiryDateTime))
            ?.SetValue(refreshToken, expiryDate);

        return refreshToken;
    }
}
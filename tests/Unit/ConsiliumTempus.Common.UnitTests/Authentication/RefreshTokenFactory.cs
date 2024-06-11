using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Authentication;

public static class RefreshTokenFactory
{
    public static RefreshToken Create(
        Guid? jwtId = null,
        UserAggregate? user = null,
        bool invalidated = false,
        DateTime? expiryDate = null)
    {
        var refreshToken = RefreshToken.Create(
            user ?? UserFactory.Create(),
            JwtId.Create(jwtId ?? Guid.NewGuid()));

        typeof(RefreshToken)
            .GetProperty(nameof(refreshToken.IsInvalidated))
            ?.SetValue(refreshToken, IsInvalidated.Create(invalidated));

        expiryDate ??= DateTime.UtcNow.AddMonths(1);
        typeof(RefreshToken)
            .GetProperty(nameof(refreshToken.ExpiryDateTime))
            ?.SetValue(refreshToken, expiryDate);

        return refreshToken;
    }
}
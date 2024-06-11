using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class RefreshToken
    {
        internal static void AssertCreation(
            Domain.Authentication.RefreshToken refreshToken,
            Guid jwtId,
            UserAggregate user)
        {
            refreshToken.Id.Value.Should().NotBeEmpty();
            refreshToken.ExpiryDateTime.Should().BeCloseTo(DateTime.UtcNow.AddMonths(1), TimeSpanPrecision);
            refreshToken.IsInvalidated.Value.Should().BeFalse();
            refreshToken.User.Should().Be(user);
            refreshToken.JwtId.Value.Should().Be(jwtId);
            refreshToken.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            refreshToken.RefreshTimes.Should().Be(0);
            refreshToken.DomainEvents.Should().BeEmpty();
            refreshToken.History.Should().HaveCount(1);

            refreshToken.History[0].Id.Should().NotBeEmpty();
            refreshToken.History[0].JwtId.Value.Should().Be(jwtId);
            refreshToken.History[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            refreshToken.History[0].RefreshToken.Should().Be(refreshToken);
        }

        internal static void AssertRefresh(
            Domain.Authentication.RefreshToken refreshToken,
            Guid jwtId,
            int refreshTimes = 1)
        {
            refreshToken.JwtId.Value.Should().Be(jwtId);
            refreshToken.RefreshTimes.Should().Be(refreshTimes);
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            refreshToken.History.Should().HaveCount(2);

            refreshToken.History[^1].Id.Should().NotBeEmpty();
            refreshToken.History[^1].JwtId.Value.Should().Be(jwtId);
            refreshToken.History[^1].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
    }
}
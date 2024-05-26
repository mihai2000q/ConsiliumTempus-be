using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class RefreshToken
    {
        internal static void AssertCreation(
            Domain.Common.Entities.RefreshToken refreshToken,
            string jwtId,
            UserAggregate user)
        {
            refreshToken.Id.Should().NotBeEmpty();
            refreshToken.Value.Should().NotBeNullOrWhiteSpace().And.HaveLength(36);
            refreshToken.JwtId.ToString().Should().Be(jwtId);
            refreshToken.ExpiryDateTime.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpanPrecision);
            refreshToken.IsInvalidated.Should().BeFalse();
            refreshToken.RefreshTimes.Should().Be(0);
            refreshToken.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            refreshToken.User.Should().Be(user);
            refreshToken.DomainEvents.Should().BeEmpty();
        }

        internal static void AssertUpdate(
            Domain.Common.Entities.RefreshToken refreshToken,
            string jwtId,
            long usedTimes = 1)
        {
            refreshToken.Value.Should().NotBeNullOrWhiteSpace().And.HaveLength(36);
            refreshToken.JwtId.ToString().Should().Be(jwtId);
            refreshToken.IsInvalidated.Should().BeFalse();
            refreshToken.RefreshTimes.Should().Be(usedTimes);
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
    }
}
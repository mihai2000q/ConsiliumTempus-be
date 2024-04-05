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
            refreshToken.Value.Should().NotBeNullOrWhiteSpace().And.HaveLength(36);
            refreshToken.JwtId.ToString().Should().Be(jwtId);
            refreshToken.ExpiryDateTime.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromMinutes(1));
            refreshToken.IsInvalidated.Should().BeFalse();
            refreshToken.UsedTimes.Should().Be(0);
            refreshToken.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            refreshToken.User.Should().Be(user);
        }
        
        internal static void AssertUpdate(
            Domain.Common.Entities.RefreshToken refreshToken,
            string jwtId,
            long usedTimes = 1)
        {
            refreshToken.Value.Should().NotBeNullOrWhiteSpace().And.HaveLength(36);
            refreshToken.JwtId.ToString().Should().Be(jwtId);
            refreshToken.IsInvalidated.Should().BeFalse();
            refreshToken.UsedTimes.Should().Be(usedTimes);
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }
    }
}
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
            refreshToken.ExpiryDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromMinutes(1));
            refreshToken.Invalidated.Should().BeFalse();
            refreshToken.UsedTimes.Should().Be(0);
            refreshToken.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            refreshToken.User.Should().Be(user);
        }
    }
}
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

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
            refreshToken.ExpiryDateTime.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), 10.Seconds());
            refreshToken.IsInvalidated.Should().BeFalse();
            refreshToken.RefreshTimes.Should().Be(0);
            refreshToken.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
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
            refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
        }
    }
}
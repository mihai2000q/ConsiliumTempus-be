using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class RefreshToken
    {
        internal static void AssertCreation(
            Domain.Authentication.RefreshToken refreshToken,
            Guid refreshTokenResponse,
            string token,
            UserAggregate user)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(token).Should().BeTrue();
            var accessToken = handler.ReadJwtToken(token);

            refreshToken.Id.Value.Should().Be(refreshTokenResponse);
            refreshToken.JwtId.Value.ToString()
                .Should()
                .Be(accessToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
            refreshToken.User.Should().Be(user);
            refreshToken.RefreshTimes.Should().Be(0);
        }

        internal static void AssertRefresh(
            Domain.Authentication.RefreshToken refreshToken,
            Domain.Authentication.RefreshToken newRefreshToken,
            string token)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(token).Should().BeTrue();
            var accessToken = handler.ReadJwtToken(token);

            // unchanged
            newRefreshToken.Id.Should().Be(refreshToken.Id);
            newRefreshToken.IsInvalidated.Should().Be(refreshToken.IsInvalidated);
            newRefreshToken.CreatedDateTime.Should().Be(refreshToken.CreatedDateTime);
            newRefreshToken.ExpiryDateTime.Should().Be(refreshToken.ExpiryDateTime);

            // changed
            newRefreshToken.JwtId.ToString()
                .Should()
                .Be(accessToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
            newRefreshToken.RefreshTimes.Should().Be(1);
            newRefreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        internal static void AssertAlreadyRefreshed(
            Domain.Authentication.RefreshToken refreshToken,
            string token)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(token).Should().BeTrue();
            var accessToken = handler.ReadJwtToken(token);

            accessToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
                .Should().Be(refreshToken.JwtId.Value.ToString());
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class RefreshToken
    {
        internal static void AssertCreation(
            Domain.Common.Entities.RefreshToken refreshToken, 
            string? refreshTokenValue,
            string? token,
            UserAggregate user)
        {
            var handler = new JwtSecurityTokenHandler();

            handler.CanReadToken(token).Should().BeTrue();

            var accessToken = handler.ReadJwtToken(token);

            refreshToken.Value.Should().Be(refreshTokenValue);
            refreshToken.JwtId.ToString()
                .Should()
                .Be(accessToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
            refreshToken.User.Should().Be(user);
        }
    }
}
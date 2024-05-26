using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Auth
    {
        internal static void AssertToken(string? token, JwtSettings jwtSettings, UserAggregate user)
        {
            var handler = new JwtSecurityTokenHandler();

            handler.CanReadToken(token).Should().BeTrue();

            var outcomeToken = handler.ReadJwtToken(token);
            outcomeToken.Issuer.Should().Be(jwtSettings.Issuer);
            outcomeToken.Audiences.Should().HaveCount(1);
            outcomeToken.Audiences.First().Should().Be(jwtSettings.Audience);
            outcomeToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes), TimeSpanPrecision);

            // The 3 below and the 3 from above
            const int claimsSize = 3 + 3;
            outcomeToken.Claims.Should().HaveCount(claimsSize);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value
                .Should().NotBeNullOrWhiteSpace();
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value
                .Should().Be(user.Credentials.Email);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
                .Should().NotBeNullOrWhiteSpace();
        }
    }
}
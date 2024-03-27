using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static class Utils
{
    internal static void AssertToken(string token, UserAggregate user, JwtSettings jwtSettings)
    {
        var handler = new JwtSecurityTokenHandler();
        
        handler.CanReadToken(token).Should().BeTrue();

        var outcomeToken = handler.ReadJwtToken(token);
        outcomeToken.Issuer.Should().Be(jwtSettings.Issuer);
        outcomeToken.Audiences.Should().HaveCount(1);
        outcomeToken.Audiences.First().Should().Be(jwtSettings.Audience);
        outcomeToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes), 1.Minutes());

        // The 5 below and the 3 from above
        const int claimsSize = 5 + 3;
        outcomeToken.Claims.Should().HaveCount(claimsSize);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value
            .Should().Be(user.Id.Value.ToString());
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value
            .Should().Be(user.Credentials.Email);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.GivenName).Value
            .Should().Be(user.FirstName.Value);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.FamilyName).Value
            .Should().Be(user.LastName.Value);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
            .Should().HaveLength(Guid.NewGuid().ToString().Length);
    }
}
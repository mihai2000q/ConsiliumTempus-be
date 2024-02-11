using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Infrastructure.Authentication;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Auth
    {
        internal static void AssertToken(string? token, JwtSettings jwtSettings, string email)
        {
            var handler = new JwtSecurityTokenHandler();
            
            handler.CanReadToken(token).Should().BeTrue();

            var outcomeToken = handler.ReadJwtToken(token);
            outcomeToken.Issuer.Should().Be(jwtSettings.Issuer);
            outcomeToken.Audiences.Should().HaveCount(1);
            outcomeToken.Audiences.First().Should().Be(jwtSettings.Audience);
            outcomeToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddHours(jwtSettings.ExpiryHours), 1.Minutes());
        
            // The 5 below and the 3 from above
            const int claimsSize = 5 + 3;
            outcomeToken.Claims.Should().HaveCount(claimsSize);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value
                .Should().NotBeNullOrWhiteSpace();
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value
                .Should().Be(email);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.GivenName).Value
                .Should().NotBeNullOrWhiteSpace();
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.FamilyName).Value
                .Should().NotBeNullOrWhiteSpace();
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
                .Should().NotBeNullOrWhiteSpace();
        }
        
        internal static void AssertToken(string? token, JwtSettings jwtSettings, 
            string email, string firstName, string lastName)
        {
            var handler = new JwtSecurityTokenHandler();
            
            handler.CanReadToken(token).Should().BeTrue();

            var outcomeToken = handler.ReadJwtToken(token);
            outcomeToken.Issuer.Should().Be(jwtSettings.Issuer);
            outcomeToken.Audiences.Should().HaveCount(1);
            outcomeToken.Audiences.First().Should().Be(jwtSettings.Audience);
            outcomeToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddHours(jwtSettings.ExpiryHours), 1.Minutes());
        
            // The 5 below and the 3 from above
            const int claimsSize = 5 + 3;
            outcomeToken.Claims.Should().HaveCount(claimsSize);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value
                .Should().NotBeNullOrWhiteSpace();
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value
                .Should().Be(email);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.GivenName).Value
                .Should().Be(firstName);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.FamilyName).Value
                .Should().Be(lastName);
            outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
                .Should().NotBeNullOrWhiteSpace();
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

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
            refreshToken.UsedTimes.Should().Be(0);
        }
        
        internal static void AssertUpdate(
            Domain.Common.Entities.RefreshToken refreshToken, 
            Domain.Common.Entities.RefreshToken newRefreshToken, 
            string? token)
        {
            var handler = new JwtSecurityTokenHandler();

            handler.CanReadToken(token).Should().BeTrue();

            var accessToken = handler.ReadJwtToken(token);

            // unchanged
            newRefreshToken.IsInvalidated.Should().Be(refreshToken.IsInvalidated);
            newRefreshToken.CreatedDateTime.Should().Be(refreshToken.CreatedDateTime);
            newRefreshToken.Value.Should().Be(refreshToken.Value);
            newRefreshToken.ExpiryDateTime.Should().Be(refreshToken.ExpiryDateTime);
            
            // changed
            newRefreshToken.JwtId.ToString()
                .Should()
                .Be(accessToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
            newRefreshToken.UsedTimes.Should().Be(1);
            newRefreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }
    }
}
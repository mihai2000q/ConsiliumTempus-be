using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authentication;

public class JwtBearerAuthenticationTest
{
    #region Setup

    private static readonly JwtSettings JwtSettings = new()
    {
        SecretKey = "This-is-a-super-duper-secret-key-used-for-testing-purposes",
        Issuer = "Issuer",
        Audience = "Audience",
        ExpiryMinutes = 5
    };

    #endregion

    [Theory]
    [InlineData(-5, true)]
    [InlineData(0, true)]
    [InlineData(30, true)]
    [InlineData(2 * 60, false)]
    [InlineData(6 * 60, false)]
    public void AccessToken_WhenItExpiredSomeSecondsAgo_ShouldOnlyBeAcceptedWithinTheClockSkew(
        int expiredSecondsAgo,
        bool isAccepted)
    {
        // Arrange
        var token = Utils.Token.GenerateToken(
            JwtSettings,
            expires: DateTime.UtcNow.AddSeconds(-expiredSecondsAgo));

        // Act
        var outcome = () => new JwtSecurityTokenHandler()
            .ValidateToken(token, Utils.Token.GetTokenValidationParameters(JwtSettings), out _);

        // Assert
        if (isAccepted)
            outcome.Should().NotThrow();
        else
            outcome.Should().Throw<SecurityTokenExpiredException>();
    }
}

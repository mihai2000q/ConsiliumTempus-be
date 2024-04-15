using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authentication;

public class JwtTokenGeneratorTest
{
    #region Setup

    private readonly JwtSettings _jwtSettings;
    private readonly JwtTokenGenerator _uut;

    public JwtTokenGeneratorTest()
    {
        _jwtSettings = new JwtSettings
        {
            SecretKey = "This-Is-My-Secret-Duper-Key-And-I-Like-It",
            Audience = "Audience",
            ExpiryMinutes = 7,
            Issuer = "Issuer"
        };

        var options = Substitute.For<IOptions<JwtSettings>>();
        options
            .Value
            .Returns(_jwtSettings);

        _uut = new JwtTokenGenerator(options);
    }

    #endregion

    [Fact]
    public void WhenGenerateToken_ShouldReturnNewToken()
    {
        // Arrange
        var user = UserFactory.Create();

        // Act
        var outcome = _uut.GenerateToken(user);

        // Assert
        Utils.AssertToken(outcome, user, _jwtSettings);
    }

    [Fact]
    public void WhenGetJwtIdFromToken_ShouldReturnJti()
    {
        // Arrange
        const string jti = "This is the JwtId";
        var token = Utils.Token.GenerateToken(_jwtSettings, jti: jti);

        // Act
        var outcome = _uut.GetJwtIdFromToken(token);

        // Assert
        outcome.Should().Be(jti);
    }
}
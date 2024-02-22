using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.UnitTests.Authentication;

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
            ExpiryHours = 24,
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
        var user = UserFactory.Create(
            "FirstyLasty@Example.com",
            "Password123",
            "FirstName",
            "LastName");
        
        // Act
        var outcome = _uut.GenerateToken(user);

        // Assert
        Utils.AssertToken(outcome, user, _jwtSettings);
    }
}
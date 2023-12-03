using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.UserAggregate;
using ConsiliumTempus.Infrastructure.Authentication;
using FluentAssertions.Extensions;
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
        
        var options = new Mock<IOptions<JwtSettings>>();
        options.SetupGet(o => o.Value)
            .Returns(_jwtSettings);
        
        _uut = new JwtTokenGenerator(options.Object);
    }

    #endregion

    [Fact]
    public void WhenGenerateToken_ReturnNewToken()
    {
        // Arrange
        var user = User.Create(
            "First",
            "Last",
            "FirstyLasty@Example.com",
            "Password123");

        var handler = new JwtSecurityTokenHandler();
        
        // Act
        var outcome = _uut.GenerateToken(user);

        // Assert
        handler.CanReadToken(outcome).Should().BeTrue();

        var outcomeToken = handler.ReadJwtToken(outcome);
        outcomeToken.Issuer.Should().Be(_jwtSettings.Issuer);
        outcomeToken.Audiences.Should().HaveCount(1);
        outcomeToken.Audiences.First().Should().Be(_jwtSettings.Audience);
        outcomeToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours), 1.Minutes());
        
        // The 5 below and the 3 from above
        const int claimsSize = 5 + 3;
        outcomeToken.Claims.Should().HaveCount(claimsSize);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value
            .Should().Be(user.Id.Value.ToString());
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value
            .Should().Be(user.Email);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.GivenName).Value
            .Should().Be(user.FirstName);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.FamilyName).Value
            .Should().Be(user.LastName);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
            .Should().HaveLength(Guid.NewGuid().ToString().Length);
    }
}
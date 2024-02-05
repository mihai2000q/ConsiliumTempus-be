using System.IdentityModel.Tokens.Jwt;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;
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
    public void WhenGenerateToken_ShouldReturnNewToken()
    {
        // Arrange
        var user = Mock.Mock.User.CreateMock(
            Credentials.Create(
                "FirstyLasty@Example.com",
                "Password123"),
            Name.Create(
                "First",
                "Last"));

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
            .Should().Be(user.Credentials.Email);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.GivenName).Value
            .Should().Be(user.Name.First);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.FamilyName).Value
            .Should().Be(user.Name.Last);
        outcomeToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value
            .Should().HaveLength(Guid.NewGuid().ToString().Length);
    }

    [Fact]
    public void GetUserIdFromToken_WhenIsValid_ShouldReturnTheUserId()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var token = Mock.Mock.Token.CreateMock((JwtRegisteredClaimNames.Sub, userId));

        // Act
        var outcome = _uut.GetUserIdFromToken(token);

        // Assert
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(userId);
    }
    
    [Fact]
    public void GetUserIdFromToken_WhenClaimIsNull_ShouldReturnInvalidTokenError()
    {
        // Arrange
        var token = Mock.Mock.Token.CreateMock();

        // Act
        var outcome = _uut.GetUserIdFromToken(token);

        // Assert
        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Should().Be(Errors.Authentication.InvalidToken);
    }
}
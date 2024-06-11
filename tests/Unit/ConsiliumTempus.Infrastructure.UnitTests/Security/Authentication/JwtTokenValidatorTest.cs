using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authentication;

public class JwtTokenValidatorTest
{
    #region Setup

    private readonly JwtSettings _jwtSettings;
    private readonly IUserProvider _userProvider;
    private readonly JwtTokenValidator _uut;

    public JwtTokenValidatorTest()
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
        
        _userProvider = Substitute.For<IUserProvider>();
        _uut = new JwtTokenValidator(options, _userProvider);
    }

    #endregion

    [Fact]
    public void ValidateRefreshToken_WhenValid_ShouldReturnTrue()
    {
        // Arrange
        var refreshToken = RefreshTokenFactory.Create();

        // Act
        var outcome = _uut.ValidateRefreshToken(refreshToken);

        // Assert
        outcome.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(JwtTokenValidatorData.GetInvalidRefreshTokens))]
    public void ValidateRefreshToken_WhenInvalid_ShouldReturnFalse(RefreshToken refreshToken)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = _uut.ValidateRefreshToken(refreshToken);

        // Assert
        outcome.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAccessToken_WhenValid_ShouldReturnTrue()
    {
        // Arrange
        var user = UserFactory.Create();
        _userProvider
            .Get(Arg.Any<UserId>())
            .Returns(user);

        var token = Utils.Token.GenerateToken(_jwtSettings, user);

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId == user.Id));

        outcome.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAccessToken_WhenUserIsNotFound_ShouldReturnFalse()
    {
        // Arrange
        var sub = Guid.NewGuid();
        var token = Utils.Token.GenerateToken(_jwtSettings, sub: sub.ToString());

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId.Value == sub));

        outcome.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(JwtTokenValidatorData.GetInvalidTokensByJwtSettings))]
    public async Task ValidateAccessToken_WhenInvalidByJwtSettings_ShouldReturnFalse(string token)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        _userProvider.DidNotReceive();

        outcome.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAccessToken_WhenUserIdIsWrong_ShouldReturnFalse()
    {
        // Arrange
        var token = Utils.Token.GenerateToken(_jwtSettings, sub: "asd");

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        _userProvider.DidNotReceive();

        outcome.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(JwtTokenValidatorData.GetInvalidTokensByClaims))]
    public async Task ValidateAccessToken_WhenInvalidByClaims_ShouldReturnFalse(string token, UserAggregate user)
    {
        // Arrange - parameterized
        _userProvider
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId == user.Id));

        outcome.Should().BeFalse();
    }
}
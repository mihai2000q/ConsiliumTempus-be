using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            SecretKey = JwtTokenValidatorData.SecretKey,
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

    [Fact]
    public async Task ValidateAccessToken_WhenTokenIsExpired_ShouldReturnTrue()
    {
        // Arrange
        var user = UserFactory.Create();
        _userProvider
            .Get(Arg.Any<UserId>())
            .Returns(user);

        var expiredSettings = new JwtSettings
        {
            SecretKey = _jwtSettings.SecretKey,
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            ExpiryMinutes = -30
        };
        var token = Utils.Token.GenerateToken(expiredSettings, user);

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        outcome.Should().BeTrue(); // the access token is refreshed after it expires
    }

    [Fact]
    public async Task ValidateAccessToken_WhenSignedWithAnotherKey_ShouldReturnFalse()
    {
        // Arrange
        var user = UserFactory.Create();
        var otherSettings = new JwtSettings
        {
            SecretKey = "This-Is-Another-Secret-Key-That-Is-Not-The-Real-One",
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            ExpiryMinutes = _jwtSettings.ExpiryMinutes
        };
        var token = Utils.Token.GenerateToken(otherSettings, user);

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider.DidNotReceive().Get(Arg.Any<UserId>());

        outcome.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAccessToken_WhenPayloadWasTampered_ShouldReturnFalse()
    {
        // Arrange
        var user = UserFactory.Create();
        var otherUser = UserFactory.Create("other@gmail.com");
        var token = Utils.Token.GenerateToken(_jwtSettings, user);
        var otherToken = Utils.Token.GenerateToken(_jwtSettings, otherUser);

        var parts = token.Split('.');
        var tamperedToken = $"{parts[0]}.{otherToken.Split('.')[1]}.{parts[2]}"; // payload of another token

        // Act
        var outcome = await _uut.ValidateAccessToken(tamperedToken);

        // Assert
        await _userProvider.DidNotReceive().Get(Arg.Any<UserId>());

        outcome.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAccessToken_WhenTokenIsNotSigned_ShouldReturnFalse()
    {
        // Arrange
        var user = UserFactory.Create();
        var unsignedToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Credentials.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ],
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)));

        // Act
        var outcome = await _uut.ValidateAccessToken(unsignedToken);

        // Assert
        await _userProvider.DidNotReceive().Get(Arg.Any<UserId>());

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
        await _userProvider.DidNotReceive().Get(Arg.Any<UserId>());

        outcome.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(JwtTokenValidatorData.GetMalformedTokens))]
    public async Task ValidateAccessToken_WhenTokenIsMalformed_ShouldReturnFalse(string token)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider.DidNotReceive().Get(Arg.Any<UserId>());

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
        await _userProvider.DidNotReceive().Get(Arg.Any<UserId>());

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
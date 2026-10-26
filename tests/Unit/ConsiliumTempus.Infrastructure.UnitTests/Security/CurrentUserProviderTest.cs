using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Security;
using Microsoft.AspNetCore.Http;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security;

public class CurrentUserProviderTest
{
    #region Setup

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly CurrentUserProvider _uut;

    public CurrentUserProviderTest()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new CurrentUserProvider(_httpContextAccessor, _userRepository);
    }

    private void MockHttpContextWithSub(string sub)
    {
        var principal = new ClaimsPrincipal();
        principal.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, sub)
        ]));
        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = principal });
    }

    #endregion

    [Fact]
    public async Task GetCurrentUser_WhenIsSuccessful_ShouldReturnCurrentUser()
    {
        // Arrange
        var user = UserFactory.Create();
        MockHttpContextWithSub(user.Id.Value.ToString());
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.GetCurrentUser();

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId == user.Id));

        outcome.Should().Be(user);
    }

    [Fact]
    public async Task GetCurrentUser_WhenUserIsNotFound_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        MockHttpContextWithSub(userId.ToString());
        _userRepository
            .Get(Arg.Any<UserId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.GetCurrentUser();

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId.Value == userId));

        outcome.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("not a guid")]
    public async Task GetCurrentUser_WhenSubClaimIsNotGuid_ShouldReturnNull(string sub)
    {
        // Arrange
        MockHttpContextWithSub(sub);

        // Act
        var outcome = await _uut.GetCurrentUser();

        // Assert
        await _userRepository.DidNotReceive().Get(Arg.Any<UserId>());

        outcome.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrentUser_WhenSubClaimIsNull_ShouldReturnNull()
    {
        // Arrange
        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());

        // Act
        var outcome = await _uut.GetCurrentUser();

        // Assert
        await _userRepository.DidNotReceive().Get(Arg.Any<UserId>());

        outcome.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrentUser_WhenHttpContextIsNull_ShouldReturnNull()
    {
        // Arrange
        _httpContextAccessor
            .HttpContext
            .ReturnsNull();

        // Act
        var outcome = await _uut.GetCurrentUser();

        // Assert
        await _userRepository.DidNotReceive().Get(Arg.Any<UserId>());

        outcome.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrentUserAfterPermissionCheck_WhenIsSuccessful_ShouldReturnCurrentUser()
    {
        // Arrange
        var user = UserFactory.Create();
        MockHttpContextWithSub(user.Id.Value.ToString());
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.GetCurrentUserAfterPermissionCheck();

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId == user.Id));

        outcome.Should().Be(user);
    }

    [Fact]
    public async Task GetCurrentUserAfterPermissionCheck_WhenUserIsNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        MockHttpContextWithSub(userId.ToString());
        _userRepository
            .Get(Arg.Any<UserId>())
            .ReturnsNull();

        // Act
        var outcome = async () => await _uut.GetCurrentUserAfterPermissionCheck();

        // Assert
        await outcome.Should().ThrowAsync<InvalidOperationException>();
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(uId => uId.Value == userId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("not a guid")]
    public async Task GetCurrentUserAfterPermissionCheck_WhenSubClaimIsNotGuid_ShouldThrowInvalidOperationException(
        string sub)
    {
        // Arrange
        MockHttpContextWithSub(sub);

        // Act
        var outcome = async () => await _uut.GetCurrentUserAfterPermissionCheck();

        // Assert
        await outcome.Should().ThrowAsync<InvalidOperationException>();
        await _userRepository.DidNotReceive().Get(Arg.Any<UserId>());
    }

    [Fact]
    public async Task GetCurrentUserAfterPermissionCheck_WhenSubClaimIsNull_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());

        // Act
        var outcome = async () => await _uut.GetCurrentUserAfterPermissionCheck();

        // Assert
        await outcome.Should().ThrowAsync<InvalidOperationException>();
        await _userRepository.DidNotReceive().Get(Arg.Any<UserId>());
    }

    [Fact]
    public async Task GetCurrentUserAfterPermissionCheck_WhenHttpContextIsNull_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _httpContextAccessor
            .HttpContext
            .ReturnsNull();

        // Act
        var outcome = async () => await _uut.GetCurrentUserAfterPermissionCheck();

        // Assert
        await outcome.Should().ThrowAsync<InvalidOperationException>();
        await _userRepository.DidNotReceive().Get(Arg.Any<UserId>());
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Common.Security;

public class SecurityTest
{
    #region Setup

    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly Application.Common.Security.Security _uut;

    public SecurityTest()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new Application.Common.Security.Security(_jwtTokenGenerator, _userRepository);
    }

    #endregion

    [Fact]
    public async Task GetUserFromToken_WhenIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        const string plainToken = "This is the user Token";

        var plainUserId = Guid.NewGuid().ToString();
        _jwtTokenGenerator
            .GetUserIdFromToken(plainToken)
            .Returns(plainUserId);

        var user = UserFactory.Create();
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.GetUserFromToken(plainToken);

        // Assert
        _jwtTokenGenerator
            .Received(1)
            .GetUserIdFromToken(Arg.Any<string>());
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => Utils.User.AssertId(id, plainUserId)));
        
        outcome.Should().Be(user);
    }
}
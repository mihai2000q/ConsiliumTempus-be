using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Queries;

public class LoginQueryHandlerTest
{
    #region Setup

    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IScrambler _scrambler;
    private readonly IUserRepository _userRepository;
    private readonly LoginQueryHandler _uut;

    public LoginQueryHandlerTest()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _scrambler = Substitute.For<IScrambler>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new LoginQueryHandler(_userRepository, _scrambler, _jwtTokenGenerator);
    }

    #endregion

    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldQueryUserAndReturnNewToken()
    {
        // Arrange
        var query = AuthenticationQueryFactory.CreateLoginQuery();

        var user = UserFactory.Create(password: "This is the pass for Password123");
        _userRepository
            .GetUserByEmail(query.Email.ToLower())
            .Returns(user);

        _scrambler
            .VerifyPassword(query.Password, user.Credentials.Password)
            .Returns(true);

        const string token = "This is a token";
        _jwtTokenGenerator
            .GenerateToken(user)
            .Returns(token);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetUserByEmail(Arg.Any<string>());
        _scrambler
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Any<UserAggregate>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
    }

    [Fact]
    public async Task WhenLoginFailsDueToMissingUser_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var query = AuthenticationQueryFactory.CreateLoginQuery();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetUserByEmail(Arg.Any<string>());
        _scrambler.DidNotReceive();
        _jwtTokenGenerator.DidNotReceive();

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        outcome.FirstError.Code.Should().Be("Authentication.InvalidCredentials");
        outcome.FirstError.Description.Should().Be("Invalid Credentials");
    }

    [Fact]
    public async Task WhenLoginFailsDueToWrongPassword_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var query = AuthenticationQueryFactory.CreateLoginQuery();

        var user = UserFactory.Create();
        _userRepository
            .GetUserByEmail(query.Email.ToLower())
            .Returns(user);

        _scrambler
            .VerifyPassword(query.Password, user.Credentials.Password)
            .Returns(false);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetUserByEmail(Arg.Any<string>());
        _scrambler
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
        _jwtTokenGenerator.DidNotReceive();

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        outcome.FirstError.Code.Should().Be("Authentication.InvalidCredentials");
        outcome.FirstError.Description.Should().Be("Invalid Credentials");
    }
}
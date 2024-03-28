using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Register;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands.Register;

public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IScrambler _scrambler;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly RegisterCommandHandler _uut;

    public RegisterCommandHandlerTest()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _scrambler = Substitute.For<IScrambler>();
        _userRepository = Substitute.For<IUserRepository>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _uut = new RegisterCommandHandler(
            _jwtTokenGenerator,
            _scrambler,
            _userRepository,
            _refreshTokenRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(RegisterCommandHandlerData.GetCommands))]
    public async Task WhenRegisterIsSuccessful_ShouldCreateUserAndReturnNewToken(RegisterCommand command)
    {
        // Arrange
        UserAggregate createdUser = null!;
        _userRepository
            .When(u => u.Add(Arg.Any<UserAggregate>()))
            .Do(user => createdUser = user.Arg<UserAggregate>());

        const string hashedPassword = "This is the hash password for Password123";
        _scrambler
            .HashPassword(command.Password)
            .Returns(hashedPassword);
        
        UserAggregate userUsedForJwt = null!;
        const string token = "This is a token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>())
            .Returns(token)
            .AndDoes(user => userUsedForJwt = user.Arg<UserAggregate>());
        
        var jwtId = new Guid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(token)
            .Returns(jwtId);

        RefreshToken refreshToken = null!;
        _refreshTokenRepository
            .When(r => r.Add(Arg.Any<RefreshToken>()))
            .Do(rt => refreshToken = rt.Arg<RefreshToken>());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetUserByEmail(Arg.Any<string>());
        await _userRepository
            .Received(1)
            .Add(Arg.Any<UserAggregate>());
        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Any<UserAggregate>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        await _refreshTokenRepository
            .Received(1)
            .Add(Arg.Any<RefreshToken>());

        createdUser.Should().Be(userUsedForJwt);
        Utils.User.AssertFromRegisterCommand(createdUser, command, hashedPassword);

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);

        Utils.RefreshToken.AssertCreation(refreshToken, jwtId, createdUser);
        outcome.Value.RefreshToken.Should().Be(refreshToken.Value);
    }

    [Fact]
    public async Task WhenRegisterFindsAnotherUserWithSameEmail_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRegisterCommand();

        _userRepository
            .GetUserByEmail(command.Email.ToLower())
            .Returns(UserFactory.Create());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetUserByEmail(Arg.Any<string>());
        _jwtTokenGenerator.DidNotReceive();
        _userRepository.DidNotReceive();

        outcome.ValidateError(Errors.User.DuplicateEmail);
    }
}
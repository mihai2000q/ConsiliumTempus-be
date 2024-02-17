using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands;

public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IScrambler _scrambler;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RegisterCommandHandler _uut;

    public RegisterCommandHandlerTest()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _scrambler = Substitute.For<IScrambler>();
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new RegisterCommandHandler(
            _jwtTokenGenerator,
            _scrambler,
            _userRepository,
            _unitOfWork);
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldCreateUserAndReturnNewToken()
    {
        // Arrange
        var command = new RegisterCommand(
            "FirstName",
            "LastName",
            "Example@Email.com",
            "Password123",
            null,
            null);

        UserAggregate createdUser = null!;
        _userRepository
            .When(u => u.Add(Arg.Any<UserAggregate>()))
            .Do(user => createdUser = user.Arg<UserAggregate>());

        UserAggregate userUsedForJwt = null!;
        const string token = "This is a token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>())
            .Returns(token)
            .AndDoes(user => userUsedForJwt = user.Arg<UserAggregate>());

        const string hashedPassword = "This is the hash password for Password123";
        _scrambler
            .HashPassword(command.Password)
            .Returns(hashedPassword);

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
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();

        createdUser.Should().Be(userUsedForJwt);
        Utils.User.AssertFromRegisterCommand(createdUser, command, hashedPassword);

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
    }

    [Fact]
    public async Task WhenRegisterFindsAnotherUserWithSameEmail_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var command = new RegisterCommand(
            "",
            "",
            "Example@Email.com",
            "",
            null,
            null);

        _userRepository
            .GetUserByEmail(command.Email.ToLower())
            .Returns(Mock.Mock.User.CreateMock());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetUserByEmail(Arg.Any<string>());
        _jwtTokenGenerator.DidNotReceive();
        _userRepository.DidNotReceive();
        _unitOfWork.DidNotReceive();

        outcome.ValidateError(Errors.User.DuplicateEmail);
    }
}
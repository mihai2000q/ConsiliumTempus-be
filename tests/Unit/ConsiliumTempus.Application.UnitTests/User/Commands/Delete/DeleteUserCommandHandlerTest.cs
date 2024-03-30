using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.Delete;

public class DeleteUserCommandHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly DeleteUserCommandHandler _uut;

    public DeleteUserCommandHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new DeleteUserCommandHandler(_userRepository, _currentUserProvider);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteUserCommandIsSuccessful_ShouldDeleteAndReturnDeleteResult()
    {
        // Arrange
        var command = new DeleteUserCommand();

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        _userRepository
            .Received(1)
            .Remove(user);
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteUserResult());
    }
    
    [Fact]
    public async Task WhenDeleteUserCommandFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteUserCommand();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        _userRepository.DidNotReceive();
        
        outcome.ValidateError(Errors.User.NotFound);
    }
}
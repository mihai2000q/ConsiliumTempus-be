using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.DeleteCurrent;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.Events;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.DeleteCurrent;

public class DeleteCurrentUserCommandHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly DeleteCurrentUserCommandHandler _uut;

    public DeleteCurrentUserCommandHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new DeleteCurrentUserCommandHandler(_userRepository, _currentUserProvider);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteCurrentUserCommandIsSuccessful_ShouldDeleteAndReturnDeleteResult()
    {
        // Arrange
        var command = new DeleteCurrentUserCommand();

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
        outcome.Value.Should().Be(new DeleteCurrentUserResult());

        user.DomainEvents.Should().HaveCount(1);
        user.DomainEvents[0].Should().BeOfType<UserDeleted>();
        ((UserDeleted)user.DomainEvents[0]).User.Should().Be(user);
    }
    
    [Fact]
    public async Task WhenDeleteCurrentUserCommandFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteCurrentUserCommand();

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
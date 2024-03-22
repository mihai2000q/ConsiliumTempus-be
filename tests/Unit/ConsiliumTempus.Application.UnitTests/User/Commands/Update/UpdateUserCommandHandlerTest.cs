using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.Update;

public class UpdateUserCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly UpdateUserCommandHandler _uut;

    public UpdateUserCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new UpdateUserCommandHandler(_currentUserProvider);
    }

    #endregion

    [Fact]
    public async Task WhenUpdateUserCommandIsSuccessful_ShouldReturnNewUser()
    {
        // Arrange
        var currentUser = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(currentUser);

        var command = UserCommandFactory.CreateUpdateUserCommand();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.IsError.Should().BeFalse();
        Utils.User.AssertFromUpdateCommand(outcome.Value, command);
    }
}
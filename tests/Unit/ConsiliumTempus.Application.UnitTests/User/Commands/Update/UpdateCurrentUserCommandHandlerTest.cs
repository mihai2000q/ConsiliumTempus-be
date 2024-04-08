using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestData.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.Update;

public class UpdateCurrentUserCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly UpdateCurrentUserCommandHandler _uut;

    public UpdateCurrentUserCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new UpdateCurrentUserCommandHandler(_currentUserProvider);
    }

    #endregion
    
    [Theory]
    [ClassData(typeof(UpdateCurrentUserCommandHandlerData.GetCommands))]
    public async Task WhenUpdateCurrentUserCommandIsSuccessful_ShouldReturnSuccessResponse(UpdateCurrentUserCommand command)
    {
        // Arrange
        var currentUser = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(currentUser);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateCurrentUserResult());
        Utils.User.AssertUpdate(currentUser, command);
    }
    
    [Fact]
    public async Task WhenUpdateCurrentUserCommandFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = UserCommandFactory.CreateUpdateUserCommand();
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.ValidateError(Errors.User.NotFound);
    }
}
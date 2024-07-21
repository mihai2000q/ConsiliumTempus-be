using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Create;

public class CreateWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly CreateWorkspaceCommandHandler _uut;

    public CreateWorkspaceCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new CreateWorkspaceCommandHandler(
            _currentUserProvider,
            _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleCreateWorkspaceCommand_WhenIsSuccessful_ShouldAddWorkspaceAndReturnResponse()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand();

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

        await _workspaceRepository
            .Received(1)
            .Add(Arg.Is<WorkspaceAggregate>(workspace =>
                Utils.Workspace.AssertFromCreateCommand(workspace, command, user)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateWorkspaceResult());
    }

    [Fact]
    public async Task HandleCreateWorkspaceCommand_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand();

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        _workspaceRepository.DidNotReceive();

        outcome.ValidateError(Errors.User.NotFound);
    }
}
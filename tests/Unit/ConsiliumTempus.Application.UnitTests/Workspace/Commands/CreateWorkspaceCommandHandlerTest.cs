using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands;

public class CreateWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ISecurity _security;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly CreateWorkspaceCommandHandler _uut;

    public CreateWorkspaceCommandHandlerTest()
    {
        _security = Substitute.For<ISecurity>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new CreateWorkspaceCommandHandler(
            _security,
            _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task WhenCreateWorkspaceIsSuccessful_ShouldAddUserAndReturnNewWorkspace()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand();

        var user = UserFactory.Create();
        _security
            .GetUserFromToken(command.Token)
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _security
            .Received(1)
            .GetUserFromToken(Arg.Any<string>());
        await _workspaceRepository
            .Received(1)
            .Add(Arg.Is<WorkspaceAggregate>(workspace =>
                Utils.Workspace.AssertFromCreateCommand(workspace, command, user)));

        Utils.Workspace.AssertFromCreateCommand(outcome.Workspace, command, user);
    }
}
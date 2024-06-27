using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateOverview;

public class UpdateOverviewWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UpdateOverviewWorkspaceCommandHandler _uut;

    public UpdateOverviewWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UpdateOverviewWorkspaceCommandHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateOverviewWorkspaceCommand_WhenIsSuccessful_ShouldUpdateOverviewAndReturnSuccess()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateUpdateOverviewWorkspaceCommand(id: workspace.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateOverviewWorkspaceResult());

        Utils.Workspace.AssertFromUpdateOverviewCommand(workspace, command);
    }

    [Fact]
    public async Task HandleUpdateOverviewWorkspaceCommand_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateUpdateOverviewWorkspaceCommand();

        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
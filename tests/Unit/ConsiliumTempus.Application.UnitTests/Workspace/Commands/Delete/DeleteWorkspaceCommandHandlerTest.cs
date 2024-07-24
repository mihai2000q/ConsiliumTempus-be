using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Delete;

public class DeleteWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly DeleteWorkspaceCommandHandler _uut;

    public DeleteWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new DeleteWorkspaceCommandHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleDeleteWorkspaceCommand_WhenIsSuccessful_ShouldDeleteAndReturnDeleteResult()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateDeleteWorkspaceCommand(workspace.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _workspaceRepository
            .Received(1)
            .Remove(Arg.Is<WorkspaceAggregate>(w => w == workspace));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteWorkspaceResult());
    }

    [Fact]
    public async Task HandleDeleteWorkspaceCommand_WhenWorkspaceIsPersonal_ShouldReturnUserWorkspaceError()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create(isPersonal: true);
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateDeleteWorkspaceCommand(workspace.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _workspaceRepository
            .DidNotReceive()
            .Remove(Arg.Any<WorkspaceAggregate>());

        outcome.ValidateError(Errors.Workspace.DeletePersonalWorkspace);
    }

    [Fact]
    public async Task HandleDeleteWorkspaceCommand_WhenWorkspaceIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateDeleteWorkspaceCommand();

        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _workspaceRepository
            .DidNotReceive()
            .Remove(Arg.Any<WorkspaceAggregate>());

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
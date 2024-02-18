using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands;

public class UpdateWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateWorkspaceCommandHandler _uut;

    public UpdateWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new UpdateWorkspaceCommandHandler(_workspaceRepository, _unitOfWork);
    }

    #endregion

    [Fact]
    public async Task WhenUpdateWorkspaceIsSuccessful_ShouldUpdateAndReturnNewWorkspace()
    {
        // Arrange
        var workspace = Mock.Mock.Workspace.CreateMock();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = new UpdateWorkspaceCommand(
            workspace.Id.Value,
            "New Name",
            "New Description");

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertFromUpdateCommand(outcome.Value.Workspace, command);
    }

    [Fact]
    public async Task WhenUpdateWorkspaceIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateWorkspaceCommand(Guid.NewGuid(), "New Name", "New Description");

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _unitOfWork.DidNotReceive();
        
        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands;

public class DeleteWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteWorkspaceCommandHandler _uut;

    public DeleteWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new DeleteWorkspaceCommandHandler(_workspaceRepository, _unitOfWork);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteWorkspaceHandleIsSuccessful_ShouldDeleteAndReturnDeleteResult()
    {
        // Arrange
        var command = new DeleteWorkspaceCommand(Guid.NewGuid());

        var workspace = Mock.Mock.Workspace.CreateMock();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _workspaceRepository
            .Received(1)
            .Remove(Arg.Is<WorkspaceAggregate>(w => w == workspace));
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteWorkspaceResult());
    }

    [Fact]
    public async Task WhenDeleteWorkspaceHandleIsInvalid_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteWorkspaceCommand(Guid.NewGuid());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _workspaceRepository
            .DidNotReceive()
            .Remove(Arg.Any<WorkspaceAggregate>());
        _unitOfWork.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
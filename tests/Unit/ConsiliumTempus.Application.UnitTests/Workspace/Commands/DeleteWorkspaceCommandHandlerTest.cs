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

    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly DeleteWorkspaceCommandHandler _uut;

    public DeleteWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new DeleteWorkspaceCommandHandler(_workspaceRepository.Object, _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteWorkspaceHandleIsSuccessful_ShouldDeleteAndReturnWorkspace()
    {
        // Arrange
        var command = new DeleteWorkspaceCommand(Guid.NewGuid());

        var workspaceToDelete = Mock.Mock.Workspace.CreateMock();
        _workspaceRepository.Setup(w =>
                w.Get(It.IsAny<WorkspaceId>(), default))
            .ReturnsAsync(workspaceToDelete);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.Id)),
                    default),
            Times.Once());
        _workspaceRepository.Verify(w => w.Remove(workspaceToDelete), Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once());

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertDeleteResult(outcome.Value, workspaceToDelete);
    }

    [Fact]
    public async Task WhenDeleteWorkspaceHandleIsInvalid_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteWorkspaceCommand(Guid.NewGuid());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.Id)),
                    default),
            Times.Once());
        _workspaceRepository.Verify(w =>
                w.Remove(It.IsAny<WorkspaceAggregate>()),
            Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
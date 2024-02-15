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

    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly UpdateWorkspaceCommandHandler _uut;

    public UpdateWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new UpdateWorkspaceCommandHandler(_workspaceRepository.Object, _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenUpdateWorkspaceIsSuccessful_ShouldUpdateAndReturnNewWorkspace()
    {
        // Arrange
        var currentWorkspace = Mock.Mock.Workspace.CreateMock();
        _workspaceRepository.Setup(w =>
                w.Get(It.IsAny<WorkspaceId>(), default))
            .ReturnsAsync(currentWorkspace);
        
        var command = new UpdateWorkspaceCommand(
            currentWorkspace.Id.Value, 
            "New Name", 
            "New Description");
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.Id)), 
                    default),
                Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once());

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
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.Id)), 
                    default),
            Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never());

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
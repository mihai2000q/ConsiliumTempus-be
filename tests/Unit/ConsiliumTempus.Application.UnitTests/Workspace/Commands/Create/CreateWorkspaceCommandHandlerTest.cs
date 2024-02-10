using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Create;

public class CreateWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly Mock<ISecurity> _security;
    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly Mock<IWorkspaceRoleRepository> _workspaceRoleRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CreateWorkspaceCommandHandler _uut;

    public CreateWorkspaceCommandHandlerTest()
    {
        _security = new Mock<ISecurity>();
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _workspaceRoleRepository = new Mock<IWorkspaceRoleRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new CreateWorkspaceCommandHandler(
            _security.Object,
            _workspaceRepository.Object,
            _workspaceRoleRepository.Object,
            _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldAddUserAndReturnNewWorkspace()
    {
        // Arrange
        var command = new CreateWorkspaceCommand(
            "Workspace 1",
            "This is a description",
            "This is a token");

        var user = Mock.Mock.User.CreateMock();
        _security.Setup(s => s.GetUserFromToken(command.Token))
            .ReturnsAsync(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _security.Verify(s => s.GetUserFromToken(It.IsAny<string>()), Times.Once());
        _workspaceRepository.Verify(w => w.Add(
            It.Is<WorkspaceAggregate>(workspace =>
                Utils.Workspace.AssertFromCreateCommand(workspace, command, user))), Times.Once());
        _workspaceRoleRepository.Verify(w => w.Attach(WorkspaceRole.Admin), Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once());

        Utils.Workspace.AssertFromCreateCommand(outcome.Workspace, command, user);
    }
}
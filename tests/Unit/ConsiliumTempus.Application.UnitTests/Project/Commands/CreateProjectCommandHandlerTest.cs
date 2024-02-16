using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands;

public class CreateProjectCommandHandlerTest
{
    #region Setup

    private readonly Mock<ISecurity> _security;
    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CreateProjectCommandHandler _uut;

    public CreateProjectCommandHandlerTest()
    {
        _security = new Mock<ISecurity>();
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new CreateProjectCommandHandler(
            _security.Object,
            _workspaceRepository.Object,
            _projectRepository.Object,
            _unitOfWork.Object);
    }
    
    #endregion

    [Fact]
    public async Task WhenCreateProjectIsSuccessful_ShouldCreateAndSaveProjectOnWorkspace()
    {
        // Arrange
        var command = new CreateProjectCommand(
            Guid.NewGuid(),
            "Project Name",
            "This is the project description",
            true,
            "This-is-a-token");

        var workspace = Mock.Mock.Workspace.CreateMock();
        _workspaceRepository.Setup(w =>
                w.Get(It.IsAny<WorkspaceId>(), default))
            .ReturnsAsync(workspace);

        var user = Mock.Mock.User.CreateMock();
        _security.Setup(s => s.GetUserFromToken(It.IsAny<string>(), default))
            .ReturnsAsync(user);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.WorkspaceId)), 
                    default),
            Times.Once);
        _security.Verify(s =>
            s.GetUserFromToken(It.Is<string>(t => t == command.Token), default),
            Times.Once);
        _projectRepository.Verify(p => p.Add(
            It.Is<ProjectAggregate>(project => 
                Utils.Project.AssertFromCreateCommand(project, command, workspace, user)), 
            default),
            Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectResult());

        workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }
    
    [Fact]
    public async Task WhenCreateProjectFails_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var command = new CreateProjectCommand(
            Guid.NewGuid(),
            "Project Name",
            "This is the project description",
            true,
            "This-is-a-token");
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.WorkspaceId)), 
                    default),
            Times.Once);
        _security.Verify(s => s.GetUserFromToken(It.IsAny<string>(), default),
            Times.Never);
        _projectRepository.Verify(p => p.Add(
                It.IsAny<ProjectAggregate>(), 
                default),
            Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);
        
        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
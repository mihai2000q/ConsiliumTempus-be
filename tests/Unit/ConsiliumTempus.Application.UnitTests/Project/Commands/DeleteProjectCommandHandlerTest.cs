using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands;

public class DeleteProjectCommandHandlerTest
{
    #region Setup

    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly DeleteProjectCommandHandler _uut;

    public DeleteProjectCommandHandlerTest()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new DeleteProjectCommandHandler(_projectRepository.Object, _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteProjectIsSuccessful_ShouldDeleteProjectAndReturnSuccess()
    {
        // Arrange
        var command = new DeleteProjectCommand(Guid.NewGuid());

        var user = Mock.Mock.User.CreateMock();
        var workspace = Mock.Mock.Workspace.CreateMock();
        var project = Mock.Mock.Project.CreateMock(workspace, user);
        _projectRepository.Setup(p =>
                p.GetWithWorkspace(It.IsAny<ProjectId>(), default))
            .ReturnsAsync(project);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _projectRepository.Verify(p =>
                p.GetWithWorkspace(
                    It.Is<ProjectId>(id => Utils.Project.AssertId(id, command.Id)),
                    default),
            Times.Once);
        _projectRepository.Verify(p => p.Remove(
                It.Is<ProjectAggregate>(pr => pr == project)),
            Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectResult());

        workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task WhenDeleteProjectFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteProjectCommand(Guid.NewGuid());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _projectRepository.Verify(p =>
                p.GetWithWorkspace(
                    It.Is<ProjectId>(id => Utils.Project.AssertId(id, command.Id)),
                    default),
            Times.Once);
        _projectRepository.Verify(p => p.Remove(It.IsAny<ProjectAggregate>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);

        outcome.ValidateError(Errors.Project.NotFound);
    }
}
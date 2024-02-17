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

    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteProjectCommandHandler _uut;

    public DeleteProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new DeleteProjectCommandHandler(_projectRepository, _unitOfWork);
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
        _projectRepository
            .GetWithWorkspace(Arg.Any<ProjectId>())
            .Returns(project);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => Utils.Project.AssertId(id, command.Id)));
        _projectRepository
            .Received(1)
            .Remove(Arg.Is<ProjectAggregate>(pr => pr == project));
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();

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
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => Utils.Project.AssertId(id, command.Id)));
        _projectRepository
            .DidNotReceive()
            .Remove(Arg.Any<ProjectAggregate>());
        _unitOfWork.DidNotReceive();

        outcome.ValidateError(Errors.Project.NotFound);
    }
}
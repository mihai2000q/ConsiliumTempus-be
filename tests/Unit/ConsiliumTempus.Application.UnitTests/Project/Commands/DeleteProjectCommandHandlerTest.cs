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
    private readonly DeleteProjectCommandHandler _uut;

    public DeleteProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new DeleteProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteProjectIsSuccessful_ShouldDeleteProjectAndReturnSuccess()
    {
        // Arrange
        var command = new DeleteProjectCommand(Guid.NewGuid());

        var project = Mock.Mock.Project.CreateMock();
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

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectResult());

        project.Workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
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

        outcome.ValidateError(Errors.Project.NotFound);
    }
}
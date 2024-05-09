using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Delete;

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
        var project = ProjectFactory.Create();
        _projectRepository
            .GetWithWorkspace(Arg.Any<ProjectId>())
            .Returns(project);
        
        var command = new DeleteProjectCommand(project.Id.Value);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => id.Value == command.Id));
        _projectRepository
            .Received(1)
            .Remove(Arg.Is<ProjectAggregate>(p => Utils.Project.AssertFromDeleteCommand(p, command)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectResult());
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
            .GetWithWorkspace(Arg.Is<ProjectId>(id => id.Value == command.Id));
        _projectRepository
            .DidNotReceive()
            .Remove(Arg.Any<ProjectAggregate>());

        outcome.ValidateError(Errors.Project.NotFound);
    }
}
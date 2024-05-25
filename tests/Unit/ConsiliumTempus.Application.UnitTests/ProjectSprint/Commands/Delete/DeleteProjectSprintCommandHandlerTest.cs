using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.Delete;

public class DeleteProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly DeleteProjectSprintCommandHandler _uut;

    public DeleteProjectSprintCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new DeleteProjectSprintCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleDeleteProjectSprintCommand_WhenIsSuccessful_ShouldRemoveAndReturnSuccessfulResult()
    {
        // Arrange
        var projectSprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithSprintsAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(projectSprint);

        var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand(projectSprint.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithSprintsAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .Received(1)
            .Remove(Arg.Is<ProjectSprintAggregate>(ps =>
                Utils.ProjectSprint.AssertFromDeleteCommand(ps, command)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectSprintResult());
    }
    
    [Fact]
    public async Task HandleDeleteProjectSprintCommand_WhenThereIsOnlyOneSprint_ShouldReturnOnlyOneSprintError()
    {
        // Arrange
        var projectSprint = ProjectSprintFactory.Create(sprintsCount: 1);
        _projectSprintRepository
            .GetWithSprintsAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(projectSprint);

        var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand(projectSprint.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithSprintsAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .DidNotReceive()
            .Remove(Arg.Any<ProjectSprintAggregate>());

        outcome.IsError.Should().BeTrue();
        outcome.ValidateError(Errors.ProjectSprint.OnlyOneSprint);
    }

    [Fact]
    public async Task HandleDeleteProjectSprintCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand();

        _projectSprintRepository
            .GetWithSprintsAndWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithSprintsAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .DidNotReceive()
            .Remove(Arg.Any<ProjectSprintAggregate>());

        outcome.IsError.Should().BeTrue();
        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Delete;

public class DeleteProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectStageRepository _projectStageRepository;
    private readonly DeleteProjectTaskCommandHandler _uut;

    public DeleteProjectTaskCommandHandlerTest()
    {
        _projectStageRepository = Substitute.For<IProjectStageRepository>();
        _uut = new DeleteProjectTaskCommandHandler(_projectStageRepository);
    }

    #endregion

    [Fact]
    public async Task DeleteProjectTaskCommand_WhenIsSuccessful_ShouldDeleteProjectTask()
    {
        // Arrange
        var stage = ProjectStageFactory.CreateWithTasks();
        _projectStageRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .Returns(stage);

        var task = stage.Tasks[0];
        
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(task.Id.Value, stage.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectStageRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.StageId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectTaskResult());

        Utils.ProjectTask.AssertFromDeleteCommand(task, stage, command);
    }

    [Fact]
    public async Task DeleteProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand();

        _projectStageRepository
            .GetWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.StageId))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectStageRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.StageId));

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}
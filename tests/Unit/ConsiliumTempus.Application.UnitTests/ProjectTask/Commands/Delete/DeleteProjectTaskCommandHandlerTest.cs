using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Delete;

public class DeleteProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly DeleteProjectTaskCommandHandler _uut;

    public DeleteProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new DeleteProjectTaskCommandHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleDeleteProjectTaskCommand_WhenIsSuccessful_ShouldDeleteProjectTask()
    {
        // Arrange
        var task = ProjectTaskFactory.CreateWithTasks();
        _projectTaskRepository
            .GetWithTasks(Arg.Any<ProjectTaskId>())
            .Returns(task);
        
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(task.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectTaskResult());

        Utils.ProjectTask.AssertFromDeleteCommand(task, command);
    }

    [Fact]
    public async Task HandleDeleteProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand();

        _projectTaskRepository
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateIsCompleted;

public class UpdateIsCompletedProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly UpdateIsCompletedProjectTaskCommandHandler _uut;

    public UpdateIsCompletedProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new UpdateIsCompletedProjectTaskCommandHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateIsCompletedProjectTaskCommand_WhenIsSuccessful_ShouldUpdateIsCompletedProjectTask()
    {
        // Arrange
        var task = ProjectTaskFactory.Create();
        _projectTaskRepository
            .GetWithWorkspace(Arg.Any<ProjectTaskId>())
            .Returns(task);

        var command = ProjectTaskCommandFactory.CreateUpdateIsCompletedProjectTaskCommand();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateIsCompletedProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateIsCompletedCommand(task, command);
    }

    [Fact]
    public async Task HandleUpdateIsCompletedProjectTaskCommand_WhenProjectTaskIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateIsCompletedProjectTaskCommand();

        _projectTaskRepository
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
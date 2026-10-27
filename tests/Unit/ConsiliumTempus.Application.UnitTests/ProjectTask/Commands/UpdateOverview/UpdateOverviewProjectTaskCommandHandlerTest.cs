using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateOverview;

public class UpdateOverviewProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UpdateOverviewProjectTaskCommandHandler _uut;

    public UpdateOverviewProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UpdateOverviewProjectTaskCommandHandler(_projectTaskRepository, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(UpdateOverviewProjectTaskCommandHandlerData.GetCommands))]
    public async Task HandleUpdateOverviewProjectTaskCommand_WhenIsSuccessful_ShouldUpdateOverviewProjectTask(
        UpdateOverviewProjectTaskCommand command)
    {
        // Arrange
        var task = ProjectTaskFactory.Create();
        _projectTaskRepository
            .GetWithWorkspace(Arg.Any<ProjectTaskId>())
            .Returns(task);

        var assignee = UserFactory.Create();
        _workspaceRepository
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns(assignee);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        if (command.AssigneeId is not null)
            await _workspaceRepository
                .Received(1)
                .GetCollaborator(
                    Arg.Is<UserId>(uId => uId.Value == command.AssigneeId),
                    Arg.Is<WorkspaceId>(wId => wId == task.Stage.Sprint.Project.Workspace.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateOverviewProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateOverviewCommand(task, command, assignee);
    }

    [Fact]
    public async Task HandleUpdateOverviewProjectTaskCommand_WhenAssigneeIsNotCollaborator_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand(assigneeId: Guid.NewGuid());

        var task = ProjectTaskFactory.Create();
        _projectTaskRepository
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        _workspaceRepository
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _workspaceRepository
            .Received(1)
            .GetCollaborator(
                Arg.Is<UserId>(uId => uId.Value == command.AssigneeId),
                Arg.Is<WorkspaceId>(wId => wId == task.Stage.Sprint.Project.Workspace.Id));

        outcome.ValidateError(Errors.User.NotFound);
        task.Assignee.Should().BeNull();
    }

    [Fact]
    public async Task HandleUpdateOverviewProjectTaskCommand_WhenProjectTaskIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand();

        _projectTaskRepository
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _workspaceRepository
            .DidNotReceive()
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>());

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
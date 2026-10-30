using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Update;

public class UpdateProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UpdateProjectTaskCommandHandler _uut;

    public UpdateProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UpdateProjectTaskCommandHandler(_projectTaskRepository, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(UpdateProjectTaskCommandHandlerData.GetCommands))]
    public async Task HandleUpdateProjectTaskCommand_WhenIsSuccessful_ShouldUpdateProjectTask(
        UpdateProjectTaskCommand command)
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
        outcome.Value.Should().Be(new UpdateProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateCommand(task, command, assignee);
    }

    [Fact]
    public async Task HandleUpdateProjectTaskCommand_WhenAssigneeIsNotCollaborator_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand(assigneeId: Guid.NewGuid());

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
    public async Task HandleUpdateProjectTaskCommand_WhenProjectTaskIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand();

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
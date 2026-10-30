using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateCustomField;

public class UpdateCustomFieldFromProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UpdateCustomFieldFromProjectTaskCommandHandler _uut;

    public UpdateCustomFieldFromProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UpdateCustomFieldFromProjectTaskCommandHandler(_projectTaskRepository, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandHandlerData.GetCommands))]
    public async Task HandleUpdateCustomFieldFromProjectTaskCommand_WhenIsSuccessful_ShouldUpdateCustomFieldProjectTask(
        UpdateCustomFieldFromProjectTaskCommand command,
        ProjectTaskAggregate task)
    {
        // Arrange
        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Any<ProjectTaskId>())
            .Returns(task);

        var user = UserFactory.Create();
        _workspaceRepository
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        if (command.PeopleCustomField?.PersonId is not null)
            await _workspaceRepository
                .Received(1)
                .GetCollaborator(
                    Arg.Is<UserId>(id => id.Value == command.PeopleCustomField.PersonId),
                    Arg.Is<WorkspaceId>(wId => wId == task.Stage.Sprint.Project.Workspace.Id));

        _projectTaskRepository
            .Received(1)
            .MarkAsUpdated(task);

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateCustomFieldFromProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateCustomFieldCommand(task, command, user);
    }

    [Fact]
    public async Task
        HandleUpdateCustomFieldFromProjectTaskCommand_WhenMultiSelectOptionIsNull_ShouldReturnMultiSelectOptionNotFoundError()
    {
        // Arrange
        var task = ProjectTaskFactory.CreateWithCustomFields();
        var customField = task.CustomFields.OfType<MultiSelectCustomField>().First();

        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
            id: task.Id.Value,
            customFieldId: customField.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                Guid.NewGuid(),
                true));

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _workspaceRepository
            .DidNotReceive()
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>());

        outcome.ValidateError(Errors.MultiSelectOption.NotFound);
    }

    [Fact]
    public async Task
        HandleUpdateCustomFieldFromProjectTaskCommand_WhenSingleSelectOptionIsNull_ShouldReturnSingleSelectOptionNotFoundError()
    {
        // Arrange
        var task = ProjectTaskFactory.CreateWithCustomFields();
        var customField = task.CustomFields.OfType<SingleSelectCustomField>().First();
        var option = customField.Option;

        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
            id: task.Id.Value,
            customFieldId: customField.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(
                Guid.NewGuid()));

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.SingleSelectOption.NotFound);
        customField.Option.Should().Be(option);
    }

    [Fact]
    public async Task
        HandleUpdateCustomFieldFromProjectTaskCommand_WhenPersonIsNotCollaborator_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var task = ProjectTaskFactory.CreateWithCustomFields();
        var customField = task.CustomFields.OfType<PeopleCustomField>().First();

        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
            id: task.Id.Value,
            customFieldId: customField.Id.Value,
            type: CustomFieldType.People,
            peopleCustomField: new UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand(Guid.NewGuid()));

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        _workspaceRepository
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _workspaceRepository
            .Received(1)
            .GetCollaborator(
                Arg.Is<UserId>(id => id.Value == command.PeopleCustomField!.PersonId),
                Arg.Is<WorkspaceId>(wId => wId == task.Stage.Sprint.Project.Workspace.Id));

        outcome.ValidateError(Errors.User.NotFound);
    }

    [Fact]
    public async Task
        HandleUpdateCustomFieldFromProjectTaskCommand_WhenTypeDoesNotMatchTheCustomField_ShouldReturnCustomFieldTypeMismatchError()
    {
        // Arrange
        var task = ProjectTaskFactory.Create();
        var customField = CustomFieldFactory.CreateText("Text", task: task);
        task.AddCustomField(customField);
        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
            task.Id.Value,
            customField.Id.Value,
            CustomFieldType.Number,
            numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(10));

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _projectTaskRepository
            .DidNotReceive()
            .MarkAsUpdated(Arg.Any<ProjectTaskAggregate>());

        outcome.ValidateError(Errors.CustomField.TypeMismatch);
        customField.Text!.Value.Should().Be("Text");
    }

    [Fact]
    public async Task
        HandleUpdateCustomFieldFromProjectTaskCommand_WhenCustomFieldIsNull_ShouldReturnCustomFieldNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand();

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(ProjectTaskFactory.Create());

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _workspaceRepository
            .DidNotReceive()
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>());

        outcome.ValidateError(Errors.CustomField.NotFound);
    }

    [Fact]
    public async Task HandleUpdateCustomFieldFromProjectTaskCommand_WhenProjectTaskIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand();

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _workspaceRepository
            .DidNotReceive()
            .GetCollaborator(Arg.Any<UserId>(), Arg.Any<WorkspaceId>());

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
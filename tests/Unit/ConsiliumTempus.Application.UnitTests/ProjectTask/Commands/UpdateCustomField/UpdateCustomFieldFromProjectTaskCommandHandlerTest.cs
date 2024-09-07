using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateCustomField;

public class UpdateCustomFieldProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUserRepository _userRepository;
    private readonly UpdateCustomFieldFromProjectTaskCommandHandler _uut;

    public UpdateCustomFieldProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new UpdateCustomFieldFromProjectTaskCommandHandler(_projectTaskRepository, _userRepository);
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
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        if (command.PeopleCustomField?.PersonId is not null)
            await _userRepository
                .Received(1)
                .Get(Arg.Is<UserId>(id => id.Value == command.PeopleCustomField.PersonId));

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
            multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                Guid.NewGuid(),
                true));

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _userRepository.DidNotReceive();

        outcome.ValidateError(Errors.MultiSelectOption.NotFound);
    }

    [Fact]
    public async Task
        HandleUpdateCustomFieldFromProjectTaskCommand_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var task = ProjectTaskFactory.CreateWithCustomFields();
        var customField = task.CustomFields.OfType<PeopleCustomField>().First();

        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
            id: task.Id.Value,
            customFieldId: customField.Id.Value,
            peopleCustomField: new UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand(Guid.NewGuid()));

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .Returns(task);

        _userRepository
            .Get(Arg.Any<UserId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == command.PeopleCustomField!.PersonId));

        outcome.ValidateError(Errors.User.NotFound);
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
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _userRepository.DidNotReceive();

        outcome.ValidateError(Errors.CustomField.NotFound);
    }

    [Fact]
    public async Task HandleUpdateCustomFieldFromProjectTaskCommand_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand();

        _projectTaskRepository
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _userRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
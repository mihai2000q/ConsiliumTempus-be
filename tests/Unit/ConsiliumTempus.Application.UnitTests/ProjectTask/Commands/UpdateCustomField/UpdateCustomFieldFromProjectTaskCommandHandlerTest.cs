using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateCustomField;

public class UpdateCustomFieldProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly UpdateCustomFieldFromProjectTaskCommandHandler _uut;

    public UpdateCustomFieldProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new UpdateCustomFieldFromProjectTaskCommandHandler(_projectTaskRepository);
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

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithCustomFieldsAndWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateCustomFieldFromProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateCustomFieldCommand(task, command);
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

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
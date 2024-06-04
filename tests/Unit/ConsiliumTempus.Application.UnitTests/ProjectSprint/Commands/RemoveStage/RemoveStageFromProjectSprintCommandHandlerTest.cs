using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.RemoveStage;

public class RemoveStageFromProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IAuditRepository _auditRepository;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly RemoveStageFromProjectSprintCommandHandler _uut;

    public RemoveStageFromProjectSprintCommandHandlerTest()
    {
        _auditRepository = Substitute.For<IAuditRepository>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new RemoveStageFromProjectSprintCommandHandler(_auditRepository, _projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleRemoveStageFromProjectSprintCommand_WhenSuccessful_ShouldRemoveAndReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var expectedStageToRemove = sprint.Stages[0];

        var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand(
            stageId: expectedStageToRemove.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _auditRepository
            .Received(1)
            .Remove(Arg.Is<Audit>(a => a == sprint.Audit));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new RemoveStageFromProjectSprintResult());

        Utils.ProjectSprint.AssertRemoveStageCommand(expectedStageToRemove, command);
    }
    
    [Fact]
    public async Task HandleRemoveStageFromProjectSprintCommand_WhenThereIsOnlyOneStage_ShouldReturnOnlyOneStageError()
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create(stagesCount: 1);
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var expectedStageToRemove = sprint.Stages[0];

        var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand(
            stageId: expectedStageToRemove.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _auditRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectStage.OnlyOneStage);
    }

    [Fact]
    public async Task HandleRemoveStageFromProjectSprintCommand_WhenSprintIsNotFound_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _auditRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }

    [Fact]
    public async Task HandleRemoveStageFromProjectSprintCommand_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand();

        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _auditRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}
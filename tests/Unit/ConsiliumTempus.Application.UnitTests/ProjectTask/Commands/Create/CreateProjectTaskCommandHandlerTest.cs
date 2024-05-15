using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using FluentAssertions.Extensions;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Create;

public class CreateProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectStageRepository _projectStageRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly CreateProjectTaskCommandHandler _uut;

    public CreateProjectTaskCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectStageRepository = Substitute.For<IProjectStageRepository>();
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new CreateProjectTaskCommandHandler(
            _currentUserProvider,
            _projectStageRepository,
            _projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleCreateProjectTaskCommand_WhenIsSuccessful_ShouldCreateAndSaveProjectTaskOnProjectStage()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand();

        var stage = ProjectStageFactory.Create();
        _projectStageRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .Returns(stage);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectStageRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.ProjectStageId));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();
        await _projectTaskRepository
            .Received(1)
            .Add(Arg.Is<ProjectTaskAggregate>(task =>
                Utils.ProjectTask.AssertFromCreateCommand(task, command, stage, user)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectTaskResult());

        stage.Sprint.Project.Workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }

    [Fact]
    public async Task HandleCreateProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand();

        _projectStageRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectStageRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.ProjectStageId));
        _currentUserProvider.DidNotReceive();
        _projectTaskRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}
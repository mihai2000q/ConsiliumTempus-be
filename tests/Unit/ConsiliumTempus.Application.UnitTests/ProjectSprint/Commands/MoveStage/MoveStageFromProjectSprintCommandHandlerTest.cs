using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.MoveStage;

public class MoveStageFromProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly MoveStageFromProjectSprintCommandHandler _uut;

    public MoveStageFromProjectSprintCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new MoveStageFromProjectSprintCommandHandler(_currentUserProvider, _projectSprintRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(MoveStageFromProjectSprintCommandHandlerData.GetCommands))]
    public async Task HandleMoveStageFromProjectSprintCommand_WhenSuccessful_ShouldMoveAndReturnSuccessResponse(
        MoveStageFromProjectSprintCommand command,
        ProjectSprintAggregate sprint,
        ProjectStage stage,
        ProjectStage overStage)
    {
        // Arrange
        var expectedCustomOrderPosition = overStage.CustomOrderPosition.Value;

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveStageFromProjectSprintResult());

        Utils.ProjectSprint.AssertFromMoveStageCommand(
            stage,
            command,
            user,
            expectedCustomOrderPosition);
    }

    [Fact]
    public async Task HandleMoveStageFromProjectSprintCommand_WhenSprintIsNotFound_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }

    [Fact]
    public async Task HandleMoveStageFromProjectSprintCommand_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand();

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
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}
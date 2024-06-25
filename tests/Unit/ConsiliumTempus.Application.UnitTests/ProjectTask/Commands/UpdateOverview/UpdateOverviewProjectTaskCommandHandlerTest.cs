using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateOverview;

public class UpdateOverviewProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUserRepository _userRepository;
    private readonly UpdateOverviewProjectTaskCommandHandler _uut;

    public UpdateOverviewProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new UpdateOverviewProjectTaskCommandHandler(_projectTaskRepository, _userRepository);
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
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(assignee);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        if (command.AssigneeId is not null)
        {
            await _userRepository
                .Received(1)
                .Get(Arg.Is<UserId>(uId => uId.Value == command.AssigneeId));
        }

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateOverviewProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateOverviewCommand(task, command, assignee);
    }

    [Fact]
    public async Task HandleUpdateOverviewProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand();

        _projectTaskRepository
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _userRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
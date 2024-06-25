using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Update;

public class UpdateProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUserRepository _userRepository;
    private readonly UpdateProjectTaskCommandHandler _uut;

    public UpdateProjectTaskCommandHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new UpdateProjectTaskCommandHandler(_projectTaskRepository, _userRepository);
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
            .GetWithTasks(Arg.Any<ProjectTaskId>())
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
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        if (command.AssigneeId is not null)
        {
            await _userRepository
                .Received(1)
                .Get(Arg.Is<UserId>(uId => uId.Value == command.AssigneeId));
        }

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateProjectTaskResult());

        Utils.ProjectTask.AssertFromUpdateCommand(task, command, assignee);
    }

    [Fact]
    public async Task HandleUpdateProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand();

        _projectTaskRepository
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _userRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}
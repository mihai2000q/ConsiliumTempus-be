using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.AddStage;

public class AddStageToProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly AddStageToProjectSprintCommandHandler _uut;
    
    public AddStageToProjectSprintCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new AddStageToProjectSprintCommandHandler(_currentUserProvider, _projectSprintRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(AddStageToProjectSprintCommandHandlerData.GetCommands))]
    public async Task HandleAddStageToProjectSprintCommand_WhenSuccessful_ShouldAddAndReturnSuccessResponse(
        AddStageToProjectSprintCommand command)
    {
        // Arrange
        var sprintCreatedBy = UserFactory.Create();
        var sprint = ProjectSprintFactory.Create(createdBy: sprintCreatedBy);
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var createdBy = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(createdBy);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new AddStageToProjectSprintResult());

        Utils.ProjectSprint.AssertFromAddStageCommand(sprint, command, createdBy, sprintCreatedBy);
    }
    
    [Fact]
    public async Task HandleAddStageToProjectSprintCommand_WhenSprintIsNotFound_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand();

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
}
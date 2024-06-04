using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.Create;

public class CreateProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly CreateProjectSprintCommandHandler _uut;

    public CreateProjectSprintCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new CreateProjectSprintCommandHandler(
            _currentUserProvider,
            _projectRepository,
            _projectSprintRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(CreateProjectSprintCommandHandlerData.GetCommands))]
    public async Task HandleCreateProjectSprintCommand_WhenIsSuccessful_ShouldAddAndReturnSuccessResult(
        CreateProjectSprintCommand command,
        ProjectAggregate project)
    {
        // Arrange
        var previousSprintEndDate = project.Sprints.IfNotEmpty(sprints => sprints[0].EndDate);

        _projectRepository
            .GetWithStagesAndWorkspace(Arg.Any<ProjectId>())
            .Returns(project);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithStagesAndWorkspace(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();
        await _projectSprintRepository
            .Received(1)
            .Add(Arg.Is<ProjectSprintAggregate>(ps =>
                Utils.ProjectSprint.AssertFromCreateCommand(ps, command, project, user, previousSprintEndDate)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectSprintResult());
    }

    [Fact]
    public async Task HandleCreateProjectSprintCommand_WhenItFails_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand();

        _projectRepository
            .GetWithStagesAndWorkspace(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithStagesAndWorkspace(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        _currentUserProvider.DidNotReceive();
        _projectSprintRepository.DidNotReceive();

        outcome.ValidateError(Errors.Project.NotFound);
    }
}
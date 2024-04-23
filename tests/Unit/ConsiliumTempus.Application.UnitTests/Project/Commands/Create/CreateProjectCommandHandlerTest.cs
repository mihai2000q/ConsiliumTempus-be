using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Create;

public class CreateProjectCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly CreateProjectCommandHandler _uut;

    public CreateProjectCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new CreateProjectCommandHandler(
            _currentUserProvider,
            _workspaceRepository,
            _projectRepository);
    }

    #endregion

    [Fact]
    public async Task CreateProjectCommand_WhenIsSuccessful_ShouldCreateAndSaveProjectOnWorkspace()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();
        await _projectRepository
            .Received(1)
            .Add(Arg.Is<ProjectAggregate>(project =>
                Utils.Project.AssertFromCreateCommand(project, command, workspace, user)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectResult());

        workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task CreateProjectCommand_WhenWorkspaceIsNull_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        _currentUserProvider.DidNotReceive();
        _projectRepository.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands;

public class CreateProjectCommandHandlerTest
{
    #region Setup

    private readonly ISecurity _security;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly CreateProjectCommandHandler _uut;

    public CreateProjectCommandHandlerTest()
    {
        _security = Substitute.For<ISecurity>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new CreateProjectCommandHandler(
            _security,
            _workspaceRepository,
            _projectRepository);
    }

    #endregion

    [Fact]
    public async Task WhenCreateProjectIsSuccessful_ShouldCreateAndSaveProjectOnWorkspace()
    {
        // Arrange
        var command = new CreateProjectCommand(
            Guid.NewGuid(),
            "Project Name",
            "This is the project description",
            true,
            "This-is-a-token");

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);
        
        var user = UserFactory.Create();
        _security
            .GetUserFromToken(Arg.Any<string>())
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.WorkspaceId)));
        await _security
            .Received(1)
            .GetUserFromToken(Arg.Is<string>(token => token == command.Token));
        await _projectRepository
            .Received(1)
            .Add(Arg.Is<ProjectAggregate>(project =>
                Utils.Project.AssertFromCreateCommand(project, command, workspace, user)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectResult());

        workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task WhenCreateProjectFails_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var command = new CreateProjectCommand(
            Guid.NewGuid(),
            "Project Name",
            "This is the project description",
            true,
            "This-is-a-token");

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => Utils.Workspace.AssertId(id, command.WorkspaceId)));
        _security.DidNotReceive();
        _projectRepository.DidNotReceive();
        
        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
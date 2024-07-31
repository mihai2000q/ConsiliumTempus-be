using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateOwner;

public class UpdateOwnerProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly UpdateOwnerProjectCommandHandler _uut;

    public UpdateOwnerProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdateOwnerProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateOwnerProjectCommand_WhenIsSuccessful_ShouldUpdateOwnerProjectAndReturnSuccess()
    {
        // Arrange
        var workspace = WorkspaceFactory.CreateWithCollaborators();
        var collaborator = workspace.Memberships[1].User;
        var project = ProjectFactory.Create(workspace: workspace);
        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateUpdateOwnerProjectCommand(
            project.Id.Value,
            collaborator.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateOwnerProjectResult());

        Utils.Project.AssertFromUpdateOwnerCommand(project, command);
    }

    [Fact]
    public async Task HandleUpdateOwnerProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdateOwnerProjectCommand();

        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}
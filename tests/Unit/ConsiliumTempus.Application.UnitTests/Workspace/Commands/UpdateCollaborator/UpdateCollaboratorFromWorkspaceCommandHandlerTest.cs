using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateCollaborator;

public class UpdateCollaboratorFromWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UpdateCollaboratorFromWorkspaceCommandHandler _uut;

    public UpdateCollaboratorFromWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UpdateCollaboratorFromWorkspaceCommandHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleUpdateCollaboratorFromWorkspaceCommand_WhenIsSuccessful_ShouldUpdateCollaboratorAndReturnSuccess()
    {
        // Arrange
        var workspace = WorkspaceFactory.CreateWithCollaborators();
        var collaborator = workspace.Memberships[1].User;
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand(
            workspace.Id.Value,
            collaborator.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateCollaboratorFromWorkspaceResult());

        Utils.Workspace.AssertFromUpdateCollaboratorCommand(workspace, command);
    }

    [Fact]
    public async Task
        HandleUpdateCollaboratorFromWorkspaceCommand_WhenCollaboratorIsOwner_ShouldReturnChangeOwnerRoleError()
    {
        // Arrange
        var workspace = WorkspaceFactory.CreateWithCollaborators();
        var ownerMembership = workspace.Memberships.Single(m => m.User == workspace.Owner);
        var role = ownerMembership.WorkspaceRole;
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand(
            workspace.Id.Value,
            workspace.Owner.Id.Value,
            nameof(WorkspaceRole.View));

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.ChangeOwnerRole);
        ownerMembership.WorkspaceRole.Should().Be(role);
    }

    [Fact]
    public async Task
        HandleUpdateCollaboratorFromWorkspaceCommand_WhenCollaboratorIsNull_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand();

        var workspace = WorkspaceFactory.CreateWithCollaborators();
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);
    }

    [Fact]
    public async Task HandleUpdateCollaboratorFromWorkspaceCommand_WhenWorkspaceIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand();

        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
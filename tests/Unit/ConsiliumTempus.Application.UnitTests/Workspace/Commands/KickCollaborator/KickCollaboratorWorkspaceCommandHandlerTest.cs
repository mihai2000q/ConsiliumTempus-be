using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.KickCollaborator;

public class KickCollaboratorFromWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly KickCollaboratorFromWorkspaceCommandHandler _uut;

    public KickCollaboratorFromWorkspaceCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new KickCollaboratorFromWorkspaceCommandHandler(_currentUserProvider, _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleKickCollaboratorFromWorkspaceCommand_WhenIsSuccessful_ShouldRemoveCollaboratorAndReturnSuccess()
    {
        // Arrange
        var workspace = WorkspaceFactory.CreateWithCollaborators();
        var collaborator = workspace.Memberships[1].User;
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(UserFactory.Create());

        var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand(
            workspace.Id.Value,
            collaboratorId: collaborator.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new KickCollaboratorFromWorkspaceResult());

        Utils.Workspace.AssertFromKickCollaboratorCommand(command, workspace);
    }

    [Fact]
    public async Task HandleKickCollaboratorFromWorkspaceCommand_WhenCurrentUserIsKicked_ShouldReturnKickYourselfError()
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        var workspace = WorkspaceFactory.CreateWithCollaborators();
        workspace.AddUserMembership(MembershipFactory.Create(user));
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand(
            collaboratorId: user.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.ValidateError(Errors.Workspace.KickYourself);
    }

    [Fact]
    public async Task HandleKickCollaboratorFromWorkspaceCommand_WhenOwnerIsKicked_ShouldReturnKickOwnerError()
    {
        // Arrange
        var workspace = WorkspaceFactory.CreateWithCollaborators();
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand(
            collaboratorId: workspace.Owner.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.KickOwner);
    }

    [Fact]
    public async Task HandleKickCollaboratorFromWorkspaceCommand_WhenCollaboratorsIsNull_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand();

        var workspace = WorkspaceFactory.CreateWithCollaborators();
        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);
    }

    [Fact]
    public async Task HandleKickCollaboratorFromWorkspaceCommand_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand();

        _workspaceRepository
            .GetWithCollaborators(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
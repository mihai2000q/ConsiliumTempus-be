using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Common.UnitTests.Workspace.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.RejectInvitation;

public class RejectInvitationToWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly RejectInvitationToWorkspaceCommandHandler _uut;

    public RejectInvitationToWorkspaceCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new RejectInvitationToWorkspaceCommandHandler(
            _currentUserProvider,
            _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleRejectInvitationToWorkspaceCommand_WhenIsSuccessful_ShouldRemoveInvitationAndReturnSuccessResponse()
    {
        // Arrange
        var user = UserFactory.Create();
        var workspace = WorkspaceFactory.Create();
        var invitation = WorkspaceInvitationFactory.Create(collaborator: user);
        var otherInvitation = WorkspaceInvitationFactory.Create();
        workspace.AddInvitation(invitation);
        workspace.AddInvitation(otherInvitation);
        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand(
            workspace.Id.Value,
            invitation.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new RejectInvitationToWorkspaceResult());

        Utils.Workspace.AssertFromRejectInvitationCommand(command, workspace, [otherInvitation]);
    }

    [Fact]
    public async Task HandleRejectInvitationToWorkspaceCommand_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var invitation = WorkspaceInvitationFactory.Create();
        workspace.AddInvitation(invitation);
        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand(
            workspace.Id.Value,
            invitation.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.ValidateError(Errors.User.NotFound);
        workspace.Invitations.Should().Equal(invitation);
    }

    [Fact]
    public async Task
        HandleRejectInvitationToWorkspaceCommand_WhenUserIsNotTheInvitedCollaborator_ShouldReturnInvitationNotFoundError()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var invitation = WorkspaceInvitationFactory.Create();
        workspace.AddInvitation(invitation);
        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _currentUserProvider
            .GetCurrentUser()
            .Returns(UserFactory.Create());

        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand(
            workspace.Id.Value,
            invitation.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.ValidateError(Errors.WorkspaceInvitation.NotFound);
        workspace.Invitations.Should().Equal(invitation);
    }

    [Fact]
    public async Task
        HandleRejectInvitationToWorkspaceCommand_WhenInvitationIsNull_ShouldReturnInvitationNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        outcome.ValidateError(Errors.WorkspaceInvitation.NotFound);
    }

    [Fact]
    public async Task HandleRejectInvitationToWorkspaceCommand_WhenWorkspaceIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand();

        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
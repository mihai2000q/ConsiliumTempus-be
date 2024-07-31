using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Common.UnitTests.Workspace.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.AcceptInvitation;

public class AcceptInvitationToWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly AcceptInvitationToWorkspaceCommandHandler _uut;

    public AcceptInvitationToWorkspaceCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new AcceptInvitationToWorkspaceCommandHandler(
            _currentUserProvider,
            _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleAcceptInvitationToWorkspaceCommand_WhenIsSuccessful_ShouldAddUserRemoveInvitationAndReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var invitation = WorkspaceInvitationFactory.Create();
        workspace.AddInvitation(invitation);
        _workspaceRepository
            .GetWithCollaboratorsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand(
            workspace.Id.Value,
            invitation.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaboratorsAndInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new AcceptInvitationToWorkspaceResult());

        Utils.Workspace.AssertFromAcceptInvitationCommand(command, workspace, user);
    }

    [Fact]
    public async Task HandleAcceptInvitationToWorkspaceCommand_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var invitation = WorkspaceInvitationFactory.Create();
        workspace.AddInvitation(invitation);
        _workspaceRepository
            .GetWithCollaboratorsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand(
            workspace.Id.Value,
            invitation.Id.Value);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaboratorsAndInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.ValidateError(Errors.User.NotFound);
    }
    
    [Fact]
    public async Task HandleAcceptInvitationToWorkspaceCommand_WhenInvitationIsNull_ShouldReturnInvitationNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithCollaboratorsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaboratorsAndInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.WorkspaceInvitation.NotFound);
    }
    
    [Fact]
    public async Task HandleAcceptInvitationToWorkspaceCommand_WhenWorkspaceIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand();

        _workspaceRepository
            .GetWithCollaboratorsAndInvitations(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithCollaboratorsAndInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
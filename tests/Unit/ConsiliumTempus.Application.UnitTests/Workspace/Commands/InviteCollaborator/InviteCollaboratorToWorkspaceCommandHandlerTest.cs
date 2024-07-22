using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Common.UnitTests.Workspace.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.InviteCollaborator;

public class InviteCollaboratorToWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly InviteCollaboratorToWorkspaceCommandHandler _uut;

    public InviteCollaboratorToWorkspaceCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new InviteCollaboratorToWorkspaceCommandHandler(
            _currentUserProvider,
            _workspaceRepository,
            _userRepository);
    }

    #endregion

    [Fact]
    public async Task HandleInviteCollaboratorToWorkspaceCommand_WhenIsSuccessful_ShouldSendInvitationAndReturnResponse()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithMembershipsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var collaborator = UserFactory.Create();
        _userRepository
            .GetByEmail(Arg.Any<string>())
            .Returns(collaborator);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(
            workspace.Id.Value,
            collaborator.Credentials.Email);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMembershipsAndInvitations(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new InviteCollaboratorToWorkspaceResult());

        Utils.Workspace.AssertFromInviteCollaboratorCommand(
            command, 
            workspace,
            user,
            collaborator);
    }

    [Fact]
    public async Task HandleInviteCollaboratorToWorkspaceCommand_WhenWorkspaceAlreadyHasCollaborator_ShouldReturnAlreadyCollaboratorError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithMembershipsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var collaborator = UserFactory.Create();
        _userRepository
            .GetByEmail(Arg.Any<string>())
            .Returns(collaborator);

        workspace.AddUserMembership(MembershipFactory.Create(user: collaborator));

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMembershipsAndInvitations(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.WorkspaceInvitation.AlreadyCollaborator);
    }

    [Fact]
    public async Task HandleInviteCollaboratorToWorkspaceCommand_WhenWorkspaceHasCollaboratorInvited_ShouldReturnAlreadyInvitedError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithMembershipsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var collaborator = UserFactory.Create();
        _userRepository
            .GetByEmail(Arg.Any<string>())
            .Returns(collaborator);

        workspace.AddInvitation(WorkspaceInvitationFactory.Create(collaborator: collaborator));

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMembershipsAndInvitations(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.WorkspaceInvitation.AlreadyInvited);
    }

    [Fact]
    public async Task HandleInviteCollaboratorToWorkspaceCommand_WhenCollaboratorIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithMembershipsAndInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _userRepository
            .GetByEmail(Arg.Any<string>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMembershipsAndInvitations(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.User.NotFound);
    }

    [Fact]
    public async Task HandleInviteCollaboratorToWorkspaceCommand_WhenWorkspaceIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand();

        _workspaceRepository
            .GetWithMembershipsAndInvitations(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMembershipsAndInvitations(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _userRepository.DidNotReceive();
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
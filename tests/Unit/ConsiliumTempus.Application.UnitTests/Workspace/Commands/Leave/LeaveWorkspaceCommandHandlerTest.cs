using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Leave;

public class LeaveWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly LeaveWorkspaceCommandHandler _uut;

    public LeaveWorkspaceCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new LeaveWorkspaceCommandHandler(
            _currentUserProvider,
            _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleLeaveWorkspaceCommand_WhenIsSuccessful_ShouldSendInvitationAndReturnResponse()
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        var workspace = WorkspaceFactory.Create();
        workspace.AddUserMembership(MembershipFactory.Create(user));
        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateLeaveWorkspaceCommand(
            workspace.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new LeaveWorkspaceResult());

        Utils.Workspace.AssertFromLeaveCommand(command, workspace, user);
    }

    [Fact]
    public async Task HandleLeaveWorkspaceCommand_WhenWorkspaceIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateLeaveWorkspaceCommand();

        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
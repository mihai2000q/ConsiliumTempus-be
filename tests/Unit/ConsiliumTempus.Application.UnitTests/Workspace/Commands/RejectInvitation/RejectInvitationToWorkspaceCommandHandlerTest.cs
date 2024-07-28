using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Common.UnitTests.Workspace.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.RejectInvitation;

public class RejectInvitationToWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly RejectInvitationToWorkspaceCommandHandler _uut;

    public RejectInvitationToWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new RejectInvitationToWorkspaceCommandHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleRejectInvitationToWorkspaceCommand_WhenIsSuccessful_ShouldRemoveInvitationAndReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var invitation = WorkspaceInvitationFactory.Create();
        workspace.AddInvitation(invitation);
        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand(
            workspace.Id.Value,
            invitation.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new RejectInvitationToWorkspaceResult());

        Utils.Workspace.AssertFromRejectInvitationCommand(command, workspace);
    }

    [Fact]
    public async Task HandleRejectInvitationToWorkspaceCommand_WhenInvitationIsNull_ShouldReturnInvitationNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithInvitations(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, default);

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
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithInvitations(Arg.Is<WorkspaceId>(wId => wId.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
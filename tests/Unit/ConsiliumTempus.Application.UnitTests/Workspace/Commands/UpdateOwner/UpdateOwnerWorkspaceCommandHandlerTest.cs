using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateOwner;

public class UpdateOwnerWorkspaceCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UpdateOwnerWorkspaceCommandHandler _uut;

    public UpdateOwnerWorkspaceCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UpdateOwnerWorkspaceCommandHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateOwnerWorkspaceCommand_WhenIsSuccessful_ShouldUpdateOwnerAndReturnSuccess()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var owner = UserFactory.Create();
        workspace.AddUserMembership(MembershipFactory.Create(owner));
        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var command = WorkspaceCommandFactory.CreateUpdateOwnerWorkspaceCommand(
            id: workspace.Id.Value,
            ownerId: owner.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateOwnerWorkspaceResult());

        Utils.Workspace.AssertFromUpdateOwnerCommand(workspace, command, owner);
    }

    [Fact]
    public async Task
        HandleUpdateOwnerWorkspaceCommand_WhenCollaboratorIsNotFound_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateUpdateOwnerWorkspaceCommand();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);
    }

    [Fact]
    public async Task HandleUpdateOwnerWorkspaceCommand_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateUpdateOwnerWorkspaceCommand();

        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}
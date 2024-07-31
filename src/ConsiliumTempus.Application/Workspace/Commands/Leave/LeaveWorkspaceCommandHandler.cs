using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Leave;

public sealed class LeaveWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<LeaveWorkspaceCommand, ErrorOr<LeaveWorkspaceResult>>
{
    public async Task<ErrorOr<LeaveWorkspaceResult>> Handle(LeaveWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithCollaborators(
            WorkspaceId.Create(command.Id),
            cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        // TODO: Remove this, and use the memberships
        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        if (workspace.Owner == user) return Errors.Workspace.LeaveOwnedWorkspace;

        var membership = workspace.Memberships.Single(m => m.User == user);
        workspace.RemoveUserMembership(membership);
        workspace.RefreshActivity();

        return new LeaveWorkspaceResult();
    }
}
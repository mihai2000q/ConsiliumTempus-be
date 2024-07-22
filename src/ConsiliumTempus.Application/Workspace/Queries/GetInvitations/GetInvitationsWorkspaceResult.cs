using ConsiliumTempus.Domain.Workspace.Entities;

namespace ConsiliumTempus.Application.Workspace.Queries.GetInvitations;

public sealed record GetInvitationsWorkspaceResult(
    List<WorkspaceInvitation> Invitations,
    int TotalCount);
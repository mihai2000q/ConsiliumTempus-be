namespace ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;

public sealed record AcceptInvitationToWorkspaceRequest(
    Guid Id,
    Guid InvitationId);
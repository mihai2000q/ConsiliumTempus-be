namespace ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;

public sealed record RejectInvitationToWorkspaceRequest(
    Guid Id,
    Guid InvitationId);
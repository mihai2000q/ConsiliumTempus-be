namespace ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;

public sealed record InviteCollaboratorToWorkspaceRequest(
    Guid Id,
    string Email);
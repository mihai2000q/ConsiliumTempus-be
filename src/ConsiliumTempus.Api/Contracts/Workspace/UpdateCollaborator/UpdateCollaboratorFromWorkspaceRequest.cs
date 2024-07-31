namespace ConsiliumTempus.Api.Contracts.Workspace.UpdateCollaborator;

public sealed record UpdateCollaboratorFromWorkspaceRequest(
    Guid Id,
    Guid CollaboratorId,
    string WorkspaceRole);
namespace ConsiliumTempus.Api.Contracts.Workspace.UpdateOwner;

public sealed record UpdateOwnerWorkspaceRequest(
    Guid Id,
    Guid OwnerId);
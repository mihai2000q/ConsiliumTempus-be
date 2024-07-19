namespace ConsiliumTempus.Api.Contracts.Workspace.Update;

public sealed record UpdateWorkspaceRequest(
    Guid Id,
    string Name);
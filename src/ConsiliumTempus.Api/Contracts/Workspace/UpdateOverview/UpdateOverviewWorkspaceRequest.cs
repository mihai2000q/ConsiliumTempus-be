namespace ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;

public sealed record UpdateOverviewWorkspaceRequest(
    Guid Id,
    string Description);
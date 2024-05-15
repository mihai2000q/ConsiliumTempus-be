namespace ConsiliumTempus.Api.Contracts.Project.UpdateOverview;

public sealed record UpdateOverviewProjectRequest(
    Guid Id,
    string Description);
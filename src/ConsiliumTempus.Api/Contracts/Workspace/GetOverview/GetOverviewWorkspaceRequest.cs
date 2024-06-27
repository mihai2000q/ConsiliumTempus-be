using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetOverview;

public sealed record GetOverviewWorkspaceRequest
{
    [FromRoute] public Guid Id { get; init; }
}
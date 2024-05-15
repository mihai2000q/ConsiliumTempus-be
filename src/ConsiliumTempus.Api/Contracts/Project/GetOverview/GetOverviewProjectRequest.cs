using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetOverview;

public sealed record GetOverviewProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
}
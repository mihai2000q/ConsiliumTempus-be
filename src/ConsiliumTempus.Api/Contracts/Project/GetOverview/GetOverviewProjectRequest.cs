using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetOverview;

public sealed class GetOverviewProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
}
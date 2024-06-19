using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;

public sealed record GetCollectionProjectSprintRequest
{
    [FromQuery] public Guid ProjectId { get; init; }
    [FromQuery] public string[]? Search { get; init; }
    [FromQuery] public bool FromThisYear { get; init; }
}
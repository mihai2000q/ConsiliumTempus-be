using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;

public sealed record GetCollectionProjectTaskRequest
{
    [FromQuery] public Guid ProjectStageId { get; init; }
    [FromQuery] public string[]? Search { get; init; }
    [FromQuery] public string[]? OrderBy { get; init; }
    [FromQuery] public int? CurrentPage { get; init; }
    [FromQuery] public int? PageSize { get; init; }
}
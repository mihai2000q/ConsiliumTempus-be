using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollection;

public sealed record GetCollectionProjectRequest
{
    [FromQuery] public int? PageSize { get; init; }

    [FromQuery] public int? CurrentPage { get; init; }

    [FromQuery] public List<string> OrderBy { get; init; } = [];

    [FromQuery] public List<string> Search { get; init; } = [];

    [FromQuery] public Guid? WorkspaceId { get; init; }
}
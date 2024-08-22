using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollection;

public sealed record GetCollectionWorkspaceRequest
{
    [FromQuery] public bool IsPersonalWorkspaceFirst { get; init; }

    [FromQuery] public int? PageSize { get; init; }

    [FromQuery] public int? CurrentPage { get; init; }

    [FromQuery] public List<string> OrderBy { get; init; } = [];

    [FromQuery] public List<string> Search { get; init; } = [];
}
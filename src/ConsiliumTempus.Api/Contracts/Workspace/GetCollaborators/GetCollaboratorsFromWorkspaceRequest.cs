using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;

public sealed record GetCollaboratorsFromWorkspaceRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromQuery] public int? CurrentPage { get; init; }
    [FromQuery] public int? PageSize { get; init; }
    [FromQuery] public List<string> OrderBy { get; init; } = [];
    [FromQuery] public List<string> Search { get; init; } = [];
    [FromQuery] public string? SearchValue { get; init; }
}
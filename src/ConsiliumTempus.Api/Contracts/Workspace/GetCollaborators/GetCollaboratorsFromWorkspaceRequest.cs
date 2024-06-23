using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;

public sealed record GetCollaboratorsFromWorkspaceRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromQuery] public string? SearchValue { get; init; }
}
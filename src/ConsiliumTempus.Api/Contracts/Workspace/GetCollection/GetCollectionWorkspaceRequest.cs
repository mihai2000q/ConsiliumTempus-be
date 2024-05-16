using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollection;

public sealed record GetCollectionWorkspaceRequest
{
    [FromQuery] public bool IsPersonalWorkspaceFirst { get; init; }
    
    [FromQuery] public int? PageSize { get; init; }
    
    [FromQuery] public int? CurrentPage { get; init; }
    
    [FromQuery] public string? Order { get; init; }
    
    [FromQuery] public string? Name { get; init; }
}
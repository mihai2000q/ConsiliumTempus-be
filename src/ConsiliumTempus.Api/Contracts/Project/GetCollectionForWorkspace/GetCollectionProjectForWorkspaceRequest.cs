using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;

public sealed class GetCollectionProjectForWorkspaceRequest
{
    [FromQuery] public Guid WorkspaceId { get; init; }
    
    [FromQuery] public string? Name { get; init; }
    
    [FromQuery] public bool? IsFavorite { get; init; }
    
    [FromQuery] public bool? IsPrivate { get; init; }
}
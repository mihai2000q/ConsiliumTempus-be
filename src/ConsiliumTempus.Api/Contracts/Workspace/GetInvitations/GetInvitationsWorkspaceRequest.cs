using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;

public sealed record GetInvitationsWorkspaceRequest
{
    [FromQuery] public bool? IsSender { get; init; }
    [FromQuery] public Guid? WorkspaceId { get; init; }
    [FromQuery] public int? PageSize { get; init; }
    
    [FromQuery] public int? CurrentPage { get; init; }
}
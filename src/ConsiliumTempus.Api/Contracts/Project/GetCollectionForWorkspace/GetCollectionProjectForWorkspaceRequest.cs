using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;

public sealed class GetCollectionProjectForWorkspaceRequest
{
    [FromQuery]
    public Guid WorkspaceId { get; init; }
}
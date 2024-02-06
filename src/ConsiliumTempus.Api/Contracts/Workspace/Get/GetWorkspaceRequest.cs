using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.Get;

public class GetWorkspaceRequest
{
    [FromRoute] 
    public string Id { get; set; } = string.Empty;
}
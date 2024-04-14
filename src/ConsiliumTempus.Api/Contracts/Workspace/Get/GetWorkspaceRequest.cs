using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.Get;

public sealed class GetWorkspaceRequest
{
    [FromRoute]
    public Guid Id { get; init; }
}
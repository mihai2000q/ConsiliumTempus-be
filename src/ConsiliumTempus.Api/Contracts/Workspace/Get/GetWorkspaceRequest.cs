using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.Get;

public sealed record GetWorkspaceRequest
{
    [FromRoute]
    public Guid Id { get; init; }
}
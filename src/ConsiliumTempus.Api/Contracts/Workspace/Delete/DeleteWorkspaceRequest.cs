using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.Delete;

public sealed record DeleteWorkspaceRequest
{
    [FromRoute] public Guid Id { get; init; }
}
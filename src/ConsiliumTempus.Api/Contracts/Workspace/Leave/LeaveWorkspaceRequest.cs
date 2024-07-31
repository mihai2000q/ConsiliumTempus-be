using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.Leave;

public sealed record LeaveWorkspaceRequest
{
    [FromRoute] public Guid Id { get; init; }
}
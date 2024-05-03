using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollection;

public sealed class GetCollectionWorkspaceRequest
{
    [FromQuery] public string? Order { get; init; }
}
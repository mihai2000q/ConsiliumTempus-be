using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionWorkspaceResponse(
    List<GetCollectionWorkspaceResponse.WorkspaceResponse> Workspaces)
{
    public sealed record WorkspaceResponse(
        Guid Id,
        string Name,
        string Description);
}
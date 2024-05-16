using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionWorkspaceResponse(
    List<GetCollectionWorkspaceResponse.WorkspaceResponse> Workspaces,
    int TotalCount,
    int? TotalPages)
{
    public sealed record WorkspaceResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsPersonal,
        Owner Owner);

    public sealed record Owner(
        Guid Id,
        string Name);
}
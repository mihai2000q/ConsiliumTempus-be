using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionWorkspaceResponse(
    List<GetCollectionWorkspaceResponse.WorkspaceResponse> Workspaces,
    int TotalCount)
{
    public sealed record WorkspaceResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsFavorite,
        bool IsPersonal,
        UserResponse Owner);

    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}
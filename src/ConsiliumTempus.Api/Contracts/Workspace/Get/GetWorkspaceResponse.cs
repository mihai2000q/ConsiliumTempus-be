using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetWorkspaceResponse(
    string Name,
    bool IsFavorite,
    bool IsPersonal,
    GetWorkspaceResponse.UserResponse Owner)
{
    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}
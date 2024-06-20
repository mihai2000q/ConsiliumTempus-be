using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollaboratorsFromWorkspaceResponse(
    List<GetCollaboratorsFromWorkspaceResponse.UserResponse> Collaborators)
{
    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}
using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollaboratorsFromWorkspaceResponse(
    List<GetCollaboratorsFromWorkspaceResponse.CollaboratorResponse> Collaborators,
    int TotalCount)
{
    public sealed record CollaboratorResponse(
        Guid Id,
        string Name,
        string Email,
        string WorkspaceRole);
}
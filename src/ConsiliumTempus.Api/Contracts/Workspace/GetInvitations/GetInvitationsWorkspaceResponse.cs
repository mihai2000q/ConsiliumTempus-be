using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetInvitationsWorkspaceResponse(
    List<GetInvitationsWorkspaceResponse.WorkspaceInvitationResponse> Invitations,
    int TotalCount)
{
    public sealed record WorkspaceInvitationResponse(
        Guid Id,
        UserResponse Sender,
        UserResponse Collaborator,
        WorkspaceResponse Workspace);

    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);

    public sealed record WorkspaceResponse(
        Guid Id,
        string Name,
        bool IsPersonal);
}
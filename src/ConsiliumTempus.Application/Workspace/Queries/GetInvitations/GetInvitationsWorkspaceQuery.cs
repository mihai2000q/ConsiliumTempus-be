using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetInvitations;

public sealed record GetInvitationsWorkspaceQuery(
    bool? IsSender,
    Guid? WorkspaceId,
    int? PageSize,
    int? CurrentPage)
    : IRequest<ErrorOr<GetInvitationsWorkspaceResult>>;
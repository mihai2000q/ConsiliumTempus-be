using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetInvitations;

public sealed class GetInvitationsWorkspaceQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetInvitationsWorkspaceQuery, ErrorOr<GetInvitationsWorkspaceResult>>
{
    public async Task<ErrorOr<GetInvitationsWorkspaceResult>> Handle(GetInvitationsWorkspaceQuery query,
        CancellationToken cancellationToken)
    {
        var user = query.IsSender is not null 
            ? await currentUserProvider.GetCurrentUser(cancellationToken)
            : null;
        if (query.IsSender is not null && user is null) return Errors.User.NotFound;

        var paginationInfo = PaginationInfo.Create(query.PageSize, query.CurrentPage);
        var workspaceId = query.WorkspaceId.IfNotNull(WorkspaceId.Create);

        var invitations = await workspaceRepository.GetInvitations(
            user,
            query.IsSender,
            workspaceId,
            paginationInfo,
            cancellationToken);
        var totalCount = await workspaceRepository.GetInvitationsCount(
            user,
            query.IsSender,
            workspaceId,
            cancellationToken);

        return new GetInvitationsWorkspaceResult(invitations, totalCount);
    }
}
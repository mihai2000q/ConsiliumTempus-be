using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.User;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

internal sealed class GetCollectionWorkspaceQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetCollectionWorkspaceQuery, ErrorOr<GetCollectionWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollectionWorkspaceResult>> Handle(GetCollectionWorkspaceQuery query,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;

        var paginationInfo = PaginationInfo.Create(query.PageSize, query.CurrentPage);
        var orders = WorkspaceOrder.Parse(query.OrderBy);
        if (query.IsPersonalFirst) orders = [PersonalFirst(user), ..orders];
        var filters = WorkspaceFilter.Parse(query.Search);

        var workspaces = await workspaceRepository.GetListByUser(
            user,
            paginationInfo,
            orders,
            filters,
            cancellationToken);
        var workspacesCount = await workspaceRepository.GetListByUserCount(
            user,
            filters,
            cancellationToken);

        return new GetCollectionWorkspaceResult(
            workspaces,
            workspacesCount,
            user);
    }

    // The user can also be a member of other people's personal workspaces,
    // so only the one that the user owns is the personal workspace of the user
    private static WorkspaceOrder PersonalFirst(UserAggregate user)
    {
        return new WorkspaceOrder(w => w.Owner == user && w.IsPersonal.Value, OrderType.Descending);
    }
}
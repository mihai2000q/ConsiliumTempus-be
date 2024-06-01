using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed class GetCollectionProjectQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<GetCollectionProjectQuery, ErrorOr<GetCollectionProjectResult>>
{
    public async Task<ErrorOr<GetCollectionProjectResult>> Handle(
        GetCollectionProjectQuery query,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var paginationInfo = PaginationInfo.Create(query.PageSize, query.CurrentPage);
        var orders = ProjectOrder.Parse(query.OrderBy);
        var filters = ProjectFilter.Parse(query.Search);
        var workspaceId = query.WorkspaceId.IfNotNull(WorkspaceId.Create);

        var projects = await projectRepository.GetListByUser(
            user.Id,
            workspaceId,
            paginationInfo,
            orders,
            filters,
            cancellationToken);
        var totalCount = await projectRepository.GetListByUserCount(
            user.Id,
            workspaceId,
            filters,
            cancellationToken);

        return new GetCollectionProjectResult(
            projects,
            totalCount,
            paginationInfo?.GetTotalPages(totalCount));
    }
}
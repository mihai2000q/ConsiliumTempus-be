using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed class GetCollaboratorsFromWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetCollaboratorsFromWorkspaceQuery, ErrorOr<GetCollaboratorsFromWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollaboratorsFromWorkspaceResult>> Handle(GetCollaboratorsFromWorkspaceQuery query,
        CancellationToken cancellationToken)
    {
        var workspaceId = WorkspaceId.Create(query.Id);
        var filters = MembershipFilter.Parse(query.Search);
        var orders = MembershipOrder.Parse(query.OrderBy);
        var paginationInfo = PaginationInfo.Create(query.PageSize, query.CurrentPage);
        var searchValue = query.SearchValue?.Trim();

        var collaborators = await workspaceRepository.GetCollaborators(
            workspaceId,
            searchValue,
            filters, 
            orders,
            paginationInfo, 
            cancellationToken);
        var totalCount = await workspaceRepository.GetCollaboratorsCount(
            workspaceId,
            searchValue,
            filters,
            cancellationToken);

        return new GetCollaboratorsFromWorkspaceResult(collaborators, totalCount);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetOverview;

public sealed class GetOverviewWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetOverviewWorkspaceQuery, ErrorOr<WorkspaceAggregate>>
{
    public async Task<ErrorOr<WorkspaceAggregate>> Handle(GetOverviewWorkspaceQuery query, CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(query.Id), cancellationToken);
        return workspace is not null ? workspace : Errors.Workspace.NotFound;
    }
}
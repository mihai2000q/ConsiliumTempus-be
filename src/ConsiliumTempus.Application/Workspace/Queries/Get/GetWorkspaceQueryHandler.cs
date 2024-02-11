using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public sealed class GetWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetWorkspaceQuery, ErrorOr<WorkspaceAggregate>>
{
    public async Task<ErrorOr<WorkspaceAggregate>> Handle(GetWorkspaceQuery query, CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(query.Id), cancellationToken);
        return workspace is not null ? workspace : Errors.Workspace.NotFound;
    }
}
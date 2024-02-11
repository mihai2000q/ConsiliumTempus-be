using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed class GetCollectionWorkspaceQueryHandler(
    IWorkspaceRepository workspaceRepository,
    ISecurity security)
    : IRequestHandler<GetCollectionWorkspaceQuery, List<WorkspaceAggregate>>
{
    public async Task<List<WorkspaceAggregate>> Handle(GetCollectionWorkspaceQuery workspaceQuery, 
        CancellationToken cancellationToken)
    {
        return await workspaceRepository.GetListForUser(
            await security.GetUserFromToken(workspaceQuery.Token, cancellationToken),
            cancellationToken);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed class GetCollectionWorkspaceQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetCollectionWorkspaceQuery, List<WorkspaceAggregate>>
{
    public async Task<List<WorkspaceAggregate>> Handle(GetCollectionWorkspaceQuery workspaceQuery, 
        CancellationToken cancellationToken)
    {
        return await workspaceRepository.GetListForUser(
            await currentUserProvider.GetCurrentUser(cancellationToken),
            cancellationToken);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

public sealed class GetCollectionProjectForWorkspaceQueryHandler(
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<GetCollectionProjectForWorkspaceQuery, ErrorOr<GetCollectionProjectForWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollectionProjectForWorkspaceResult>> Handle(GetCollectionProjectForWorkspaceQuery query, 
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(query.WorkspaceId), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;
        return new GetCollectionProjectForWorkspaceResult(
            await projectRepository.GetListForWorkspace(workspace.Id, cancellationToken));
    }
}
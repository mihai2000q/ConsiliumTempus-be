using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

public sealed class GetCollectionProjectForWorkspaceQueryHandler(
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<GetCollectionProjectForWorkspaceQuery, ErrorOr<GetCollectionProjectForWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollectionProjectForWorkspaceResult>> Handle(
        GetCollectionProjectForWorkspaceQuery query,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(query.WorkspaceId), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var filters = new List<IFilter<ProjectAggregate>>
        {
            new Filters.Project.NameFilter(query.Name),
            new Filters.Project.IsFavoriteFilter(query.IsFavorite),
            new Filters.Project.IsPrivateFilter(query.IsPrivate)
        };

        var projects = await projectRepository.GetListByWorkspace(
            workspace.Id,
            filters, 
            cancellationToken);
        return new GetCollectionProjectForWorkspaceResult(projects);
    }
}
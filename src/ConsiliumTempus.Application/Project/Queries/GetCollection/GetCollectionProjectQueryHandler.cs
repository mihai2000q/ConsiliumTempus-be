using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
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

        var filters = new List<IFilter<ProjectAggregate>>
        {
            new Filters.Project.WorkspaceFilter(query.WorkspaceId.IfNotNull(() =>
                WorkspaceId.Create(query.WorkspaceId!.Value))),
            new Filters.Project.NameFilter(query.Name),
            new Filters.Project.IsFavoriteFilter(query.IsFavorite),
            new Filters.Project.IsPrivateFilter(query.IsPrivate)
        };

        var projects = await projectRepository.GetListByUser(
            user.Id,
            filters,
            cancellationToken);
        return new GetCollectionProjectResult(projects);
    }
}
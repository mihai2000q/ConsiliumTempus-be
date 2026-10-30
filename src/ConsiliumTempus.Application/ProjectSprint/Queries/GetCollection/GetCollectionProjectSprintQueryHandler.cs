using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

internal sealed class GetCollectionProjectSprintQueryHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<GetCollectionProjectSprintQuery, ErrorOr<GetCollectionProjectSprintResult>>
{
    public async Task<ErrorOr<GetCollectionProjectSprintResult>> Handle(
        GetCollectionProjectSprintQuery query,
        CancellationToken cancellationToken)
    {
        var projectId = ProjectId.Create(query.ProjectId);
        var searchFilters = ProjectSprintFilter.Parse(query.Search);
        var filters = query.FromThisYear
            ? [..searchFilters, ProjectSprintFilter.FromThisYear()]
            : searchFilters;

        var sprints = await projectSprintRepository.GetListByProject(
            projectId,
            filters,
            cancellationToken);
        var totalCount = await projectSprintRepository.GetListByProjectCount(
            projectId,
            filters,
            cancellationToken);

        if (!sprints.IsEmpty() || !searchFilters.IsEmpty() || !query.FromThisYear)
            return new GetCollectionProjectSprintResult(sprints, totalCount);

        var firstSprint = await projectSprintRepository.GetFirstByProject(projectId, searchFilters, cancellationToken);
        return firstSprint is not null 
            ? new GetCollectionProjectSprintResult([firstSprint], 1) 
            : new GetCollectionProjectSprintResult(sprints, totalCount);
    }
}
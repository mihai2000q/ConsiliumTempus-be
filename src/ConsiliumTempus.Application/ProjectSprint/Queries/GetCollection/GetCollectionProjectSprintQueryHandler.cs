using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

public sealed class GetCollectionProjectSprintQueryHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<GetCollectionProjectSprintQuery, ErrorOr<GetCollectionProjectSprintResult>>
{
    public async Task<ErrorOr<GetCollectionProjectSprintResult>> Handle(
        GetCollectionProjectSprintQuery query,
        CancellationToken cancellationToken)
    {
        var projectId = ProjectId.Create(query.ProjectId);
        var filters = ProjectSprintFilter.Parse(query.Search);

        var sprints = await projectSprintRepository.GetListByProject(
            projectId,
            filters,
            query.FromThisYear,
            cancellationToken);
        var totalCount = await projectSprintRepository.GetListByProjectCount(
            projectId,
            filters,
            query.FromThisYear,
            cancellationToken);

        if (sprints.IsEmpty() && filters.IsEmpty() && query.FromThisYear)
        {
            sprints = [await projectSprintRepository.GetFirstByProject(projectId, filters, cancellationToken)];
        }

        return new GetCollectionProjectSprintResult(sprints, totalCount);
    }
}
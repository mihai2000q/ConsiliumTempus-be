using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
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
        var sprints = await projectSprintRepository.GetListByProject(
            projectId,
            cancellationToken);
        var totalCount = await projectSprintRepository.GetListByProjectCount(
            projectId,
            cancellationToken);

        return new GetCollectionProjectSprintResult(sprints, totalCount);
    }
}
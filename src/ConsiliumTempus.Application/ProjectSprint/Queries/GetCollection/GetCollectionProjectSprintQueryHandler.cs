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
        var sprints = await projectSprintRepository.GetListByProject(
            ProjectId.Create(query.ProjectId),
            cancellationToken);

        return new GetCollectionProjectSprintResult(sprints);
    }
}
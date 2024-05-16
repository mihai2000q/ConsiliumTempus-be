using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;

public sealed class GetCollectionProjectStageQueryHandler(IProjectStageRepository projectStageRepository)
    : IRequestHandler<GetCollectionProjectStageQuery, ErrorOr<GetCollectionProjectStageResult>>
{
    public async Task<ErrorOr<GetCollectionProjectStageResult>> Handle(GetCollectionProjectStageQuery query,
        CancellationToken cancellationToken)
    {
        var stages = await projectStageRepository.GetListBySprint(
            ProjectSprintId.Create(query.ProjectSprintId),
            cancellationToken);

        return new GetCollectionProjectStageResult(stages);
    }
}
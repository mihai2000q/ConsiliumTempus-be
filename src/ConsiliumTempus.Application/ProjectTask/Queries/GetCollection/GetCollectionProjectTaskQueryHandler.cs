using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public sealed class GetCollectionProjectTaskQueryHandler(IProjectTaskRepository projectTaskRepository)
    : IRequestHandler<GetCollectionProjectTaskQuery, ErrorOr<GetCollectionProjectTaskResult>>
{
    public async Task<ErrorOr<GetCollectionProjectTaskResult>> Handle(GetCollectionProjectTaskQuery query, 
        CancellationToken cancellationToken)
    {
        var tasks = await projectTaskRepository.GetListByStage(
            ProjectStageId.Create(query.ProjectStageId), 
            [],
            cancellationToken);
        var totalCount = await projectTaskRepository.GetListByStageCount(
            ProjectStageId.Create(query.ProjectStageId),
            [], 
            cancellationToken);
        
        return new GetCollectionProjectTaskResult(
            tasks,
            totalCount);
    }
}
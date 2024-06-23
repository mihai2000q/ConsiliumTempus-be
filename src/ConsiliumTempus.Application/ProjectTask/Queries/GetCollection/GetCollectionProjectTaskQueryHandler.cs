using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
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
        var stageId = ProjectStageId.Create(query.ProjectStageId);
        var paginationInfo = PaginationInfo.Create(query.PageSize, query.CurrentPage);
        var filters = ProjectTaskFilter.Parse(query.Search);
        var orders = ProjectTaskOrder.Parse(query.OrderBy);

        var tasks = await projectTaskRepository.GetListByStage(
            stageId,
            filters,
            orders,
            paginationInfo,
            cancellationToken);
        var totalCount = await projectTaskRepository.GetListByStageCount(
            stageId,
            filters,
            cancellationToken);

        return new GetCollectionProjectTaskResult(
            tasks,
            totalCount);
    }
}
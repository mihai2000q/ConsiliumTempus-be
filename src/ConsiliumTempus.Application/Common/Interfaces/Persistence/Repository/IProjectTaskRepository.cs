using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectTaskRepository
{
    Task<ProjectTaskAggregate?> GetWithWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithStagesAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<List<ProjectTaskAggregate>> GetListByStage(
        ProjectStageId stageId,
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        IReadOnlyList<IOrder<ProjectTaskAggregate>> orders,
        PaginationInfo? paginationInfo,
        CancellationToken cancellationToken = default);

    Task<int> GetListByStageCount(
        ProjectStageId stageId,
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default);
}
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectTaskRepository
{
    Task<ProjectTaskAggregate?> Get(ProjectTaskId id, CancellationToken cancellationToken = default);

    Task<List<ProjectTaskAggregate>> GetListByStage(
        ProjectStageId stageId, 
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default);
    
    Task<int> GetListByStageCount(
        ProjectStageId stageId, 
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task Add(ProjectTaskAggregate task, CancellationToken cancellationToken = default);

    void Remove(ProjectTaskAggregate task);
}
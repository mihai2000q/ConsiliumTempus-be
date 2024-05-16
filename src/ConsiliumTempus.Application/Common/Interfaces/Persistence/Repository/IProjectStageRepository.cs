using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectStageRepository
{
    Task<ProjectStage?> Get(ProjectStageId id, CancellationToken cancellationToken = default);

    Task<ProjectStage?> GetWithTasksAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default);

    Task<ProjectStage?> GetWithWorkspace(ProjectStageId id, CancellationToken cancellationToken = default);

    Task<ProjectStage?> GetWithStagesAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default);

    Task<List<ProjectStage>> GetListBySprint(ProjectSprintId id, CancellationToken cancellationToken = default);
}
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectStageRepository
{
    Task<ProjectStage?> GetWithWorkspace(ProjectStageId id, CancellationToken cancellationToken = default);
    
    Task<ProjectStage?> GetWithStagesAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default);

    Task Add(ProjectStage stage, CancellationToken cancellationToken = default);
}
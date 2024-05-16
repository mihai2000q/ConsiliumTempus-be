using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectStageRepository
{
    Task<ProjectStage?> GetWithTasksAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default);
}
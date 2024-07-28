using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

public interface IProjectProvider
{
    Task<ProjectAggregate?> Get(ProjectId id, CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetByProjectSprint(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetByProjectStage(ProjectStageId id, CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetByProjectTask(ProjectTaskId id, CancellationToken cancellationToken = default);
}
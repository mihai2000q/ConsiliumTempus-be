using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectSprintRepository
{
    Task<ProjectSprintAggregate?> Get(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<ProjectSprintAggregate?> GetWithWorkspace(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<ProjectSprintAggregate?> GetWithSprintsAndWorkspace(
        ProjectSprintId id,
        CancellationToken cancellationToken = default);

    Task<ProjectSprintAggregate?> GetWithTasksAndWorkspace(
        ProjectSprintId id,
        CancellationToken cancellationToken = default);

    Task<ProjectSprintAggregate> GetFirstByProject(
        ProjectId projectId,
        IReadOnlyList<IFilter<ProjectSprintAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task<List<ProjectSprintAggregate>> GetListByProject(
        ProjectId projectId,
        IReadOnlyList<IFilter<ProjectSprintAggregate>> filters,
        bool fromThisYear,
        CancellationToken cancellationToken = default);

    Task<int> GetListByProjectCount(
        ProjectId projectId,
        IReadOnlyList<IFilter<ProjectSprintAggregate>> filters,
        bool fromThisYear,
        CancellationToken cancellationToken = default);

    Task<List<ProjectStage>> GetStages(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task Add(ProjectSprintAggregate sprint, CancellationToken cancellationToken = default);

    void Remove(ProjectSprintAggregate sprint);
}
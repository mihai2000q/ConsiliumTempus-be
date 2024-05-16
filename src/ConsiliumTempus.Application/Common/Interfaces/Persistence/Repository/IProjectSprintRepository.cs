using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectSprintRepository
{
    Task<ProjectSprintAggregate?> Get(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<ProjectSprintAggregate?> GetWithWorkspace(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<List<ProjectSprintAggregate>> GetListByProject(ProjectId projectId, CancellationToken cancellationToken = default);

    Task Add(ProjectSprintAggregate sprint, CancellationToken cancellationToken = default);

    void Remove(ProjectSprintAggregate sprint);
}
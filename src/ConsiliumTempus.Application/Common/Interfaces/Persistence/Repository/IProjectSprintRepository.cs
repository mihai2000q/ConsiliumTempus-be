using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectSprintRepository
{
    Task<ProjectSprint?> Get(ProjectSprintId id, CancellationToken cancellationToken = default);
    
    Task<ProjectSprint?> GetWithWorkspace(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<ProjectSprint?> GetWithStagesAndWorkspace(ProjectSprintId id, CancellationToken cancellationToken = default);

    Task<List<ProjectSprint>> GetListByProject(ProjectId projectId, CancellationToken cancellationToken = default);

    Task Add(ProjectSprint sprint, CancellationToken cancellationToken = default);

    void Remove(ProjectSprint sprint);
}
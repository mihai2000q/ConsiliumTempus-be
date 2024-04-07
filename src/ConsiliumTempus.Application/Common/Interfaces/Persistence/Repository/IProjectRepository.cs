using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectRepository
{
    Task<ProjectAggregate?> Get(ProjectId projectId, CancellationToken cancellationToken = default);

    Task<List<ProjectAggregate>> GetListForUser(UserId userId, CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetWithWorkspace(ProjectId id, CancellationToken cancellationToken = default);
    
    Task Add(ProjectAggregate project, CancellationToken cancellationToken = default);

    void Remove(ProjectAggregate project);
}
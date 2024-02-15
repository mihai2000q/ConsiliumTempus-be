using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectRepository
{
    Task<ProjectAggregate?> Get(ProjectId projectId, CancellationToken cancellationToken = default);
    
    Task Add(ProjectAggregate project, CancellationToken cancellationToken = default);

    void Remove(ProjectAggregate project);
}
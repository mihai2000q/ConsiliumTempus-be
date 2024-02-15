using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectRepository
{
    Task Add(ProjectAggregate project, CancellationToken cancellationToken = default);
}
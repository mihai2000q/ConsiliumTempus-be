using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectSprintRepository
{
    Task Add(ProjectSprint sprint, CancellationToken cancellationToken = default);
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectSprintRepository(ConsiliumTempusDbContext dbContext) : IProjectSprintRepository
{
    public async Task<ProjectSprint?> GetWithProjectAndWorkspace(ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints.FindAsync([id], cancellationToken);
    }

    public async Task Add(ProjectSprint sprint, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(sprint, cancellationToken);
    }

    public void Remove(ProjectSprint sprint)
    {
        dbContext.Remove(sprint);
    }
}
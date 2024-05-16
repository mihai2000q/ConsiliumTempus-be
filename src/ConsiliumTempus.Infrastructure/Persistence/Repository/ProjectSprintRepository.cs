using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectSprintRepository(ConsiliumTempusDbContext dbContext) : IProjectSprintRepository
{
    public async Task<ProjectSprintAggregate?> Get(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints
            .Include(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }
    
    public Task<ProjectSprintAggregate?> GetWithWorkspace(ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectSprints
            .Include(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Include(ps => ps.Project.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public Task<List<ProjectSprintAggregate>> GetListByProject(ProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectSprints
            .Where(ps => ps.Project.Id == projectId)
            .OrderBy(s => s.StartDate)
            .ThenBy(s => s.EndDate)
            .ThenBy(s => s.Name.Value)
            .ThenBy(s => s.CreatedDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task Add(ProjectSprintAggregate sprint, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(sprint, cancellationToken);
    }

    public void Remove(ProjectSprintAggregate sprint)
    {
        dbContext.Remove(sprint);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectSprintRepository(ConsiliumTempusDbContext dbContext) : IProjectSprintRepository
{
    public async Task<ProjectSprint?> Get(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints.FindAsync([id], cancellationToken);
    }
    
    public async Task<ProjectSprint?> GetWithProjectAndWorkspace(ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints
            .Include(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<ProjectSprint?> GetWithStagesProjectAndWorkspace(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints
            .Include(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .Include(ps => ps.Stages)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<List<ProjectSprint>> GetListByProject(ProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints
            .Where(ps => ps.Project.Id == projectId)
            .ToListAsync(cancellationToken);
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
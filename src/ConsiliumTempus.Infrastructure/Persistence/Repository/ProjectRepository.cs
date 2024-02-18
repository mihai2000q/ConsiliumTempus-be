using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectRepository(ConsiliumTempusDbContext dbContext) : IProjectRepository
{
    public async Task<ProjectAggregate?> Get(ProjectId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects.FindAsync([id], cancellationToken);
    }
    
    public async Task<ProjectAggregate?> GetWithWorkspace(ProjectId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<List<ProjectAggregate>> GetListForWorkspace(WorkspaceId workspaceId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Projects
            .Where(p => p.Workspace.Id == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public Task<List<ProjectAggregate>> GetListForUser(UserId userId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User.Id == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task Add(ProjectAggregate project, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(project, cancellationToken);
    }

    public void Remove(ProjectAggregate project)
    {
        dbContext.Remove(project);
    }
}
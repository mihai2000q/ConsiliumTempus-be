using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectStageRepository(ConsiliumTempusDbContext dbContext) : IProjectStageRepository
{
    public async Task<ProjectStage?> Get(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages.FindAsync([id], cancellationToken);
    }
    
    public async Task<ProjectStage?> GetWithTasksAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(ps => ps.Workspace)
            .Include(ps => ps.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<ProjectStage?> GetWithWorkspace(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<ProjectStage?> GetWithStagesAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }
    
    public Task<List<ProjectStage>> GetListBySprint(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectStages
            .Where(ps => ps.Sprint.Id == id)
            .OrderBy(ps => ps.CustomOrderPosition.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task Add(ProjectStage stage, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(stage, cancellationToken);
    }
}
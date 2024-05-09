using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectStageRepository(ConsiliumTempusDbContext dbContext) : IProjectStageRepository
{
    public async Task<ProjectStage?> GetWithWorkspace(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task Add(ProjectStage stage, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(stage, cancellationToken);
    }

    public void Remove(ProjectStage stage)
    {
        dbContext.Remove(stage);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectStageRepository(ConsiliumTempusDbContext dbContext) : IProjectStageRepository
{
    public async Task<ProjectStage?> Get(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages.FindAsync([id], cancellationToken);
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
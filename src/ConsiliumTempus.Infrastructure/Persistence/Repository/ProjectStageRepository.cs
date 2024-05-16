using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectStageRepository(ConsiliumTempusDbContext dbContext) : IProjectStageRepository
{
    public async Task<ProjectStage?> GetWithTasksAndWorkspace(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectStages
            .Include(ps => ps.Sprint.Project.Workspace)
            .Include(ps => ps.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }
}
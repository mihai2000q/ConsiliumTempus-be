using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectTaskRepository(ConsiliumTempusDbContext dbContext) : IProjectTaskRepository
{
    public Task<ProjectTaskAggregate?> Get(ProjectTaskId id, CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<List<ProjectTaskAggregate>> GetListByStage(
        ProjectStageId stageId, 
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Where(t => t.Stage.Id == stageId)
            .ApplyFilters(filters)
            .ToListAsync(cancellationToken);
    }
    
    public Task<int> GetListByStageCount(
        ProjectStageId stageId, 
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Include(t => t.CreatedBy)
            .Where(t => t.Stage.Id == stageId)
            .ApplyFilters(filters)
            .CountAsync(cancellationToken);
    }

    public async Task Add(ProjectTaskAggregate task, CancellationToken cancellationToken = default)
    {
        await dbContext.ProjectTasks.AddAsync(task, cancellationToken);
    }

    public void Remove(ProjectTaskAggregate task)
    {
        dbContext.ProjectTasks.Remove(task);
    }
}
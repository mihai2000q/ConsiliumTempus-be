using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
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

    public Task<ProjectTaskAggregate?> GetWithStageAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Include(t => t.Stage)
            .ThenInclude(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<ProjectStage?> GetStageWithTasksAndWorkspace(
        ProjectStageId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Set<ProjectStage>()
            .Include(s => s.Sprint.Project.Workspace)
            .Include(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<List<ProjectTaskAggregate>> GetListByStage(
        ProjectStageId stageId,
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        IReadOnlyList<IOrder<ProjectTaskAggregate>> orders,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Where(t => t.Stage.Id == stageId)
            .ApplyFilters(filters)
            .ApplyOrders(orders)
            .OrderByIf(orders.Count == 0, t => t.CustomOrderPosition.Value)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetListByStageCount(
        ProjectStageId stageId,
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Where(t => t.Stage.Id == stageId)
            .ApplyFilters(filters)
            .CountAsync(cancellationToken);
    }
}
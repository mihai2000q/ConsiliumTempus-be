using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
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
    public async Task<ProjectTaskAggregate?> GetWithWorkspace(ProjectTaskId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ProjectTaskAggregate?> GetWithSprint(
        ProjectTaskId id,
        bool isTracking = true,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectTasks
            .AsTrackingOrNot(isTracking)
            .Include(t => t.Stage.Sprint)
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<ProjectTaskAggregate?> GetWithStagesAndWorkspace(ProjectTaskId id, CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .Include(t => t.Stage.Sprint.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<ProjectTaskAggregate?> GetWithTasks(
        ProjectTaskId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Include(t => t.Stage)
            .ThenInclude(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    // TODO: Potentially remove method by adding the project sprint Id too, so that it can be queried from DB instead
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
        PaginationInfo? paginationInfo,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Where(t => t.Stage.Id == stageId)
            .ApplyFilters(filters)
            .ApplyOrders(orders)
            .OrderByIf(orders.Count == 0, t => t.CustomOrderPosition.Value)
            .Paginate(paginationInfo)
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
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectTaskRepository(ConsiliumTempusDbContext dbContext) : IProjectTaskRepository
{
    public async Task<ProjectTaskAggregate?> GetWithWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<ProjectTaskAggregate?> GetWithStagesAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .Include(t => t.Stage.Sprint.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<ProjectTaskAggregate?> GetWithTasksAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .Include(t => t.Stage.Sprint.Stages)
            .ThenInclude(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
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
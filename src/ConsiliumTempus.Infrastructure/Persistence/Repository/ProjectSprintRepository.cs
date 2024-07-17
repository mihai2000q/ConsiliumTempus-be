using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectSprintRepository(ConsiliumTempusDbContext dbContext) : IProjectSprintRepository
{
    public async Task<ProjectSprintAggregate?> Get(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectSprints.FindAsync([id], cancellationToken);
    }

    public Task<ProjectSprintAggregate?> GetWithWorkspace(
        ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectSprints
            .Include(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Include(ps => ps.Project.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public Task<ProjectSprintAggregate?> GetWithSprintsAndWorkspace(
        ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectSprints
            .Include(ps => ps.Project.Sprints)
            .Include(ps => ps.Project.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public Task<ProjectSprintAggregate?> GetWithTasksAndWorkspace(
        ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectSprints
            .Include(ps => ps.Project.Workspace)
            .Include(ps => ps.Stages)
            .ThenInclude(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public Task<ProjectSprintAggregate> GetFirstByProject(
        ProjectId projectId,
        IReadOnlyList<IFilter<ProjectSprintAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProjectSprints
            .Where(ps => ps.Project.Id == projectId)
            .ApplyFilters(filters)
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.EndDate)
            .ThenByDescending(s => s.Name.Value)
            .ThenByDescending(s => s.Audit.CreatedDateTime)
            .FirstAsync(cancellationToken);
    }

    public Task<List<ProjectSprintAggregate>> GetListByProject(
        ProjectId projectId,
        IReadOnlyList<IFilter<ProjectSprintAggregate>> filters,
        bool fromThisYear,
        CancellationToken cancellationToken = default)
    {
        var date = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return dbContext.ProjectSprints
            .Where(ps => ps.Project.Id == projectId)
            .WhereIf(fromThisYear, ps =>
                ps.StartDate >= DateOnly.FromDateTime(date) ||
                ps.EndDate >= DateOnly.FromDateTime(date) ||
                ps.Audit.CreatedDateTime >= date)
            .ApplyFilters(filters)
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.EndDate)
            .ThenByDescending(s => s.Name.Value)
            .ThenByDescending(s => s.Audit.CreatedDateTime)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetListByProjectCount(
        ProjectId projectId,
        IReadOnlyList<IFilter<ProjectSprintAggregate>> filters,
        bool fromThisYear,
        CancellationToken cancellationToken = default)
    {
        var date = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return dbContext.ProjectSprints
            .Where(ps => ps.Project.Id == projectId)
            .WhereIf(fromThisYear, ps =>
                ps.StartDate >= DateOnly.FromDateTime(date) ||
                ps.EndDate >= DateOnly.FromDateTime(date) ||
                ps.Audit.CreatedDateTime >= date)
            .ApplyFilters(filters)
            .CountAsync(cancellationToken);
    }

    public Task<List<ProjectStage>> GetStages(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<ProjectStage>()
            .Where(ps => ps.Sprint.Id == id)
            .OrderBy(ps => ps.CustomOrderPosition.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task Add(ProjectSprintAggregate sprint, CancellationToken cancellationToken = default)
    {
        await dbContext.ProjectSprints.AddAsync(sprint, cancellationToken);
    }

    public void Remove(ProjectSprintAggregate sprint)
    {
        dbContext.ProjectSprints.Remove(sprint);
        dbContext.Set<Audit>().Remove(sprint.Audit);
    }
}
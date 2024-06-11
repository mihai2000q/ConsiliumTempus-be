using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class ProjectRepository(ConsiliumTempusDbContext dbContext) : IProjectRepository
{
    public async Task<ProjectAggregate?> Get(ProjectId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .Include(p => p.Favorites)
            .Include(p => p.Statuses.OrderByDescending(s => s.Audit.CreatedDateTime))
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ProjectAggregate?> GetWithWorkspace(ProjectId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .Include(p => p.Favorites)
            .Include(p => p.Statuses)
            .Include(p => p.Workspace)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ProjectAggregate?> GetWithStagesAndWorkspace(ProjectId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .Include(p => p.Workspace)
            .Include(p => p.Sprints
                .OrderByDescending(s => s.StartDate)
                .ThenByDescending(s => s.EndDate)
                .ThenByDescending(s => s.Name.Value)
                .ThenByDescending(s => s.Audit.CreatedDateTime))
            .ThenInclude(ps => ps.Stages)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<List<ProjectAggregate>> GetListByUser(
        UserId userId,
        WorkspaceId? workspaceId,
        PaginationInfo? paginationInfo,
        IReadOnlyList<IOrder<ProjectAggregate>> orders,
        IEnumerable<IFilter<ProjectAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Projects
            .Include(p => p.Favorites)
            .Include(p => p.Statuses.OrderByDescending(s => s.Audit.CreatedDateTime))
            .Where(p => p.Workspace.Memberships.Any(m => m.User.Id == userId))
            .WhereIf(workspaceId is not null, p => p.Workspace.Id == workspaceId!)
            .ApplyFilters(filters)
            .ApplyOrders(orders)
            .Paginate(paginationInfo)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetListByUserCount(
        UserId userId,
        WorkspaceId? workspaceId,
        IEnumerable<IFilter<ProjectAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Projects
            .IgnoreAutoIncludes()
            .Where(p => p.Workspace.Memberships.Any(m => m.User.Id == userId))
            .WhereIf(workspaceId is not null, p => p.Workspace.Id == workspaceId!)
            .ApplyFilters(filters)
            .CountAsync(cancellationToken);
    }

    public async Task<List<ProjectStatus>> GetStatuses(ProjectId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<ProjectStatus>()
            .Where(ps => ps.Project.Id == id)
            .OrderByDescending(ps => ps.Audit.CreatedDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetStatusesCount(ProjectId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<ProjectStatus>()
            .IgnoreAutoIncludes()
            .Where(ps => ps.Project.Id == id)
            .CountAsync(cancellationToken);
    }

    public async Task Add(ProjectAggregate project, CancellationToken cancellationToken = default)
    {
        await dbContext.Projects.AddAsync(project, cancellationToken);
    }

    public void Remove(ProjectAggregate project)
    {
        dbContext.Projects.Remove(project);
    }
}
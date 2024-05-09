using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class WorkspaceRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRepository, IWorkspaceProvider
{
    public async Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces.FindAsync([id], cancellationToken);
    }

    public async Task<WorkspaceAggregate?> GetByProject(ProjectId id, CancellationToken cancellationToken = default)
    {
        var project = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

        return project?.Workspace;
    }

    public async Task<WorkspaceAggregate?> GetByProjectSprint(ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        var projectSprint = await dbContext.ProjectSprints
            .Include(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return projectSprint?.Project.Workspace;
    }
    
    public async Task<WorkspaceAggregate?> GetByProjectStage(ProjectStageId id,
        CancellationToken cancellationToken = default)
    {
        var projectStage = await dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return projectStage?.Sprint.Project.Workspace;
    }

    public async Task<List<WorkspaceAggregate>> GetListByUser(
        UserAggregate user,
        PaginationInfo? paginationInfo,
        IOrder<WorkspaceAggregate>? order,
        IEnumerable<IFilter<WorkspaceAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Where(w => w.Memberships.Any(m => m.User == user))
            .ApplyFilters(filters)
            .ApplyOrder(order)
            .Paginate(paginationInfo)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetListByUserCount(
        UserAggregate user,
        IEnumerable<IFilter<WorkspaceAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Where(w => w.Memberships.Any(m => m.User == user))
            .ApplyFilters(filters)
            .CountAsync(cancellationToken);
    }

    public async Task<List<WorkspaceAggregate>> GetListByUserWithMemberships(UserAggregate user,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Memberships)
            .ThenInclude(m => m.User)
            .Include(w => w.Memberships)
            .Where(w => w.Memberships.Any(m => m.User == user))
            .ToListAsync(cancellationToken);
    }

    public async Task Add(WorkspaceAggregate workspaceAggregate, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(workspaceAggregate, cancellationToken);
    }

    public void Remove(WorkspaceAggregate workspace)
    {
        dbContext.Workspaces.Remove(workspace);
    }
}
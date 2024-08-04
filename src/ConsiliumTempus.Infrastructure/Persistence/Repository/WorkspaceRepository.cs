using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class WorkspaceRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRepository
{
    public async Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Favorites)
            .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<WorkspaceAggregate?> GetWithCollaborators(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Memberships)
            .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<WorkspaceAggregate?> GetWithInvitations(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Invitations)
            .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
    }
    
    public async Task<WorkspaceAggregate?> GetWithCollaboratorsAndInvitations(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Invitations)
            .Include(w => w.Memberships)
            .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<List<WorkspaceAggregate>> GetListByUser(
        UserAggregate user,
        PaginationInfo? paginationInfo,
        IReadOnlyList<IOrder<WorkspaceAggregate>> orders,
        IEnumerable<IFilter<WorkspaceAggregate>> filters,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Favorites)
            .Where(w => w.Memberships.Any(m => m.User == user))
            .ApplyFilters(filters)
            .ApplyOrders(orders)
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

    public async Task<List<WorkspaceAggregate>> GetListByUserWithCollaborators(UserAggregate user,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Memberships)
            .Where(w => w.Memberships.Any(m => m.User == user))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Membership>> GetCollaborators(
        WorkspaceId id,
        string? searchValue,
        IReadOnlyList<IFilter<Membership>> filters,
        IReadOnlyList<IOrder<Membership>> orders,
        PaginationInfo? paginationInfo,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Membership>()
            .Include(m => m.User)
            .Where(m => m.Workspace.Id == id)
            .WhereIf(!string.IsNullOrWhiteSpace(searchValue),
                m =>
                    m.User.Credentials.Email.Contains(searchValue!) ||
                    (m.User.FirstName.Value + " " + m.User.LastName.Value).Contains(searchValue!))
            .ApplyFilters(filters)
            .ApplyOrders(orders)
            .Paginate(paginationInfo)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCollaboratorsCount(
        WorkspaceId id,
        string? searchValue,
        IReadOnlyList<IFilter<Membership>> filters,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Membership>()
            .Include(m => m.User)
            .Where(m => m.Workspace.Id == id)
            .WhereIf(!string.IsNullOrWhiteSpace(searchValue),
                m =>
                    m.User.Credentials.Email.Contains(searchValue!) ||
                    (m.User.FirstName.Value + " " + m.User.LastName.Value).Contains(searchValue!))
            .CountAsync(cancellationToken);
    }

    public async Task<List<WorkspaceInvitation>> GetInvitations(
        UserAggregate? user,
        bool? isSender,
        WorkspaceId? workspaceId,
        PaginationInfo? paginationInfo,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<WorkspaceInvitation>()
            .Include(i => i.Workspace)
            .WhereIf(isSender is not null && isSender.Value, i => i.Sender == user)
            .WhereIf(isSender is not null && !isSender.Value, i => i.Collaborator == user)
            .WhereIf(workspaceId is not null, i => i.Workspace.Id == workspaceId)
            .OrderByDescending(i => i.CreatedDateTime)
            .Paginate(paginationInfo)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetInvitationsCount(
        UserAggregate? user,
        bool? isSender,
        WorkspaceId? workspaceId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<WorkspaceInvitation>()
            .WhereIf(isSender is not null && isSender.Value, i => i.Sender == user)
            .WhereIf(isSender is not null && !isSender.Value, i => i.Collaborator == user)
            .WhereIf(workspaceId is not null, i => i.Workspace.Id == workspaceId)
            .CountAsync(cancellationToken);
    }

    public async Task Add(WorkspaceAggregate workspaceAggregate, CancellationToken cancellationToken = default)
    {
        await dbContext.Workspaces.AddAsync(workspaceAggregate, cancellationToken);
    }

    public void Remove(WorkspaceAggregate workspace)
    {
        dbContext.Workspaces.Remove(workspace);
    }
}
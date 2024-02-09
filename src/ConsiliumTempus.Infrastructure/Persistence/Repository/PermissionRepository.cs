using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class PermissionRepository(ConsiliumTempusDbContext dbContext) : IPermissionProvider
{
    public async Task<HashSet<string>> GetPermissions(UserId userId, WorkspaceId workspaceId)
    { 
        var membership = await dbContext.Set<Membership>()
            .Include(m => m.WorkspaceRole)
            .ThenInclude(wr => wr.Permissions)
            .SingleAsync(u => u.User.Id == userId && u.Workspace.Id == workspaceId);

        return membership
            .WorkspaceRole
            .Permissions
            .Select(x => x.Name)
            .ToHashSet();
    }
}
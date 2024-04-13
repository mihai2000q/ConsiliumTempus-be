using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class PermissionRepository(ConsiliumTempusDbContext dbContext) : IPermissionProvider
{
    public async Task<HashSet<string>> GetPermissions(UserId userId, WorkspaceId workspaceId)
    {
        var membership = await dbContext.Set<Membership>()
            .SingleOrDefaultAsync(u => u.User.Id == userId && u.Workspace.Id == workspaceId);

        if (membership is null) return [];

        return dbContext.Set<WorkspaceRole>()
            .Include(wr => wr.Permissions)
            .Single(wr => wr == membership.WorkspaceRole)
            .Permissions
            .Select(x => x.Name)
            .ToHashSet();
    }
}
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

public sealed class PermissionProvider(ConsiliumTempusDbContext dbContext) : IPermissionProvider
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
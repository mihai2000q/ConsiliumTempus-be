using ConsiliumTempus.Domain.Common.Relations;
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
        var userToWorkspace = await dbContext.Set<UserToWorkspace>()
            .SingleAsync(u => u.User.Id == userId && u.Workspace.Id == workspaceId);

        return userToWorkspace
            .Role
            .Permissions
            .Select(x => x.Name)
            .ToHashSet();
    }
}
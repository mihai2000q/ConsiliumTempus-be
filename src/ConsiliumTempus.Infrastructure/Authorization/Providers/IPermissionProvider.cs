using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Infrastructure.Authorization.Providers;

public interface IPermissionProvider
{
    Task<HashSet<string>> GetPermissions(UserId userId, WorkspaceId workspaceId);
}
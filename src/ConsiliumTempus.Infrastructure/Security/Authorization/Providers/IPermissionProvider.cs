using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

internal interface IPermissionProvider
{
    /// <summary>
    /// The permissions of the user inside the workspace, or null when the user is not a collaborator of it
    /// </summary>
    Task<HashSet<string>?> GetPermissions(UserId userId, WorkspaceId workspaceId);
}
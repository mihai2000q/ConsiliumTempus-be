using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.Authorization;

public static class AccessControlList
{
    private static readonly List<Permissions> ViewPermissions = [
        Permissions.ReadWorkspace
    ];
    private static readonly List<Permissions> MemberPermissions = [
        Permissions.ReadWorkspace, Permissions.UpdateWorkspace
    ];
    private static readonly List<Permissions> AdminPermissions = [
        Permissions.ReadWorkspace, Permissions.UpdateWorkspace, Permissions.DeleteWorkspace
    ];
    
    public static readonly Dictionary<WorkspaceRole, List<Permissions>> RoleHasPermissions = new()
    {
        { WorkspaceRole.View, ViewPermissions },
        { WorkspaceRole.Member, MemberPermissions },
        { WorkspaceRole.Admin, AdminPermissions },
    };
}
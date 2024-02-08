using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Domain.Common.Relations;

public class WorkspaceRoleHasPermission
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceRoleHasPermission()
    {
    }
    
    private WorkspaceRoleHasPermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
    
    public int RoleId { get; init; }
    public int PermissionId { get; init; }

    public static WorkspaceRoleHasPermission Create(int roleId, int permissionId)
    {
        return new WorkspaceRoleHasPermission(roleId, permissionId);
    }
}
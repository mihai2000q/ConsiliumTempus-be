using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class WorkspaceRoleHasPermission
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceRoleHasPermission()
    {
    }
    
    private WorkspaceRoleHasPermission(int workspaceRoleId, int permissionId)
    {
        WorkspaceRoleId = workspaceRoleId;
        PermissionId = permissionId;
    }
    
    public int WorkspaceRoleId { get; init; }
    public int PermissionId { get; init; }

    public static WorkspaceRoleHasPermission Create(int roleId, int permissionId)
    {
        return new WorkspaceRoleHasPermission(roleId, permissionId);
    }
}
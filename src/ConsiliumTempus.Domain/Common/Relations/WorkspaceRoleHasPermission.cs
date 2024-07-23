using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class WorkspaceRoleHasPermission : Entity<(int, int)>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceRoleHasPermission()
    {
    }

    private WorkspaceRoleHasPermission(int workspaceRoleId, int permissionId) : base((workspaceRoleId, permissionId))
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

    private static readonly List<Permissions> ViewPermissions =
    [
        // Workspace
        Permissions.ReadWorkspace, Permissions.ReadOverviewWorkspace, Permissions.ReadCollaboratorsFromWorkspace, 
        Permissions.UpdateFavoritesWorkspace,
        // Project
        Permissions.ReadProject, Permissions.ReadOverviewProject, Permissions.ReadCollectionProject,
        Permissions.UpdateFavoritesProject,
        // Project - Project Status
        Permissions.ReadStatusesFromProject,
        // Project Sprint
        Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint,
        // Project Sprint - Project Stage
        Permissions.ReadStagesFromProjectSprint,
        // Project Task
        Permissions.ReadProjectTask, Permissions.ReadCollectionProjectTask
    ];

    private static readonly List<Permissions> MemberPermissions =
    [
        // Workspace
        Permissions.ReadWorkspace, Permissions.ReadOverviewWorkspace, Permissions.ReadCollaboratorsFromWorkspace, 
        Permissions.UpdateWorkspace, Permissions.UpdateFavoritesWorkspace, Permissions.UpdateOverviewWorkspace,
        // Project
        Permissions.ReadProject, Permissions.ReadOverviewProject, Permissions.ReadCollectionProject,
        Permissions.UpdateProject, Permissions.UpdateFavoritesProject, Permissions.UpdateOverviewProject,
        // Project - Project Status
        Permissions.ReadStatusesFromProject, Permissions.UpdateStatusFromProject,
        // Project Sprint
        Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint, Permissions.UpdateProjectSprint,
        // Project Sprint - Project Stage
        Permissions.ReadStagesFromProjectSprint, Permissions.UpdateStageFromProjectSprint,
        // Project Task
        Permissions.CreateProjectTask, Permissions.ReadProjectTask, Permissions.ReadCollectionProjectTask,
        Permissions.MoveProjectTask, 
        Permissions.UpdateProjectTask, Permissions.UpdateIsCompletedProjectTask, Permissions.UpdateOverviewProjectTask,
        Permissions.DeleteProjectTask,
    ];

    private static readonly List<Permissions> AdminPermissions =
    [
        // Workspace
        Permissions.ReadWorkspace, Permissions.ReadOverviewWorkspace, Permissions.ReadCollaboratorsFromWorkspace, 
        Permissions.ReadInvitationsFromWorkspace, Permissions.InviteCollaboratorToWorkspace,
        Permissions.UpdateWorkspace, Permissions.UpdateFavoritesWorkspace, Permissions.UpdateOverviewWorkspace,
        Permissions.DeleteWorkspace,
        // Project
        Permissions.CreateProject, Permissions.ReadProject, Permissions.ReadOverviewProject, Permissions.ReadCollectionProject,
        Permissions.UpdateProject, Permissions.UpdateFavoritesProject, Permissions.UpdateOverviewProject, 
        Permissions.DeleteProject,
        // Project - Project Status
        Permissions.AddStatusToProject, Permissions.ReadStatusesFromProject, Permissions.RemoveStatusFromProject,
        Permissions.UpdateStatusFromProject,
        // Project Sprint
        Permissions.CreateProjectSprint, Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint,
        Permissions.UpdateProjectSprint, Permissions.DeleteProjectSprint,
        // Project Sprint - Project Stage
        Permissions.AddStageToProjectSprint, Permissions.ReadStagesFromProjectSprint, 
        Permissions.MoveStageFromProjectSprint, Permissions.UpdateStageFromProjectSprint,
        Permissions.RemoveStageFromProjectSprint,
        // Project Task
        Permissions.CreateProjectTask, Permissions.ReadProjectTask, Permissions.ReadCollectionProjectTask,
        Permissions.MoveProjectTask, 
        Permissions.UpdateProjectTask, Permissions.UpdateIsCompletedProjectTask, Permissions.UpdateOverviewProjectTask,
        Permissions.DeleteProjectTask,
    ];

    public static readonly Dictionary<WorkspaceRole, List<Permissions>> DefaultData = new()
    {
        { WorkspaceRole.View, ViewPermissions },
        { WorkspaceRole.Member, MemberPermissions },
        { WorkspaceRole.Admin, AdminPermissions },
    };
}
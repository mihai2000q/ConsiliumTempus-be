using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.Security.Authorization;

public static class AccessControlList
{
    private static readonly List<Permissions> ViewPermissions =
    [
        Permissions.ReadWorkspace,
        Permissions.ReadProject, Permissions.ReadCollectionProject,
        Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint,
        Permissions.ReadCollectionProjectStage,
        Permissions.ReadProjectTask, Permissions.ReadCollectionProjectTask
    ];

    private static readonly List<Permissions> MemberPermissions =
    [
        Permissions.ReadWorkspace, Permissions.UpdateWorkspace,
        Permissions.ReadProject, Permissions.ReadCollectionProject, Permissions.UpdateProject,
        Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint, Permissions.UpdateProjectSprint,
        Permissions.ReadCollectionProjectStage, Permissions.UpdateProjectStage,
        Permissions.CreateProjectTask, Permissions.ReadProjectTask, Permissions.ReadCollectionProjectTask, Permissions.UpdateProjectTask, Permissions.DeleteProjectTask,
    ];

    private static readonly List<Permissions> AdminPermissions =
    [
        Permissions.ReadWorkspace, Permissions.UpdateWorkspace, Permissions.DeleteWorkspace,
        Permissions.CreateProject, Permissions.ReadProject, Permissions.ReadCollectionProject, Permissions.UpdateProject, Permissions.DeleteProject,
        Permissions.CreateProjectSprint, Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint, Permissions.UpdateProjectSprint, Permissions.DeleteProjectSprint,
        Permissions.CreateProjectStage, Permissions.ReadCollectionProjectStage, Permissions.UpdateProjectStage, Permissions.DeleteProjectStage,
        Permissions.CreateProjectTask, Permissions.ReadProjectTask, Permissions.ReadCollectionProjectTask, Permissions.UpdateProjectTask, Permissions.DeleteProjectTask,
    ];

    public static readonly Dictionary<WorkspaceRole, List<Permissions>> RoleHasPermissions = new()
    {
        { WorkspaceRole.View, ViewPermissions },
        { WorkspaceRole.Member, MemberPermissions },
        { WorkspaceRole.Admin, AdminPermissions },
    };
}
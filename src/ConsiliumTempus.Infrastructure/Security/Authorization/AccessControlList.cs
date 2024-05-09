using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.Security.Authorization;

public static class AccessControlList
{
    private static readonly List<Permissions> ViewPermissions =
    [
        Permissions.ReadWorkspace,
        Permissions.ReadProject, Permissions.ReadCollectionProject,
        Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint
    ];

    private static readonly List<Permissions> MemberPermissions =
    [
        Permissions.ReadWorkspace, Permissions.UpdateWorkspace,
        Permissions.ReadProject, Permissions.ReadCollectionProject, Permissions.UpdateProject,
        Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint, Permissions.UpdateProjectSprint,
        Permissions.UpdateProjectStage
    ];

    private static readonly List<Permissions> AdminPermissions =
    [
        Permissions.ReadWorkspace, Permissions.UpdateWorkspace, Permissions.DeleteWorkspace,
        Permissions.CreateProject, Permissions.ReadProject, Permissions.ReadCollectionProject, Permissions.UpdateProject, Permissions.DeleteProject,
        Permissions.CreateProjectSprint, Permissions.ReadProjectSprint, Permissions.ReadCollectionProjectSprint, Permissions.UpdateProjectSprint, Permissions.DeleteProjectSprint,
        Permissions.CreateProjectStage, Permissions.UpdateProjectStage, Permissions.DeleteProjectStage
    ];

    public static readonly Dictionary<WorkspaceRole, List<Permissions>> RoleHasPermissions = new()
    {
        { WorkspaceRole.View, ViewPermissions },
        { WorkspaceRole.Member, MemberPermissions },
        { WorkspaceRole.Admin, AdminPermissions },
    };
}
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class PermissionAuthorizationHandlerData
{
    internal class GetPermissionWithRouteValues : TheoryData<Permissions>
    {
        public GetPermissionWithRouteValues()
        {
            Add(Permissions.ReadWorkspace);
            Add(Permissions.ReadProject);
            Add(Permissions.DeleteWorkspace);
            Add(Permissions.DeleteProject);
            Add(Permissions.DeleteProjectSprint);
        }
    }
    
    internal class GetPermissionWithBody : TheoryData<Permissions>
    {
        public GetPermissionWithBody()
        {
            Add(Permissions.CreateProject);
            Add(Permissions.CreateProjectSprint);
            Add(Permissions.UpdateWorkspace);
            Add(Permissions.UpdateProject);
            Add(Permissions.UpdateProjectSprint);
        }
    }
    
    internal class GetPermissionWithBodyAndId : TheoryData<Permissions, string>
    {
        public GetPermissionWithBodyAndId()
        {
            Add(Permissions.CreateProject, "workspaceId");
            Add(Permissions.CreateProjectSprint, "id");
            Add(Permissions.UpdateWorkspace, "id");
            Add(Permissions.UpdateProject, "id");
            Add(Permissions.UpdateProjectSprint, "projectId");
        }
    }
    
    internal class GetPermissionWithWorkspaceId : TheoryData<Permissions, string?, bool, StringIdType>
    {
        public GetPermissionWithWorkspaceId()
        {
            Add(Permissions.CreateProject, "workspaceId", true, StringIdType.Workspace);
            Add(Permissions.CreateProjectSprint, "projectId", true, StringIdType.Project);
            Add(Permissions.ReadWorkspace, null, false, StringIdType.Workspace);
            Add(Permissions.UpdateWorkspace, null, true, StringIdType.Workspace);
            Add(Permissions.ReadProject, null, false, StringIdType.Project);
            Add(Permissions.UpdateProject, null, true, StringIdType.Project);
            Add(Permissions.UpdateProjectSprint, null, true, StringIdType.ProjectSprint);
            Add(Permissions.DeleteWorkspace, null, false, StringIdType.Workspace);
            Add(Permissions.DeleteProject, null, false, StringIdType.Project);
            Add(Permissions.DeleteProjectSprint, null, false, StringIdType.ProjectSprint);
        }
    }

    public enum StringIdType
    {
        Workspace, 
        Project, 
        ProjectSprint
    }
}
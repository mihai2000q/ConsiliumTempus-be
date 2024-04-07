using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class PermissionAuthorizationHandlerData
{
    internal class GetPermissions : TheoryData<Permissions, RequestLocation>
    {
        public GetPermissions()
        {
            Add(Permissions.ReadWorkspace, RequestLocation.Route);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route);

            Add(Permissions.CreateProject, RequestLocation.Body);
            Add(Permissions.ReadProject, RequestLocation.Route);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query);
            Add(Permissions.UpdateProject, RequestLocation.Body);
            Add(Permissions.DeleteProject, RequestLocation.Route);

            Add(Permissions.CreateProjectSprint, RequestLocation.Body);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route);
        }
    }
    
    internal class GetPermissionsWithId : TheoryData<Permissions, RequestLocation, string?>
    {
        public GetPermissionsWithId()
        {
            Add(Permissions.ReadWorkspace, RequestLocation.Route, null);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body, null);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route, null);

            Add(Permissions.CreateProject, RequestLocation.Body, "workspaceId");
            Add(Permissions.ReadProject, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, "workspaceId");
            Add(Permissions.UpdateProject, RequestLocation.Body, null);
            Add(Permissions.DeleteProject, RequestLocation.Route, null);

            Add(Permissions.CreateProjectSprint, RequestLocation.Body, "projectId");
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null);
        }
    }
    
    internal class GetPermissionsWithIdAndType : TheoryData<Permissions, RequestLocation, string?, StringIdType>
    {
        public GetPermissionsWithIdAndType()
        {
            Add(Permissions.ReadWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route, null, StringIdType.Workspace);

            Add(Permissions.CreateProject, RequestLocation.Body, "workspaceId", StringIdType.Workspace);
            Add(Permissions.ReadProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, "workspaceId", StringIdType.Workspace);
            Add(Permissions.UpdateProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.DeleteProject, RequestLocation.Route, null, StringIdType.Project);

            Add(Permissions.CreateProjectSprint, RequestLocation.Body, "projectId", StringIdType.Project);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);
        }
    }

    public enum StringIdType
    {
        Workspace,
        Project,
        ProjectSprint
    }

    public enum RequestLocation
    {
        Route,
        Query,
        Body
    }
}
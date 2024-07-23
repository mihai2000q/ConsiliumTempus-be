using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class PermissionAuthorizationHandlerData
{
    public enum RequestLocation
    {
        Route,
        Query,
        Body
    }

    public enum StringIdType
    {
        Workspace,
        Project,
        ProjectSprint,
        ProjectStage,
        ProjectTask
    }

    internal class GetPermissions : TheoryData<Permissions, RequestLocation>
    {
        public GetPermissions()
        {
            Add(Permissions.ReadWorkspace, RequestLocation.Route);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route);

            Add(Permissions.CreateProject, RequestLocation.Body);
            Add(Permissions.ReadProject, RequestLocation.Route);
            Add(Permissions.ReadOverviewProject, RequestLocation.Route);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query);
            Add(Permissions.UpdateProject, RequestLocation.Body);
            Add(Permissions.UpdateFavoritesProject, RequestLocation.Body);
            Add(Permissions.UpdateOverviewProject, RequestLocation.Body);
            Add(Permissions.DeleteProject, RequestLocation.Route);
            
            Add(Permissions.AddStatusToProject, RequestLocation.Body);
            Add(Permissions.ReadStatusesFromProject, RequestLocation.Route);
            Add(Permissions.UpdateProject, RequestLocation.Body);
            Add(Permissions.DeleteProject, RequestLocation.Route);

            Add(Permissions.CreateProjectSprint, RequestLocation.Body);
            Add(Permissions.ReadProjectSprint, RequestLocation.Route);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route);

            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route);

            Add(Permissions.CreateProjectTask, RequestLocation.Body);
            Add(Permissions.ReadProjectTask, RequestLocation.Route);
            Add(Permissions.ReadCollectionProjectTask, RequestLocation.Query);
            Add(Permissions.MoveProjectTask, RequestLocation.Body);
            Add(Permissions.UpdateProjectTask, RequestLocation.Body);
            Add(Permissions.UpdateIsCompletedProjectTask, RequestLocation.Body);
            Add(Permissions.UpdateOverviewProjectTask, RequestLocation.Body);
            Add(Permissions.DeleteProjectTask, RequestLocation.Route);
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
            Add(Permissions.ReadOverviewProject, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, "workspaceId");
            Add(Permissions.UpdateProject, RequestLocation.Body, null);
            Add(Permissions.UpdateFavoritesProject, RequestLocation.Body, null);
            Add(Permissions.UpdateOverviewProject, RequestLocation.Body, null);
            Add(Permissions.DeleteProject, RequestLocation.Route, null);

            Add(Permissions.CreateProjectSprint, RequestLocation.Body, "projectId");
            Add(Permissions.ReadProjectSprint, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query, "projectId");
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null);

            Add(Permissions.AddStatusToProject, RequestLocation.Body, null);
            Add(Permissions.ReadStatusesFromProject, RequestLocation.Route, null);
            Add(Permissions.UpdateProject, RequestLocation.Body, null);
            Add(Permissions.DeleteProject, RequestLocation.Route, null);            
            
            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body, null);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body, null);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route, null);

            Add(Permissions.CreateProjectTask, RequestLocation.Body, "projectStageId");
            Add(Permissions.ReadProjectTask, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProjectTask, RequestLocation.Query, "projectStageId");
            Add(Permissions.MoveProjectTask, RequestLocation.Body, null);
            Add(Permissions.UpdateProjectTask, RequestLocation.Body, null);
            Add(Permissions.UpdateIsCompletedProjectTask, RequestLocation.Body, null);
            Add(Permissions.UpdateOverviewProjectTask, RequestLocation.Body, null);
            Add(Permissions.DeleteProjectTask, RequestLocation.Route, null);
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
            Add(Permissions.ReadOverviewProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, "workspaceId", StringIdType.Workspace);
            Add(Permissions.UpdateProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.UpdateFavoritesProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.UpdateOverviewProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.DeleteProject, RequestLocation.Route, null, StringIdType.Project);
            
            Add(Permissions.AddStatusToProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.ReadStatusesFromProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.UpdateProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.DeleteProject, RequestLocation.Route, null, StringIdType.Project);   

            Add(Permissions.CreateProjectSprint, RequestLocation.Body, "projectId", StringIdType.Project);
            Add(Permissions.ReadProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query, "projectId", StringIdType.Project);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);

            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);

            Add(Permissions.CreateProjectTask, RequestLocation.Body, "projectStageId", StringIdType.ProjectStage);
            Add(Permissions.ReadProjectTask, RequestLocation.Route, null, StringIdType.ProjectTask);
            Add(Permissions.ReadCollectionProjectTask, RequestLocation.Query, "projectStageId", StringIdType.ProjectStage);
            Add(Permissions.MoveProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateIsCompletedProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateOverviewProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.DeleteProjectTask, RequestLocation.Route, null, StringIdType.ProjectTask);
        }
    }
}
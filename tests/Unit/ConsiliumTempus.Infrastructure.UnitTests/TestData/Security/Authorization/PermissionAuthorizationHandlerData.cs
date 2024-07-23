using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.Workspace;

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
            // Workspace
            Add(Permissions.ReadWorkspace, RequestLocation.Route);
            Add(Permissions.ReadOverviewWorkspace, RequestLocation.Route);
            Add(Permissions.ReadCollaboratorsFromWorkspace, RequestLocation.Route);
            Add(Permissions.ReadInvitationsFromWorkspace, RequestLocation.Route);
            Add(Permissions.InviteCollaboratorToWorkspace, RequestLocation.Body);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body);
            Add(Permissions.UpdateFavoritesWorkspace, RequestLocation.Body);
            Add(Permissions.UpdateOverviewWorkspace, RequestLocation.Body);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route);

            // Project
            Add(Permissions.CreateProject, RequestLocation.Body);
            Add(Permissions.ReadProject, RequestLocation.Route);
            Add(Permissions.ReadOverviewProject, RequestLocation.Route);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query);
            Add(Permissions.UpdateProject, RequestLocation.Body);
            Add(Permissions.UpdateFavoritesProject, RequestLocation.Body);
            Add(Permissions.UpdateOverviewProject, RequestLocation.Body);
            Add(Permissions.DeleteProject, RequestLocation.Route);

            // Project - Project Status
            Add(Permissions.AddStatusToProject, RequestLocation.Body);
            Add(Permissions.ReadStatusesFromProject, RequestLocation.Route);
            Add(Permissions.UpdateStatusFromProject, RequestLocation.Body);
            Add(Permissions.RemoveStatusFromProject, RequestLocation.Route);

            // Project Sprint
            Add(Permissions.CreateProjectSprint, RequestLocation.Body);
            Add(Permissions.ReadProjectSprint, RequestLocation.Route);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route);

            // Project Sprint - Project Stage
            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body);
            Add(Permissions.ReadStagesFromProjectSprint, RequestLocation.Route);
            Add(Permissions.MoveStageFromProjectSprint, RequestLocation.Body);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route);

            // Project Task
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
            // Workspace
            Add(Permissions.ReadWorkspace, RequestLocation.Route, null);
            Add(Permissions.ReadOverviewWorkspace, RequestLocation.Route, null);
            Add(Permissions.ReadCollaboratorsFromWorkspace, RequestLocation.Route, null);
            Add(Permissions.ReadInvitationsFromWorkspace, RequestLocation.Route, ToIdProperty<WorkspaceAggregate>());
            Add(Permissions.InviteCollaboratorToWorkspace, RequestLocation.Body, null);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body, null);
            Add(Permissions.UpdateFavoritesWorkspace, RequestLocation.Body, null);
            Add(Permissions.UpdateOverviewWorkspace, RequestLocation.Body, null);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route, null);

            // Project
            Add(Permissions.CreateProject, RequestLocation.Body, ToIdProperty<WorkspaceAggregate>());
            Add(Permissions.ReadProject, RequestLocation.Route, null);
            Add(Permissions.ReadOverviewProject, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, ToIdProperty<WorkspaceAggregate>());
            Add(Permissions.UpdateProject, RequestLocation.Body, null);
            Add(Permissions.UpdateFavoritesProject, RequestLocation.Body, null);
            Add(Permissions.UpdateOverviewProject, RequestLocation.Body, null);
            Add(Permissions.DeleteProject, RequestLocation.Route, null);

            // Project - Project Status
            Add(Permissions.AddStatusToProject, RequestLocation.Body, null);
            Add(Permissions.ReadStatusesFromProject, RequestLocation.Route, null);
            Add(Permissions.UpdateStatusFromProject, RequestLocation.Body, null);
            Add(Permissions.RemoveStatusFromProject, RequestLocation.Route, null);

            // Project Sprint
            Add(Permissions.CreateProjectSprint, RequestLocation.Body, ToIdProperty<ProjectAggregate>());
            Add(Permissions.ReadProjectSprint, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query, ToIdProperty<ProjectAggregate>());
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null);

            // Project Sprint - Project Stage
            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body, null);
            Add(Permissions.ReadStagesFromProjectSprint, RequestLocation.Route, null);
            Add(Permissions.MoveStageFromProjectSprint, RequestLocation.Body, null);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body, null);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route, null);

            // Project Task
            Add(Permissions.CreateProjectTask, RequestLocation.Body, ToIdProperty<ProjectStage>());
            Add(Permissions.ReadProjectTask, RequestLocation.Route, null);
            Add(Permissions.ReadCollectionProjectTask, RequestLocation.Query, ToIdProperty<ProjectStage>());
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
            // Workspace
            Add(Permissions.ReadWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.ReadOverviewWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.ReadCollaboratorsFromWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.ReadInvitationsFromWorkspace, RequestLocation.Route, ToIdProperty<WorkspaceAggregate>(), StringIdType.Workspace);
            Add(Permissions.InviteCollaboratorToWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.UpdateFavoritesWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.UpdateOverviewWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route, null, StringIdType.Workspace);

            // Project
            Add(Permissions.CreateProject, RequestLocation.Body, ToIdProperty<WorkspaceAggregate>(), StringIdType.Workspace);
            Add(Permissions.ReadProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.ReadOverviewProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, ToIdProperty<WorkspaceAggregate>(), StringIdType.Workspace);
            Add(Permissions.UpdateProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.UpdateFavoritesProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.UpdateOverviewProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.DeleteProject, RequestLocation.Route, null, StringIdType.Project);

            // Project - Project Status
            Add(Permissions.AddStatusToProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.ReadStatusesFromProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.UpdateStatusFromProject, RequestLocation.Body, null, StringIdType.Project);
            Add(Permissions.RemoveStatusFromProject, RequestLocation.Route, null, StringIdType.Project);

            // Project Sprint
            Add(Permissions.CreateProjectSprint, RequestLocation.Body, ToIdProperty<ProjectAggregate>(), StringIdType.Project);
            Add(Permissions.ReadProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query, ToIdProperty<ProjectAggregate>(), StringIdType.Project);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);

            // Project Sprint - Project Stage
            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.ReadStagesFromProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);
            Add(Permissions.MoveStageFromProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);

            // Project Task
            Add(Permissions.CreateProjectTask, RequestLocation.Body, ToIdProperty<ProjectStage>(), StringIdType.ProjectStage);
            Add(Permissions.ReadProjectTask, RequestLocation.Route, null, StringIdType.ProjectTask);
            Add(Permissions.ReadCollectionProjectTask, RequestLocation.Query, ToIdProperty<ProjectStage>(), StringIdType.ProjectStage);
            Add(Permissions.MoveProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateIsCompletedProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateOverviewProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.DeleteProjectTask, RequestLocation.Route, null, StringIdType.ProjectTask);
        }
    }
    
    private static string ToIdProperty<T>()
    {
        var property = typeof(T).Name.Replace("Aggregate", "");
        property = property[0].ToString().ToLower() + property[1..];
        return property + "Id";
    }
}
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;

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

    internal class GetPermissions : TheoryData<Permissions, RequestLocation, string?, StringIdType>
    {
        public GetPermissions()
        {
            // Workspace
            Add(Permissions.ReadWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.ReadOverviewWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.ReadCollaboratorsFromWorkspace, RequestLocation.Route, null, StringIdType.Workspace);
            Add(Permissions.ReadInvitationsFromWorkspace, RequestLocation.Query, typeof(WorkspaceAggregate).ToCamelId(), StringIdType.Workspace);
            Add(Permissions.InviteCollaboratorToWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.UpdateWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.UpdateFavoritesWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.UpdateOverviewWorkspace, RequestLocation.Body, null, StringIdType.Workspace);
            Add(Permissions.DeleteWorkspace, RequestLocation.Route, null, StringIdType.Workspace);

            // Project
            Add(Permissions.CreateProject, RequestLocation.Body, typeof(WorkspaceAggregate).ToCamelId(), StringIdType.Workspace);
            Add(Permissions.ReadProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.ReadOverviewProject, RequestLocation.Route, null, StringIdType.Project);
            Add(Permissions.ReadCollectionProject, RequestLocation.Query, typeof(WorkspaceAggregate).ToCamelId(), StringIdType.Workspace);
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
            Add(Permissions.CreateProjectSprint, RequestLocation.Body, typeof(ProjectAggregate).ToCamelId(), StringIdType.Project);
            Add(Permissions.ReadProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);
            Add(Permissions.ReadCollectionProjectSprint, RequestLocation.Query, typeof(ProjectAggregate).ToCamelId(), StringIdType.Project);
            Add(Permissions.UpdateProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.DeleteProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);

            // Project Sprint - Project Stage
            Add(Permissions.AddStageToProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.ReadStagesFromProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);
            Add(Permissions.MoveStageFromProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.UpdateStageFromProjectSprint, RequestLocation.Body, null, StringIdType.ProjectSprint);
            Add(Permissions.RemoveStageFromProjectSprint, RequestLocation.Route, null, StringIdType.ProjectSprint);

            // Project Task
            Add(Permissions.CreateProjectTask, RequestLocation.Body, typeof(ProjectStage).ToCamelId(), StringIdType.ProjectStage);
            Add(Permissions.ReadProjectTask, RequestLocation.Route, null, StringIdType.ProjectTask);
            Add(Permissions.ReadCollectionProjectTask, RequestLocation.Query, typeof(ProjectStage).ToCamelId(), StringIdType.ProjectStage);
            Add(Permissions.MoveProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateIsCompletedProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.UpdateOverviewProjectTask, RequestLocation.Body, null, StringIdType.ProjectTask);
            Add(Permissions.DeleteProjectTask, RequestLocation.Route, null, StringIdType.ProjectTask);
        }
    }
}
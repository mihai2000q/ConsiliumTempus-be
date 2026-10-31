using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Action = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Action;
using Controller = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Controller;
using Method = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Method;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class PermissionAuthorizationHandlerData
{
    /// <summary>
    /// The endpoint that requires each permission
    /// </summary>
    internal static readonly Dictionary<Permissions, (Controller Controller, Action Action, Method Method)> Endpoints = new()
    {
        [Permissions.ReadCustomFieldSetup] = (Controller.CustomFieldSetup, Action.Get, Method.GET),
        [Permissions.ReadCollectionCustomFieldSetupFromWorkspace] = (Controller.CustomFieldSetup, Action.GetCollectionFromWorkspace, Method.GET),
        [Permissions.ReadCollectionCustomFieldSetupFromProject] = (Controller.CustomFieldSetup, Action.GetCollectionFromProject, Method.GET),
        [Permissions.CreateCustomFieldSetupOnWorkspace] = (Controller.CustomFieldSetup, Action.CreateOnWorkspace, Method.POST),
        [Permissions.CreateCustomFieldSetupOnProject] = (Controller.CustomFieldSetup, Action.CreateOnProject, Method.POST),
        [Permissions.AddCustomFieldSetupToProject] = (Controller.CustomFieldSetup, Action.AddToProject, Method.POST),
        [Permissions.UpdateCustomFieldSetup] = (Controller.CustomFieldSetup, Action.Update, Method.PUT),
        [Permissions.MakeCustomFieldSetupGlobal] = (Controller.CustomFieldSetup, Action.MakeGlobal, Method.PUT),
        [Permissions.DeleteCustomFieldSetup] = (Controller.CustomFieldSetup, Action.Delete, Method.DELETE),
        [Permissions.RemoveCustomFieldSetupFromProject] = (Controller.CustomFieldSetup, Action.RemoveFromProject, Method.DELETE),
        [Permissions.ReadProject] = (Controller.Project, Action.Get, Method.GET),
        [Permissions.ReadOverviewProject] = (Controller.Project, Action.GetOverview, Method.GET),
        [Permissions.ReadCollectionProject] = (Controller.Project, Action.GetCollection, Method.GET),
        [Permissions.ReadStatusesFromProject] = (Controller.Project, Action.GetStatuses, Method.GET),
        [Permissions.ReadAllowedMembersFromProject] = (Controller.Project, Action.GetAllowedMembers, Method.GET),
        [Permissions.CreateProject] = (Controller.Project, Action.Create, Method.POST),
        [Permissions.AddStatusToProject] = (Controller.Project, Action.AddStatus, Method.POST),
        [Permissions.UpdateProject] = (Controller.Project, Action.Update, Method.PUT),
        [Permissions.UpdateFavoritesProject] = (Controller.Project, Action.UpdateFavorites, Method.PUT),
        [Permissions.UpdateOverviewProject] = (Controller.Project, Action.UpdateOverview, Method.PUT),
        [Permissions.UpdateStatusFromProject] = (Controller.Project, Action.UpdateStatus, Method.PUT),
        [Permissions.DeleteProject] = (Controller.Project, Action.Delete, Method.DELETE),
        [Permissions.RemoveStatusFromProject] = (Controller.Project, Action.RemoveStatus, Method.DELETE),
        [Permissions.ReadProjectSprint] = (Controller.ProjectSprint, Action.Get, Method.GET),
        [Permissions.ReadCollectionProjectSprint] = (Controller.ProjectSprint, Action.GetCollection, Method.GET),
        [Permissions.ReadStagesFromProjectSprint] = (Controller.ProjectSprint, Action.GetStages, Method.GET),
        [Permissions.CreateProjectSprint] = (Controller.ProjectSprint, Action.Create, Method.POST),
        [Permissions.AddStageToProjectSprint] = (Controller.ProjectSprint, Action.AddStage, Method.POST),
        [Permissions.UpdateProjectSprint] = (Controller.ProjectSprint, Action.Update, Method.PUT),
        [Permissions.MoveStageFromProjectSprint] = (Controller.ProjectSprint, Action.MoveStage, Method.POST),
        [Permissions.UpdateStageFromProjectSprint] = (Controller.ProjectSprint, Action.UpdateStage, Method.PUT),
        [Permissions.DeleteProjectSprint] = (Controller.ProjectSprint, Action.Delete, Method.DELETE),
        [Permissions.RemoveStageFromProjectSprint] = (Controller.ProjectSprint, Action.RemoveStage, Method.DELETE),
        [Permissions.ReadProjectTask] = (Controller.ProjectTask, Action.Get, Method.GET),
        [Permissions.ReadCollectionProjectTask] = (Controller.ProjectTask, Action.GetCollection, Method.GET),
        [Permissions.CreateProjectTask] = (Controller.ProjectTask, Action.Create, Method.POST),
        [Permissions.MoveProjectTask] = (Controller.ProjectTask, Action.Move, Method.POST),
        [Permissions.UpdateProjectTask] = (Controller.ProjectTask, Action.Update, Method.PUT),
        [Permissions.UpdateCustomFieldFromProjectTask] = (Controller.ProjectTask, Action.UpdateCustomField, Method.PUT),
        [Permissions.UpdateIsCompletedProjectTask] = (Controller.ProjectTask, Action.UpdateIsCompleted, Method.PUT),
        [Permissions.UpdateOverviewProjectTask] = (Controller.ProjectTask, Action.UpdateOverview, Method.PUT),
        [Permissions.DeleteProjectTask] = (Controller.ProjectTask, Action.Delete, Method.DELETE),
        [Permissions.ReadWorkspace] = (Controller.Workspace, Action.Get, Method.GET),
        [Permissions.ReadOverviewWorkspace] = (Controller.Workspace, Action.GetOverview, Method.GET),
        [Permissions.ReadCollaboratorsFromWorkspace] = (Controller.Workspace, Action.GetCollaborators, Method.GET),
        [Permissions.ReadInvitationsFromWorkspace] = (Controller.Workspace, Action.GetInvitations, Method.GET),
        [Permissions.InviteCollaboratorToWorkspace] = (Controller.Workspace, Action.InviteCollaborator, Method.POST),
        [Permissions.UpdateWorkspace] = (Controller.Workspace, Action.Update, Method.PUT),
        [Permissions.UpdateCollaboratorFromWorkspace] = (Controller.Workspace, Action.UpdateCollaborator, Method.PUT),
        [Permissions.UpdateFavoritesWorkspace] = (Controller.Workspace, Action.UpdateFavorites, Method.PUT),
        [Permissions.UpdateOverviewWorkspace] = (Controller.Workspace, Action.UpdateOverview, Method.PUT),
        [Permissions.DeleteWorkspace] = (Controller.Workspace, Action.Delete, Method.DELETE),
        [Permissions.KickCollaboratorFromWorkspace] = (Controller.Workspace, Action.KickCollaborator, Method.DELETE),
    };

    internal class GetPermissions : TheoryData<Permissions, IdLocation, string?, TargetKind>
    {
        public GetPermissions()
        {
            // Project
            Add(Permissions.CreateCustomFieldSetupOnWorkspace, IdLocation.Body, typeof(WorkspaceAggregate).ToCamelId(), TargetKind.Workspace);
            Add(Permissions.CreateCustomFieldSetupOnProject, IdLocation.Body, typeof(ProjectAggregate).ToCamelId(), TargetKind.Project);
            Add(Permissions.AddCustomFieldSetupToProject, IdLocation.Body, null, TargetKind.CustomFieldSetup);
            Add(Permissions.UpdateCustomFieldSetup, IdLocation.Body, null, TargetKind.CustomFieldSetup);
            Add(Permissions.MakeCustomFieldSetupGlobal, IdLocation.Body, null, TargetKind.CustomFieldSetup);
            Add(Permissions.ReadCollectionCustomFieldSetupFromWorkspace, IdLocation.Route, typeof(WorkspaceAggregate).ToCamelId(), TargetKind.Workspace);
            Add(Permissions.ReadCollectionCustomFieldSetupFromProject, IdLocation.Route, typeof(ProjectAggregate).ToCamelId(), TargetKind.Project);
            Add(Permissions.ReadCustomFieldSetup, IdLocation.Route, null, TargetKind.CustomFieldSetup);
            Add(Permissions.DeleteCustomFieldSetup, IdLocation.Route, null, TargetKind.CustomFieldSetup);
            Add(Permissions.RemoveCustomFieldSetupFromProject, IdLocation.Route, null, TargetKind.CustomFieldSetup);

            // Project
            Add(Permissions.CreateProject, IdLocation.Body, typeof(WorkspaceAggregate).ToCamelId(), TargetKind.Workspace);
            Add(Permissions.UpdateProject, IdLocation.Body, null, TargetKind.Project);
            Add(Permissions.UpdateFavoritesProject, IdLocation.Body, null, TargetKind.Project);
            Add(Permissions.UpdateOverviewProject, IdLocation.Body, null, TargetKind.Project);
            Add(Permissions.DeleteProject, IdLocation.Route, null, TargetKind.Project);
            Add(Permissions.ReadProject, IdLocation.Route, null, TargetKind.Project);
            Add(Permissions.ReadOverviewProject, IdLocation.Route, null, TargetKind.Project);
            Add(Permissions.ReadCollectionProject, IdLocation.Query, typeof(WorkspaceAggregate).ToCamelId(), TargetKind.Workspace);

            // Project - Allowed Members
            Add(Permissions.ReadAllowedMembersFromProject, IdLocation.Route, null, TargetKind.Project);

            // Project - Project Status
            Add(Permissions.AddStatusToProject, IdLocation.Body, null, TargetKind.Project);
            Add(Permissions.UpdateStatusFromProject, IdLocation.Body, null, TargetKind.Project);
            Add(Permissions.RemoveStatusFromProject, IdLocation.Route, null, TargetKind.Project);
            Add(Permissions.ReadStatusesFromProject, IdLocation.Route, null, TargetKind.Project);

            // Project Sprint
            Add(Permissions.CreateProjectSprint, IdLocation.Body, typeof(ProjectAggregate).ToCamelId(), TargetKind.Project);
            Add(Permissions.UpdateProjectSprint, IdLocation.Body, null, TargetKind.ProjectSprint);
            Add(Permissions.DeleteProjectSprint, IdLocation.Route, null, TargetKind.ProjectSprint);
            Add(Permissions.ReadProjectSprint, IdLocation.Route, null, TargetKind.ProjectSprint);
            Add(Permissions.ReadCollectionProjectSprint, IdLocation.Query, typeof(ProjectAggregate).ToCamelId(), TargetKind.Project);

            // Project Sprint - Project Stage
            Add(Permissions.AddStageToProjectSprint, IdLocation.Body, null, TargetKind.ProjectSprint);
            Add(Permissions.MoveStageFromProjectSprint, IdLocation.Body, null, TargetKind.ProjectSprint);
            Add(Permissions.UpdateStageFromProjectSprint, IdLocation.Body, null, TargetKind.ProjectSprint);
            Add(Permissions.RemoveStageFromProjectSprint, IdLocation.Route, null, TargetKind.ProjectSprint);
            Add(Permissions.ReadStagesFromProjectSprint, IdLocation.Route, null, TargetKind.ProjectSprint);

            // Project Task
            Add(Permissions.CreateProjectTask, IdLocation.Body, typeof(ProjectStage).ToCamelId(), TargetKind.ProjectStage);
            Add(Permissions.MoveProjectTask, IdLocation.Body, null, TargetKind.ProjectTask);
            Add(Permissions.UpdateProjectTask, IdLocation.Body, null, TargetKind.ProjectTask);
            Add(Permissions.UpdateCustomFieldFromProjectTask, IdLocation.Body, null, TargetKind.ProjectTask);
            Add(Permissions.UpdateIsCompletedProjectTask, IdLocation.Body, null, TargetKind.ProjectTask);
            Add(Permissions.UpdateOverviewProjectTask, IdLocation.Body, null, TargetKind.ProjectTask);
            Add(Permissions.DeleteProjectTask, IdLocation.Route, null, TargetKind.ProjectTask);
            Add(Permissions.ReadProjectTask, IdLocation.Route, null, TargetKind.ProjectTask);
            Add(Permissions.ReadCollectionProjectTask, IdLocation.Query, typeof(ProjectStage).ToCamelId(), TargetKind.ProjectStage);

            // Workspace
            Add(Permissions.UpdateWorkspace, IdLocation.Body, null, TargetKind.Workspace);
            Add(Permissions.UpdateFavoritesWorkspace, IdLocation.Body, null, TargetKind.Workspace);
            Add(Permissions.UpdateOverviewWorkspace, IdLocation.Body, null, TargetKind.Workspace);
            Add(Permissions.DeleteWorkspace, IdLocation.Route, null, TargetKind.Workspace);
            Add(Permissions.ReadWorkspace, IdLocation.Route, null, TargetKind.Workspace);
            Add(Permissions.ReadOverviewWorkspace, IdLocation.Route, null, TargetKind.Workspace);
            Add(Permissions.ReadInvitationsFromWorkspace, IdLocation.Query, typeof(WorkspaceAggregate).ToCamelId(), TargetKind.Workspace);

            // Workspace - Collaborators
            Add(Permissions.InviteCollaboratorToWorkspace, IdLocation.Body, null, TargetKind.Workspace);
            Add(Permissions.UpdateCollaboratorFromWorkspace, IdLocation.Body, null, TargetKind.Workspace);
            Add(Permissions.KickCollaboratorFromWorkspace, IdLocation.Route, null, TargetKind.Workspace);
            Add(Permissions.ReadCollaboratorsFromWorkspace, IdLocation.Route, null, TargetKind.Workspace);
        }
    }
}
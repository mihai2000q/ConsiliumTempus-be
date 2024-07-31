namespace ConsiliumTempus.Domain.Common.Enums;

public enum Permissions
{
    // Workspace
    ReadWorkspace,
    ReadOverviewWorkspace,
    ReadCollaboratorsFromWorkspace,
    ReadInvitationsFromWorkspace,
    InviteCollaboratorToWorkspace,
    UpdateWorkspace,
    UpdateFavoritesWorkspace,
    UpdateOverviewWorkspace,
    DeleteWorkspace,
    // Project
    CreateProject,
    ReadProject,
    ReadOverviewProject,
    ReadCollectionProject,
    UpdateProject,
    UpdateFavoritesProject,
    UpdateOverviewProject,
    DeleteProject,
    // Project - Project Status
    AddStatusToProject,
    ReadStatusesFromProject,
    UpdateStatusFromProject,
    RemoveStatusFromProject,
    // Project - Allowed Members
    ReadAllowedMembersFromProject,
    // Project Sprint
    CreateProjectSprint,
    ReadProjectSprint,
    ReadCollectionProjectSprint,
    UpdateProjectSprint,
    DeleteProjectSprint,
    // Project Sprint - Project Stage
    ReadStagesFromProjectSprint,
    AddStageToProjectSprint,
    MoveStageFromProjectSprint,
    UpdateStageFromProjectSprint,
    RemoveStageFromProjectSprint,
    // Project Task
    CreateProjectTask,
    ReadProjectTask,
    ReadCollectionProjectTask,
    MoveProjectTask,
    UpdateProjectTask,
    UpdateIsCompletedProjectTask,
    UpdateOverviewProjectTask,
    DeleteProjectTask
}
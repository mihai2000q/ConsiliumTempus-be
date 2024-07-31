namespace ConsiliumTempus.Domain.Common.Enums;

public enum Permissions
{
    // Workspace
    ReadWorkspace,
    ReadOverviewWorkspace,
    ReadInvitationsFromWorkspace,
    UpdateWorkspace,
    UpdateFavoritesWorkspace,
    UpdateOverviewWorkspace,
    DeleteWorkspace,
    // Workspace - Collaborators
    InviteCollaboratorToWorkspace,
    ReadCollaboratorsFromWorkspace,
    UpdateCollaboratorFromWorkspace,
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
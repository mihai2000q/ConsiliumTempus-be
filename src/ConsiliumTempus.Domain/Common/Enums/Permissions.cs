namespace ConsiliumTempus.Domain.Common.Enums;

public enum Permissions
{
    // Custom Field Setup
    CreateCustomFieldSetupOnProject,
    CreateCustomFieldSetupOnWorkspace,
    AddCustomFieldSetupToProject,
    ReadCustomFieldSetup,
    ReadCollectionCustomFieldSetupFromProject,
    ReadCollectionCustomFieldSetupFromWorkspace,
    UpdateCustomFieldSetup,
    UpdateWorkspaceCustomFieldSetup,
    DeleteCustomFieldSetup,
    RemoveCustomFieldSetupFromProject,
    
    // Project
    CreateProject,
    ReadProject,
    ReadOverviewProject,
    ReadCollectionProject,
    UpdateProject,
    UpdateFavoritesProject,
    UpdateOverviewProject,
    DeleteProject,

    // Project - Allowed Members
    ReadAllowedMembersFromProject,
    
    // Project - Project Status
    AddStatusToProject,
    ReadStatusesFromProject,
    UpdateStatusFromProject,
    RemoveStatusFromProject,

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
    UpdateCustomFieldFromProjectTask,
    UpdateIsCompletedProjectTask,
    UpdateOverviewProjectTask,
    DeleteProjectTask,
    
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
    KickCollaboratorFromWorkspace,
}
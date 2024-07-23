namespace ConsiliumTempus.Domain.Common.Enums;

public enum Permissions
{
    // Workspace
    ReadWorkspace,
    UpdateWorkspace,
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
    // Project Sprint
    CreateProjectSprint,
    ReadProjectSprint,
    ReadCollectionProjectSprint,
    UpdateProjectSprint,
    DeleteProjectSprint,
    // Project Sprint - Project Stage
    AddStageToProjectSprint,
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
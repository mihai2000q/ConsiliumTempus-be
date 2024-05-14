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
    ReadCollectionProject,
    UpdateProject,
    DeleteProject,
    // Project Sprint
    CreateProjectSprint,
    ReadProjectSprint,
    ReadCollectionProjectSprint,
    UpdateProjectSprint,
    DeleteProjectSprint,
    // Project Stage
    CreateProjectStage,
    UpdateProjectStage,
    DeleteProjectStage,
    // Project Task
    CreateProjectTask,
    ReadProjectTask,
    ReadCollectionProjectTask,
    UpdateProjectTask,
    DeleteProjectTask
}
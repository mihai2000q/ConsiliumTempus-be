using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint;

public static class ProjectSprintCommandFactory
{
    public static AddStageToProjectSprintCommand CreateAddStageToProjectSprintCommand(
        Guid? projectSprintId = null,
        string name = Constants.ProjectStage.Name,
        bool onTop = false)
    {
        return new AddStageToProjectSprintCommand(
            projectSprintId ?? Guid.NewGuid(),
            name,
            onTop);
    }
    
    public static CreateProjectSprintCommand CreateCreateProjectSprintCommand(
        Guid? projectId = null,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new CreateProjectSprintCommand(
            projectId ?? Guid.NewGuid(),
            name,
            startDate,
            endDate);
    }
    
    public static DeleteProjectSprintCommand CreateDeleteProjectSprintCommand(Guid? id = null)
    {
        return new DeleteProjectSprintCommand(id ?? Guid.NewGuid());
    }
    
    public static RemoveStageFromProjectSprintCommand CreateRemoveStageFromProjectSprintCommand(
        Guid? projectSprintId = null,
        Guid? stageId = null)
    {
        return new RemoveStageFromProjectSprintCommand(
            projectSprintId ?? Guid.NewGuid(),
            stageId ?? Guid.NewGuid());
    }
    
    public static UpdateProjectSprintCommand CreateUpdateProjectSprintCommand(
        Guid? id = null,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new UpdateProjectSprintCommand(
            id ?? Guid.NewGuid(),
            name,
            startDate,
            endDate);
    }
    
    public static UpdateStageFromProjectSprintCommand CreateUpdateStageFromProjectSprintCommand(
        Guid? projectSprintId = null,
        Guid? stageId = null,
        string name = Constants.ProjectSprint.Name)
    {
        return new UpdateStageFromProjectSprintCommand(
            projectSprintId ?? Guid.NewGuid(),
            stageId ?? Guid.NewGuid(),
            name);
    }
}
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskCommandFactory
{
    public static CreateProjectTaskCommand CreateCreateProjectTaskCommand(
        Guid? projectStageId = null,
        string name = Constants.ProjectTask.Name,
        bool onTop = false)
    {
        return new CreateProjectTaskCommand(
            projectStageId ?? Guid.NewGuid(),
            name,
            onTop);
    }
    
    public static MoveProjectTaskCommand CreateMoveProjectTaskCommand(
        Guid? id = null,
        Guid? overId = null)
    {
        return new MoveProjectTaskCommand(
            id ?? Guid.NewGuid(),
            overId ?? Guid.NewGuid());
    }
    
    public static DeleteProjectTaskCommand CreateDeleteProjectTaskCommand(
        Guid? id = null,
        Guid? stageId = null)
    {
        return new DeleteProjectTaskCommand(
            id ?? Guid.NewGuid(),
            stageId ?? Guid.NewGuid());
    }
    
    public static UpdateProjectTaskCommand CreateUpdateProjectTaskCommand(
        Guid? id = null,
        string name = Constants.ProjectTask.Name,
        Guid? assigneeId = null)
    {
        return new UpdateProjectTaskCommand(
            id ?? Guid.NewGuid(),
            name,
            assigneeId);
    }

    public static UpdateIsCompletedProjectTaskCommand CreateUpdateIsCompletedProjectTaskCommand(
        Guid? id = null,
        bool isCompleted = true)
    {
        return new UpdateIsCompletedProjectTaskCommand(
            id ?? Guid.NewGuid(),
            isCompleted);
    }
    
    public static UpdateOverviewProjectTaskCommand CreateUpdateOverviewProjectTaskCommand(
        Guid? id = null,
        string name = Constants.ProjectTask.Name,
        string description = Constants.ProjectTask.Description,
        Guid? assigneeId = null)
    {
        return new UpdateOverviewProjectTaskCommand(
            id ?? Guid.NewGuid(),
            name,
            description,
            assigneeId);
    }
}
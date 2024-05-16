using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
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
    
    public static DeleteProjectTaskCommand CreateDeleteProjectTaskCommand(Guid? id = null)
    {
        return new DeleteProjectTaskCommand(id ?? Guid.NewGuid());
    }
    
    public static UpdateProjectTaskCommand CreateUpdateProjectTaskCommand(
        Guid? id = null,
        string name = Constants.ProjectTask.Name,
        bool isCompleted = false,
        Guid? assigneeId = null)
    {
        return new UpdateProjectTaskCommand(
            id ?? Guid.NewGuid(),
            name,
            isCompleted,
            assigneeId);
    }
}
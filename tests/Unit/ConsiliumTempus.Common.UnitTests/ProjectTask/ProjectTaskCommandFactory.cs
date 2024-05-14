using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskCommandFactory
{
    public static CreateProjectTaskCommand CreateCreateProjectTaskCommand(
        Guid? projectStageId = null,
        string name = Constants.ProjectTask.Name)
    {
        return new CreateProjectTaskCommand(
            projectStageId ?? Guid.NewGuid(),
            name);
    }
    
    public static DeleteProjectTaskCommand CreateDeleteProjectTaskCommand(
        Guid? id = null,
        Guid? projectStageId = null)
    {
        return new DeleteProjectTaskCommand(
            id ?? Guid.NewGuid(),
            projectStageId ?? Guid.NewGuid());
    }
}
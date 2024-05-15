using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageCommandFactory
{
    public static CreateProjectStageCommand CreateCreateProjectStageCommand(
        Guid? projectSprintId = null,
        string name = Constants.ProjectStage.Name)
    {
        return new CreateProjectStageCommand(
            projectSprintId ?? Guid.NewGuid(),
            name);
    }
    
    public static UpdateProjectStageCommand CreateUpdateProjectStageCommand(
        Guid? id = null,
        string name = Constants.ProjectStage.Name)
    {
        return new UpdateProjectStageCommand(
            id ?? Guid.NewGuid(),
            name);
    }
    
    public static DeleteProjectStageCommand CreateDeleteProjectStageCommand(Guid? id = null)
    {
        return new DeleteProjectStageCommand(id ?? Guid.NewGuid());
    }
}
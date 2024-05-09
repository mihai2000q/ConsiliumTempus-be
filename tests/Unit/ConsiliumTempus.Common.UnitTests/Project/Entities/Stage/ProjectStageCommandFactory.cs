using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
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
}
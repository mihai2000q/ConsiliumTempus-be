using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageFactory
{
    public static ProjectStage Create(
        Domain.Project.Entities.ProjectSprint? sprint = null,
        string name = Constants.ProjectStage.Name,
        int customOrderPosition = 0)
    {
        return ProjectStage.Create(
            Name.Create(name),
            CustomOrderPosition.Create(customOrderPosition),
            sprint ?? ProjectSprintFactory.Create());
    }
}
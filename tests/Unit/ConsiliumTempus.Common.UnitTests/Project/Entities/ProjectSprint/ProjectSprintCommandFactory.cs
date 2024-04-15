using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;

public static class ProjectSprintCommandFactory
{
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
}
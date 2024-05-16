using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

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
    
    public static DeleteProjectSprintCommand CreateDeleteProjectSprintCommand(Guid? id = null)
    {
        return new DeleteProjectSprintCommand(id ?? Guid.NewGuid());
    }
}
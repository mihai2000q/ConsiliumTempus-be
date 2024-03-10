using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;

public static class ProjectSprintFactory
{
    public static Domain.Project.Entities.ProjectSprint Create(
        string name = "",
        ProjectAggregate? project = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return Domain.Project.Entities.ProjectSprint.Create(
            Name.Create(name), 
            project ?? ProjectFactory.Create(),
            startDate,
            endDate);
    }
}
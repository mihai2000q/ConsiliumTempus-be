using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectSprintFactory
{
    public static ProjectSprint Create(
        string name = "",
        ProjectAggregate? project = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return ProjectSprint.Create(
            Name.Create(name), 
            project ?? ProjectFactory.Create(),
            startDate,
            endDate);
    }
}
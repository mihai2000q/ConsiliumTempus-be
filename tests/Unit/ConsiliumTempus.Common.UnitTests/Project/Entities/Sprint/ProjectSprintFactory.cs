using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

public static class ProjectSprintFactory
{
    public static ProjectSprint Create(
        string name = Constants.ProjectSprint.Name,
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

    public static List<ProjectSprint> CreateList(
        int count = 5,
        string name = Constants.ProjectSprint.Name,
        ProjectAggregate? project = null)
    {
        return Enumerable
            .Range(0, count)
            .Select(i => Create(name + i, project))
            .ToList();
    }
}
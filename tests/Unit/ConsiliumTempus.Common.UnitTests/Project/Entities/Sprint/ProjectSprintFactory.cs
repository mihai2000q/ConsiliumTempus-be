using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

public static class ProjectSprintFactory
{
    public static ProjectSprintAggregate Create(
        string name = Constants.ProjectSprint.Name,
        ProjectAggregate? project = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return ProjectSprintAggregate.Create(
            Name.Create(name),
            project ?? ProjectFactory.Create(),
            startDate,
            endDate);
    }

    public static List<ProjectSprintAggregate> CreateList(
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
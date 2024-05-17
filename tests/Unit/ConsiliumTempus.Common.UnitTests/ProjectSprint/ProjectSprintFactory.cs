using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint;

public static class ProjectSprintFactory
{
    public static ProjectSprintAggregate Create(
        string name = Constants.ProjectSprint.Name,
        ProjectAggregate? project = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        int stagesCount = 5)
    {
        var sprint = ProjectSprintAggregate.Create(
            Name.Create(name),
            project ?? ProjectFactory.CreateWithSprints(),
            startDate,
            endDate);

        Enumerable
            .Range(0, stagesCount)
            .ToList()
            .ForEach(i => sprint.AddStage(ProjectStageFactory.Create(
                sprint, 
                Constants.ProjectStage.Name + i,
                i)));

        return sprint;
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
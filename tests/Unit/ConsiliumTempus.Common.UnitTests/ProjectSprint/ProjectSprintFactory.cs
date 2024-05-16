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
        DateOnly? endDate = null)
    {
        return ProjectSprintAggregate.Create(
            Name.Create(name),
            project ?? ProjectFactory.Create(),
            startDate,
            endDate);
    }

    public static ProjectSprintAggregate CreateWithStages(
        string name = Constants.ProjectSprint.Name,
        ProjectAggregate? project = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        int stagesCount = 3)
    {
        var sprint = ProjectSprintAggregate.Create(
            Name.Create(name),
            project ?? ProjectFactory.Create(),
            startDate,
            endDate);

        var customOrderPosition = 0;
        Enumerable
            .Range(0, stagesCount)
            .ToList()
            .ForEach(i => sprint.AddStage(ProjectStageFactory.Create(
                sprint, 
                Constants.ProjectStage.Name + i,
                customOrderPosition++)));

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
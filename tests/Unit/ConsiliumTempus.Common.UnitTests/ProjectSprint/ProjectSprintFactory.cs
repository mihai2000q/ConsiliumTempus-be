using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint;

public static class ProjectSprintFactory
{
    public static ProjectSprintAggregate Create(
        string name = Constants.ProjectSprint.Name,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        int stagesCount = 5,
        int sprintsCount = 5)
    {
        project ??= ProjectFactory.CreateWithSprints(sprintsCount: sprintsCount - 1);
        createdBy ??= UserFactory.Create();
        var sprint = ProjectSprintAggregate.Create(
            Name.Create(name),
            project,
            createdBy,
            startDate,
            endDate);

        project.AddSprint(sprint);
        
        ProjectStageFactory.CreateList(sprint, createdBy, stagesCount)
            .ForEach(stage => sprint.AddStage(stage));

        return sprint;
    }

    public static List<ProjectSprintAggregate> CreateList(
        int count = 5,
        string name = Constants.ProjectSprint.Name,
        ProjectAggregate? project = null,
        DateOnly? endDate = null)
    {
        return Enumerable
            .Range(0, count)
            .Select(i => Create(name + i, project, endDate: endDate))
            .ToList();
    }
}
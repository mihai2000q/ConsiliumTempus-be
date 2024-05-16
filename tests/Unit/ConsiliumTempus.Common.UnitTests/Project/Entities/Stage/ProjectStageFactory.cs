using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageFactory
{
    public static ProjectStage Create(
        ProjectSprintAggregate? sprint = null,
        string name = Constants.ProjectStage.Name,
        int customOrderPosition = 0)
    {
        return ProjectStage.Create(
            Name.Create(name),
            CustomOrderPosition.Create(customOrderPosition),
            sprint ?? ProjectSprintFactory.Create());
    }
    
    public static ProjectStage CreateWithTasks(
        int tasksCount = 5)
    {
        var stage = Create();

        Enumerable
            .Range(0, tasksCount)
            .ToList()
            .ForEach(i => stage.AddTask(ProjectTaskFactory.Create(customOrderPosition: i)));

        return stage;
    }
    
    public static ProjectStage CreateWithStages(
        int stagesCount = 5)
    {
        var sprint = ProjectSprintFactory.Create();

        var count = 0;
        Enumerable.Repeat(0, stagesCount)
            .ToList()
            .ForEach(_ => sprint.AddStage(Create(sprint, customOrderPosition: count++)));

        return sprint.Stages[0];
    }

    public static List<ProjectStage> CreateList(
        int count = 5,
        ProjectSprintAggregate? sprint = null)
    {
        return Enumerable
            .Range(0, count)
            .Select(i => Create(sprint, Constants.ProjectStage.Name + i, i))
            .ToList();
    }
}
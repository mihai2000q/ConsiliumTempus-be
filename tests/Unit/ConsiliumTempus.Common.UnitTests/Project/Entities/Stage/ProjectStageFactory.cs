using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageFactory
{
    public static ProjectStage Create(
        ProjectSprint? sprint = null,
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

        var count = 1;
        Enumerable.Repeat(0, stagesCount)
            .ToList()
            .ForEach(_ => sprint.AddStage(Create(customOrderPosition: count++)));

        return sprint.Stages[0];
    }
}
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;

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
}
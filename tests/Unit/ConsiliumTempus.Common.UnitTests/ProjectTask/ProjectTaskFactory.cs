using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskFactory
{
    public static ProjectTaskAggregate Create(
        string name = Constants.ProjectTask.Name,
        string description = Constants.ProjectTask.Description,
        int customOrderPosition = 0,
        ProjectStage? stage = null,
        UserAggregate? createdBy = null)
    {
        return ProjectTaskAggregate.Create(
            Name.Create(name), 
            Description.Create(description), 
            CustomOrderPosition.Create(customOrderPosition), 
            createdBy ?? UserFactory.Create(),
            stage ?? ProjectStageFactory.Create());
    }

    public static ProjectTaskAggregate CreateWithTasks(
        int tasksCount = 5)
    {
        var stage = ProjectStageFactory.Create();

        Enumerable
            .Range(0, tasksCount)
            .ToList()
            .ForEach(i => stage.AddTask(Create(
                Constants.ProjectTask.Name + i,
                customOrderPosition: i,
                stage: stage)));
        
        return stage.Tasks[0];
    }
    
    public static List<ProjectTaskAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => Create())
            .ToList();
    }
}
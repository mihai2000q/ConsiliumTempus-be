using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;

public static class ProjectStageFactory
{
    public static ProjectStage Create(
        ProjectSprintAggregate? sprint = null,
        string name = Constants.ProjectStage.Name,
        int customOrderPosition = 0,
        UserAggregate? createdBy = null)
    {
        return ProjectStage.Create(
            Name.Create(name),
            CustomOrderPosition.Create(customOrderPosition),
            sprint ?? ProjectSprintFactory.Create(),
            createdBy ?? UserFactory.Create());
    }
    
    public static ProjectStage CreateWithTasks(
        ProjectSprintAggregate? sprint = null,
        UserAggregate? createdBy = null,
        string name = Constants.ProjectStage.Name,
        int customOrderPosition = 0,
        int tasksCount = 5)
    {
        var stage = Create(
            sprint: sprint,
            name: name,
            createdBy: createdBy,
            customOrderPosition: customOrderPosition);

        Enumerable
            .Range(0, tasksCount)
            .ToList()
            .ForEach(i => stage.AddTask(ProjectTaskFactory.Create(
                name: Constants.ProjectTask.Name + i,
                customOrderPosition: i, 
                stage: stage,
                createdBy: createdBy)));

        return stage;
    }

    public static List<ProjectStage> CreateList(
        ProjectSprintAggregate? sprint = null,
        UserAggregate? createdBy = null,
        int stagesCount = 5)
    {
        return Enumerable
            .Range(0, stagesCount)
            .Select(i => Create(
                sprint, 
                Constants.ProjectStage.Name + i,
                i,
                createdBy))
            .ToList();
    }
    
    public static List<ProjectStage> CreateListWithTasks(
        ProjectSprintAggregate? sprint = null,
        UserAggregate? createdBy = null,
        int stagesCount = 5,
        int tasksCount = 5)
    {
        return Enumerable
            .Range(0, stagesCount)
            .Select(i => CreateWithTasks(
                sprint: sprint,
                name: Constants.ProjectStage.Name + i,
                customOrderPosition: i,
                createdBy: createdBy, 
                tasksCount: tasksCount))
            .ToList();
    }
}
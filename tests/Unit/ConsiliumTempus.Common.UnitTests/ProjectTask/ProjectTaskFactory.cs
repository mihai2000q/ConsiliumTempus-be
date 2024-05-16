using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
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
        int customOrderPosition = 1,
        ProjectStage? stage = null,
        UserAggregate? user = null)
    {
        return ProjectTaskAggregate.Create(
            Name.Create(name), 
            Description.Create(description), 
            CustomOrderPosition.Create(customOrderPosition), 
            user ?? UserFactory.Create(),
            stage ?? ProjectStageFactory.Create());
    }
    
    public static List<ProjectTaskAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => Create())
            .ToList();
    }
}
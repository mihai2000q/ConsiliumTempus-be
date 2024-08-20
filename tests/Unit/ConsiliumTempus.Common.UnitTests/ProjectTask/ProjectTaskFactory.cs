using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;
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
        var task = ProjectTaskAggregate.Create(
            Name.Create(name),
            Description.Create(description),
            CustomOrderPosition.Create(customOrderPosition),
            createdBy ?? UserFactory.Create(),
            stage ?? ProjectStageFactory.Create());

        task.ClearDomainEvents();

        return task;
    }

    public static ProjectTaskAggregate CreateWithCustomFields(
        string name = Constants.ProjectTask.Name,
        string description = Constants.ProjectTask.Description,
        int customOrderPosition = 0,
        ProjectStage? stage = null,
        UserAggregate? createdBy = null)
    {
        var task = ProjectTaskAggregate.Create(
            Name.Create(name),
            Description.Create(description),
            CustomOrderPosition.Create(customOrderPosition),
            createdBy ?? UserFactory.Create(),
            stage ?? ProjectStageFactory.Create());

        task.AddCustomField(CustomFieldFactory.CreateNumber(12, task: task));
        var singleSelectSetup = CustomFieldSetupFactory.CreateSingleSelect();
        task.AddCustomField(CustomFieldFactory.CreateSingleSelect(
            singleSelectSetup.Options[0].Id,
            setup: singleSelectSetup,
            task: task));
        task.AddCustomField(CustomFieldFactory.CreateText("Some text", task: task));

        task.ClearDomainEvents();

        return task;
    }

    public static List<ProjectTaskAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => Create())
            .ToList();
    }

    public static List<ProjectTaskAggregate> CreateListWithCustomFields(int count = 5)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => CreateWithCustomFields())
            .ToList();
    }
}
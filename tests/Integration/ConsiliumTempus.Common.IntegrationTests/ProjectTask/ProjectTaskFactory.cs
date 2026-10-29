using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectTask;

public static class ProjectTaskFactory
{
    public static ProjectTaskAggregate Create(
        ProjectStage stage,
        Audit audit,
        string name = Constants.ProjectTask.Name,
        string description = Constants.ProjectTask.Description,
        int customOrderPosition = 0,
        bool isCompleted = false,
        UserAggregate? assignee = null,
        UserAggregate? reviewer = null)
    {
        return EntityBuilder<ProjectTaskAggregate>.Empty()
            .WithProperty(nameof(ProjectTaskAggregate.Id), ProjectTaskId.CreateUnique())
            .WithProperty(nameof(ProjectTaskAggregate.Name), Name.Create(name))
            .WithProperty(nameof(ProjectTaskAggregate.Description), Description.Create(description))
            .WithProperty(nameof(ProjectTaskAggregate.CustomOrderPosition), CustomOrderPosition.Create(customOrderPosition))
            .WithProperty(nameof(ProjectTaskAggregate.IsCompleted), IsCompleted.Create(isCompleted))
            .WithProperty(nameof(ProjectTaskAggregate.Assignee), assignee)
            .WithProperty(nameof(ProjectTaskAggregate.Reviewer), reviewer)
            .WithProperty(nameof(ProjectTaskAggregate.Audit), audit)
            .WithProperty(nameof(ProjectTaskAggregate.Stage), stage)
            .Build();
    }
}
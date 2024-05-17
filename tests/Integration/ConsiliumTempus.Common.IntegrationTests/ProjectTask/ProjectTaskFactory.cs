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
        UserAggregate createdBy,
        ProjectStage stage,
        string name = Constants.ProjectTask.Name,
        string description = Constants.ProjectTask.Description,
        int customOrderPosition = 0,
        bool isCompleted = false,
        UserAggregate? assignee = null,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        var task = DomainFactory.GetObjectInstance<ProjectTaskAggregate>();

        DomainFactory.SetProperty(ref task, nameof(task.Id), ProjectTaskId.CreateUnique());
        DomainFactory.SetProperty(ref task, nameof(task.Name), Name.Create(name));
        DomainFactory.SetProperty(ref task, nameof(task.Description), Description.Create(description));
        DomainFactory.SetProperty(ref task, nameof(task.CustomOrderPosition), CustomOrderPosition.Create(customOrderPosition));
        DomainFactory.SetProperty(ref task, nameof(task.IsCompleted), IsCompleted.Create(isCompleted));
        DomainFactory.SetProperty(ref task, nameof(task.IsCompleted), IsCompleted.Create(isCompleted));
        DomainFactory.SetProperty(ref task, nameof(task.CreatedBy), createdBy);
        DomainFactory.SetProperty(ref task, nameof(task.Assignee), assignee);
        DomainFactory.SetProperty(ref task, nameof(task.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref task, nameof(task.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref task, nameof(task.Stage), stage);

        return task;
    }
}
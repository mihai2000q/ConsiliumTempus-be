using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectTask.Entities;

public static class ProjectTaskCommentFactory
{
    public static ProjectTaskComment Create(
        ProjectTaskAggregate task,
        UserAggregate createdBy,
        string message = Constants.ProjectTaskComment.Message)
    {
        return EntityBuilder<ProjectTaskComment>.Empty()
            .WithProperty(nameof(ProjectTaskComment.Id), ProjectTaskCommentId.CreateUnique())
            .WithProperty(nameof(ProjectTaskComment.Message), Message.Create(message))
            .WithProperty(nameof(ProjectTaskComment.CreatedBy), createdBy)
            .WithProperty(nameof(ProjectTaskComment.Task), task)
            .WithProperty(nameof(ProjectTaskComment.CreatedDateTime), DateTime.UtcNow)
            .WithProperty(nameof(ProjectTaskComment.UpdatedDateTime), DateTime.UtcNow)
            .Build();
    }
}
using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectTask.Entities;

public static class ProjectTaskCommentFactory
{
    public static ProjectTaskComment Create(
        ProjectTaskAggregate task,
        Authorship authorship,
        string message = Constants.ProjectTaskComment.Message)
    {
        return EntityBuilder<ProjectTaskComment>.Empty()
            .WithProperty(nameof(ProjectTaskComment.Id), ProjectTaskCommentId.CreateUnique())
            .WithProperty(nameof(ProjectTaskComment.Message), Message.Create(message))
            .WithProperty(nameof(ProjectTaskComment.Task), task)
            .WithProperty(nameof(ProjectTaskComment.Authorship), authorship)
            .Build();
    }
}

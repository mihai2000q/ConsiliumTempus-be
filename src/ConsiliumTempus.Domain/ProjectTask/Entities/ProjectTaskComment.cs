using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class ProjectTaskComment : Entity<ProjectTaskCommentId>, IAuthored
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectTaskComment()
    {
    }

    private ProjectTaskComment(
        ProjectTaskCommentId id,
        Message message,
        ProjectTaskAggregate task) : base(id)
    {
        Message = message;
        Task = task;
    }

    public Message Message { get; private set; } = null!;
    public DateOnly? Date { get; private set; }
    public TimeSpan? TimeSpent { get; private set; }
    public ProjectTaskAggregate Task { get; init; } = null!;
    public Authorship Authorship { get; init; } = null!;

    public static ProjectTaskComment Create(
        Message message,
        ProjectTaskAggregate task)
    {
        return new ProjectTaskComment(
            ProjectTaskCommentId.CreateUnique(),
            message,
            task);
    }
}

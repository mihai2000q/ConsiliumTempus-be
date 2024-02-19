using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class ProjectTaskComment : Entity<ProjectTaskCommentId>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectTaskComment()
    {
    }

    private ProjectTaskComment(
        string message,
        ProjectTaskAggregate taskAggregate,
        DateTime createdDateTime,
        DateTime updatedDateTime)
    {
        Message = message;
        TaskAggregate = taskAggregate;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }
    
    public string Message { get; private set; } = string.Empty;
    public UserAggregate CreatedBy { get; init; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public DateOnly? Date { get; private set; }
    public TimeSpan? TimeSpent { get; private set; }
    public ProjectTaskAggregate TaskAggregate { get; init; } = default!;

    public static ProjectTaskComment Create(
        string message,
        ProjectTaskAggregate taskAggregate)
    {
        return new ProjectTaskComment(
            message,
            taskAggregate,
            DateTime.UtcNow, 
            DateTime.UtcNow);
    }
}
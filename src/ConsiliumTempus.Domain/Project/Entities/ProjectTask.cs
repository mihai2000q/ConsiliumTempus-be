using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectTask : Entity<ProjectTaskId>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectTask()
    {
    }

    private ProjectTask(
        ProjectTaskId id,
        string name,
        string description,
        bool isCompleted,
        UserAggregate createdBy,
        ProjectSection section,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
        CreatedBy = createdBy;
        Section = section;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectTaskComment> _comments = [];
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public UserAggregate CreatedBy { get; private set; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public UserAggregate? Asignee { get; private set; }
    public UserAggregate? Reviewer { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public TimeSpan? EstimatedDuration { get; private set; }
    public ProjectSection Section { get; private set; } = default!;
    public IReadOnlyList<ProjectTaskComment> Comments => _comments.AsReadOnly();

    public static ProjectTask Create(
        string name,
        string description,
        UserAggregate createdBy,
        ProjectSection section)
    {
        return new ProjectTask(
            ProjectTaskId.CreateUnique(), 
            name,
            description,
            false,
            createdBy,
            section,
            DateTime.UtcNow, 
            DateTime.UtcNow);
    }

    public void AddComment(ProjectTaskComment comment)
    {
        _comments.Add(comment);
    }
}
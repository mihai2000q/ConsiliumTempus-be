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
        string name,
        string description,
        UserAggregate createdBy,
        ProjectSection section,
        DateTime createdDateTime,
        DateTime updatedDateTime)
    {
        Name = name;
        Description = description;
        CreatedBy = createdBy;
        Section = section;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectTaskComment> _comments = [];
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public UserAggregate CreatedBy { get; private set; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public UserAggregate? Asignee { get; private set; }
    public UserAggregate? Reviewer { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public TimeSpan? EstimatedDuration { get; private set; }
    public ProjectSection Section { get; init; } = default!;
    public IReadOnlyList<ProjectTaskComment> Comments => _comments.AsReadOnly();

    public static ProjectTask Create(
        string name,
        string description,
        UserAggregate createdBy,
        ProjectSection section)
    {
        return new ProjectTask(
            name,
            description,
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
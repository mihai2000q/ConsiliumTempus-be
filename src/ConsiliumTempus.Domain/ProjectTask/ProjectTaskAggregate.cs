using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.ProjectTask;

public sealed class ProjectTaskAggregate : AggregateRoot<ProjectTaskId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectTaskAggregate()
    {
    }

    private ProjectTaskAggregate(
        ProjectTaskId id,
        string name,
        string description,
        bool isCompleted,
        int order,
        UserAggregate createdBy,
        ProjectSection section,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
        Order = order;
        CreatedBy = createdBy;
        Section = section;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectTaskComment> _comments = [];

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public int Order { get; private set; }
    public UserAggregate CreatedBy { get; init; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public UserAggregate? Asignee { get; private set; }
    public UserAggregate? Reviewer { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public TimeSpan? EstimatedDuration { get; private set; }
    public ProjectSection Section { get; private set; } = default!;
    public IReadOnlyList<ProjectTaskComment> Comments => _comments.AsReadOnly();

    public static ProjectTaskAggregate Create(
        string name,
        string description,
        int order,
        UserAggregate createdBy,
        ProjectSection section)
    {
        return new ProjectTaskAggregate(
            ProjectTaskId.CreateUnique(),
            name,
            description,
            false,
            order,
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
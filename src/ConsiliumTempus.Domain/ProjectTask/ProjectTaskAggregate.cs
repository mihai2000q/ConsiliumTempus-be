using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.ProjectTask;

public sealed class ProjectTaskAggregate : AggregateRoot<ProjectTaskId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectTaskAggregate()
    {
    }

    private ProjectTaskAggregate(
        ProjectTaskId id,
        Name name,
        Description description,
        IsCompleted isCompleted,
        Order order,
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

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public IsCompleted IsCompleted { get; private set; } = default!;
    public Order Order { get; private set; } = default!;
    public UserAggregate CreatedBy { get; init; } = default!;
    public UserAggregate? Asignee { get; }
    public UserAggregate? Reviewer { get; }
    public DateOnly? DueDate { get; }
    public TimeSpan? EstimatedDuration { get; }
    public ProjectSection Section { get; private set; } = default!;
    public IReadOnlyList<ProjectTaskComment> Comments => _comments.AsReadOnly();
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; }

    public static ProjectTaskAggregate Create(
        Name name,
        Description description,
        Order order,
        UserAggregate createdBy,
        ProjectSection section)
    {
        return new ProjectTaskAggregate(
            ProjectTaskId.CreateUnique(),
            name,
            description,
            IsCompleted.Create(false),
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